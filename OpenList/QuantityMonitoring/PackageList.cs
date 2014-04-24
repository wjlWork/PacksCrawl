using HtmlAgilityPack;
using PacksModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenList.QuantityMonitoring
{
    public class PackageList
    {
        public ListConfig Lc { get; set; }

        public int Id { get; set; }
        public string ListUrl { get; set; }
        public int PackCount { get; set; }
        public List<string> PackUrlList { get; set; }

        public PackageList(string url,ListConfig lc)
        {
            this.ListUrl = url;
            this.Lc = lc;
            this.PackUrlList = allPackUrl(url, lc);
            this.PackCount = PackUrlList.Count;
            
        }

        #region 获得全部列表页面的礼包URL
        //获得全部列表页面的礼包URL
        private List<string> allPackUrl(string url, ListConfig config)
        {
            //第一页
            List<string> als = new List<string>();
            List<string> ls = GetPackUrls(url, config);
            als = ls;
            //下一页

            string nextUrl = NextPage(url, config);
            while (nextUrl != null && !string.IsNullOrEmpty(nextUrl))
            {
                ls = GetPackUrls(nextUrl, config);
                als.AddRange(ls);
                nextUrl = NextPage(nextUrl, config);
            }

            return als;
        } 
        #endregion

        #region 获得一个列表页面的礼包URL
        //获得一个列表页面的礼包URL
        private List<string> GetPackUrls(string url, ListConfig config)
        {
            List<string> ls = new List<string>();
            //获得页面源码
            HtmlNodeCollection nodes = HtmlAgilityPackHelper.GetHtmlNodes(url, config.ListPackUrlXpath);
            if (nodes != null)
            {
                foreach (var n in nodes)
                {
                    if ((n.InnerText != "淘号" && n.InnerText != "预定" && n.InnerText != "结束" && n.InnerText != "无号") || n.InnerText == "领取")
                    {
                        string href = n.Attributes["href"].Value;
                        string up_href = ConnonHelper.UpdateUrl(url, href);
                        if (up_href != "")
                        {
                            ls.Add(up_href);
                        }
                    }
                    //Helpers.WriteLog(href, "Log\\error.log");
                }
                //div[@class='zmrv_lst']//dl//dt//a[2]
            }
            return ls;
        } 
        #endregion

        #region 下一页
        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="a_Page"></param>
        /// <returns></returns>
        private static string NextPage(string a_PageUrl, ListConfig config)
        {
            //下一页
            string Variable = "";
            string nextPage = null;
            HtmlNodeCollection nodes = HtmlAgilityPackHelper.GetHtmlNodes(a_PageUrl, config.NextPageXpath);

            try
            {
                foreach (var n in nodes)
                {
                    if (n.Attributes.Count == 0 || n.Attributes["href"] == null)
                    {
                    }
                    else
                    {
                        string u = n.Attributes["href"].Value;
                        if (u != null || !string.IsNullOrEmpty(u))
                        {
                            if (n.InnerText.Contains("下一页") || n.InnerText == "下一页" || n.InnerText.Equals("下一页") || n.InnerText.Equals("&gt;") || n.InnerText.Contains("下页") || n.InnerText.Contains("&#8250;") || n.InnerHtml.Contains("下一页"))
                            {
                                Variable = ConnonHelper.UpdateUrl(a_PageUrl, u);
                                if (a_PageUrl != Variable)
                                {
                                    nextPage = Variable;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DateTime time = DateTime.Now;
                Helpers.WriteLog("获取下一页错误:" + ex.ToString() + a_PageUrl, "Log\\error.log");
            }

            return nextPage;
        } 
        #endregion




    }
}
