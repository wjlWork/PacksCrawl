using HtmlAgilityPack;
using PacksModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacksCrawl
{
    public class PacksList
    {

        //获得全部列表页面的礼包URL
        public static List<string> allPackUrl(string url, PacksConfig config)
        {
            //第一页
            List<string> als = new List<string>();
            List<string> ls = GetPackUrl(url, config);
            als = ls;
            //下一页
            
            string nextUrl = NextPage(url, config);     
            while (nextUrl != null && !string.IsNullOrEmpty(nextUrl))
            {   
                ls = GetPackUrl(nextUrl, config);
                als.AddRange(ls);
                nextUrl = NextPage(nextUrl, config);
            }

            return als;
        }


        //获得一个列表页面的礼包URL
        public static List<string> GetPackUrl(string url, PacksConfig config)
        {
            List<string> ls = new List<string>();
            //获得页面源码
            HtmlNodeCollection nodes = HtmlAgilityPackHelper.GetHtmlNodes(url, config.ListXpath);
            if (nodes != null)
            {
                foreach (var n in nodes)
                {
                    if ((n.InnerText != "淘号" && n.InnerText != "预定" && n.InnerText != "结束" && n.InnerText != "无号") || n.InnerText == "领取")
                    {
                        string href = n.Attributes["href"].Value;
                        string up_href = UpdateUrl(url, href);
                        if (up_href!="")
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


        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="a_Page"></param>
        /// <returns></returns>
        private static string NextPage(string a_PageUrl, PacksConfig config)
        {
            //下一页
            string Variable = "";
            string nextPage = null;
            HtmlNodeCollection nodes = HtmlAgilityPackHelper.GetHtmlNodes(a_PageUrl, config.PageXpath);

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
                                Variable = UpdateUrl(a_PageUrl, u);
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


        //修正路径，将相对路径更新为绝对路径
        public static string UpdateUrl(string sour_url, string herf)
        {
            string url = "";

            if (herf.Contains("http://"))
            {
                url = herf;
            }
            else
            {
                if (herf.Substring(0, 1) == "/")
                {
                    //主页
                    string[] us = sour_url.Split('/');
                    string u = us[0] + "/" + us[1] + "/" + us[2];
                    //
                    url = u + herf;
                }
                else if (herf.Substring(0, 1) == ".")
                {
                    if (herf.Substring(0, 2) == "./")
                    {
                        //主页
                        string[] us = sour_url.Split('/');
                        string u = "";
                        for (int i = 0; i < us.Length - 1; i++)
                        {
                            u += us[i] + "/";
                        }
                        int count = herf.Length;
                        url = u + herf.Substring(2, count - 2);

                    }
                    else if (herf.Substring(0, 3) == "../")
                    {
                        //主页
                        string[] us = sour_url.Split('/');
                        string u = "";
                        for (int i = 0; i < us.Length - 2; i++)
                        {
                            u += us[i] + "/";
                        }
                        int count = herf.Length;
                        url = u + herf.Substring(3, count - 3);
                    }
                }
                else
                {
                    if (herf.Contains(".html") || herf.Contains(".php") || herf.Contains(".aspx") || herf.Contains("/"))
                    {
                        //主页
                        string[] us = sour_url.Split('/');
                        string u = "";
                        for (int i = 0; i < us.Length - 1; i++)
                        {
                            u += us[i] + "/";
                        }
                        url = u + herf;
                    }
                }
            }


            if (url != null && !string.IsNullOrEmpty(url))
            {
                if (url.Contains("&amp;"))
                {
                    string[] np = url.Split(';');
                    url = np[0].Replace("amp", np[1]);
                }
            }

            return url;

        }















    }
}
