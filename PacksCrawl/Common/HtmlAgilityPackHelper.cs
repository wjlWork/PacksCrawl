using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using Mozilla.NUniversalCharDet;
using System.Threading;

namespace PacksCrawl.Common
{
    public class HtmlAgilityPackHelper
    {
        public static string getHtml(string url, string charSet)
        {
            string html = QueryHtml(url, charSet);
            while (html == "isExp" || html == null)
            {
                html = QueryHtml(url, charSet);
            }

            return html;
        }

        //获取网页源码
        public static string QueryHtml(string url, string charSet)
        {
            bool isExp = false;

            Byte[] pageData = null;
            XWebClient wc = new XWebClient();
            try
            {
                if (url == null || url.Trim() == "")
                    return null;
                //XWebClient wc = new XWebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Headers["User-Agent"] = "blah";

                //Helpers.WriteLog("下载html资源开始：" + url, "Log\\error.log");
                pageData = wc.DownloadData(url);
                //Helpers.WriteLog("下载html资源结束：" + url, "Log\\error.log");

            }
            catch (WebException ex)
            {
                isExp = true;
                if (ex.ToString().Contains("未能解析此远程名称"))
                {
                    Helpers.WriteLog("未能解析此远程名称，请检查网络，正在重试下载此资源...：" + url, "Log\\error.log");
                }
                else if (ex.ToString().Contains("操作超时") || ex.ToString().Contains("操作已超时"))
                {
                    Helpers.WriteLog("操作超时，请检查资源请求频率，正在重试下载此资源...：" + url, "Log\\error.log");
                }
                else
                {
                    Helpers.WriteLog("发送请求期间异常，请检查网络:" + ex.ToString(), "Log\\error.log");
                }
                //释放资源
                wc.Dispose();

                Helpers.WriteLog("释放资源等1分钟重试：" + url, "Log\\error.log");//
                System.Threading.Thread.Sleep(120000); //延时1分钟
                Helpers.WriteLog("开始重试：" + url, "Log\\error.log");//
            }
            if (pageData == null)
            {
                return null;
            }
            else if (isExp)
            {
                return "isExp";
            }

            //如果编码为空，则采用自动探测编码
            if (charSet == null || string.IsNullOrEmpty(charSet))
            {
                charSet = DetectEncoding_Bytes(pageData);
            }

            if (url == "http://act3.games.qq.com/10337/" || url == "http://www.91weile.com/kfb/indexrmtj.shtml")
            {
                return Encoding.Default.GetString(pageData);
            }

            //System.Threading.Thread.Sleep(60000); //延时1分钟
            return Encoding.GetEncoding(charSet).GetString(pageData);
        }

        //自动探测编码
        public static string DetectEncoding_Bytes(byte[] DetectBuff)
        {
            //int nDetLen = 0;
            UniversalDetector Det = new UniversalDetector(null);
            //while (!Det.IsDone())
            {
                Det.HandleData(DetectBuff, 0, DetectBuff.Length);
            }
            Det.DataEnd();
            if (Det.GetDetectedCharset() != null)
            {
                return Det.GetDetectedCharset();
            }

            return "utf-8";
        }


        /// <summary>
        /// 判断是否有乱码
        /// </summary>
        /// <param name="txt">需判断的文本</param>
        /// <returns></returns>
        private static bool isLuan(string txt)
        {
            var bytes = Encoding.UTF8.GetBytes(txt);
            //239 191 189
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 3)
                    if (bytes[i] == 239 && bytes[i + 1] == 191 && bytes[i + 2] == 189)
                    {
                        return true;
                    }
            }
            return false;
        }


        /**/
        //// <summary>
        /// 判断句子中是否含有中文
        /// </summary>
        /// <param >字符串</param>
        private static bool WordsIScn(string words)
        {
            string TmmP;
            for (int i = 0; i < words.Length; i++)
            {
                TmmP = words.Substring(i, 1);
                byte[] sarr = System.Text.Encoding.GetEncoding("gb2312").GetBytes(TmmP);
                if (sarr.Length == 2)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// 获得html代码块的节点集合
        /// </summary>
        /// <param name="url"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static HtmlNodeCollection GetHtmlNodes(string url, string xpath)
        {
            HtmlNodeCollection navNodes = null;
            try
            {
                //获取html源码
                string htmlStr = getHtml(url.Trim(), "");
                //实例化HtmlAgilityPack.HtmlDocument对象
                HtmlDocument doc = new HtmlDocument();
                //载入HTML
                doc.LoadHtml(htmlStr);

                //根据Xpath节点NODE的ID获取节点集
                navNodes = doc.DocumentNode.SelectNodes(xpath);
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("获取节点集异常:" + ex.ToString() + ":" + url, "Log\\error.log");
            }
            return navNodes;
        }

        /// <summary>
        /// 获得html代码的节点
        /// </summary>
        /// <param name="url"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static HtmlNode GetNode(HtmlDocument doc, string xpath)
        {
            //根据节点
            HtmlNode navNode = null;
            try
            {
                navNode = doc.DocumentNode.SelectSingleNode(xpath);
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("获取单节点异常:" + ex.ToString(), "Log\\error.log");
            }
            return navNode;
        }








    }
}
