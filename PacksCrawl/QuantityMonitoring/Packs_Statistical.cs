using HtmlAgilityPack;
using PacksCrawl.Common;
using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacksCrawl.QuantityMonitoring
{
    [Map("packs_statistical")]
    public class Packs_Statistical : TracableObject
    {
        #region 属性
        [Map("id")]
        public int Id { get; set; }

        [Map("packslistconfig_id")]
        public int ListConfigId { get; set; }

        [Map("game_name")]
        public string GameName { get; set; }

        [Map("packs_url")]
        public string PackageUrl { get; set; }

        [Map("total")]
        public string Total { get; set; }

        [Map("surplus")]
        public string Surplus { get; set; }

        [Map("send_count")]
        public string GrantCount { get; set; }

        [Map("star_time")]
        public DateTime StartTime { get; set; }

        [Map("current_time")]
        public DateTime CurrentTime { get; set; }
        #endregion

        #region 初始化HTML页文档的HtmlDocument
        private static HtmlDocument getDoc(string url)
        {
            HtmlDocument doc = new HtmlDocument();
            try
            {
                if (url != null)
                {
                    //下载html源码
                    string htmlStr = HtmlAgilityPackHelper.getHtml(url, "");
                    doc.LoadHtml(htmlStr);
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("初始化HtmlDocument出错" + ex.ToString() + url, "Log\\error.log");//
            }

            return doc;
        }
        #endregion

        #region 游戏名
        private string getGameName(HtmlDocument doc, PacksConfig_Statistical config, string url)
        {
            HtmlNode node = null;
            string _Game = "";
            try
            {
                if (doc != null && config.GameNameXpath != null)
                {
                    node = doc.DocumentNode.SelectSingleNode(config.GameNameXpath);
                    if (node != null)
                    {
                        _Game = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓游戏名出错" + ex.ToString() + url, "Log\\error.log");//
            }
            return _Game.Trim();
        }
        #endregion

        #region 礼包总数
        private string getTotal(HtmlDocument doc, PacksConfig_Statistical config, string url)
        {
            HtmlNode node = null;
            string _Total = "";
            try
            {
                if (doc != null && config.TotalXpath != null)
                {
                    node = doc.DocumentNode.SelectSingleNode(config.TotalXpath);
                    if (node != null)
                    {
                        _Total = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓礼包总数出错" + ex.ToString() + url, "Log\\error.log");//
            }
            if (_Total == "")
                return "-1";
            if (_Total.Contains("%"))
                return "100%";
            return _Total.Trim();
        }
        #endregion

        #region 礼包剩余数
        private string getSurplus(HtmlDocument doc, PacksConfig_Statistical config, string url)
        {
            HtmlNode node = null;
            string _Surplus = "";
            try
            {
                if (doc != null && config.SurplusXpath != null)
                {
                    node = doc.DocumentNode.SelectSingleNode(config.SurplusXpath);
                    if (node != null)
                    {
                        _Surplus = node.InnerText;
                        _Surplus = System.Text.RegularExpressions.Regex.Replace(_Surplus, @"[\u4e00-\u9fa5]*", "").Replace("：", "");
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓礼包剩余数出错" + ex.ToString() + url, "Log\\error.log");//
            }
            if (_Surplus == "")
                return "-1";
            return _Surplus.Trim();
        }
        #endregion

        #region 礼包发放数量
        private string SendCount(HtmlDocument doc, string xpath, string url)
        {
            HtmlNode node = null;
            string _Surplus = "";
            try
            {
                if (doc != null && xpath != null)
                {
                    node = doc.DocumentNode.SelectSingleNode(xpath);
                    if (node != null)
                    {
                        _Surplus = node.InnerText;
                        _Surplus = System.Text.RegularExpressions.Regex.Replace(_Surplus, @"[\u4e00-\u9fa5]*", "").Replace("：", "");
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓礼包发放数量出错" + ex.ToString() + url, "Log\\error.log");//
            }
            if (_Surplus == "")
                return "-1";
            return _Surplus.Trim();
        } 
        #endregion


        #region 获得礼包
        /// <summary>
        /// 获得礼包
        /// </summary>
        /// <param name="url">礼包地址</param>
        /// <param name="pcfig">礼包字段配置</param>
        /// <returns>礼包对象</returns>
        public Packs_Statistical GetPack(string url, PacksConfig_Statistical pcfig)
        {
            Packs_Statistical pck = new Packs_Statistical();
            HtmlDocument doc = null;
            if (!string.IsNullOrEmpty(url) || url != "")
            {
                doc = getDoc(url);
            }

            if (doc != null)
            {
                pck.ListConfigId = pcfig.ListConfigId;
                pck.GameName = getGameName(doc, pcfig, url);
                pck.PackageUrl = url;
                pck.Total = getTotal(doc, pcfig, url);
                pck.Surplus = getSurplus(doc, pcfig, url);
                pck.GrantCount = "";
                pck.StartTime = DateTime.Now;
                pck.CurrentTime = DateTime.Now;

                var p = Table.Object<Packs_Statistical>()
                             .Where(m => m.PackageUrl, url)
                             .SelectList().FirstOrDefault();
                if (p != null)
                {
                    if (p.Surplus.Contains("%") && pck.Surplus.Contains("%"))
                    {
                        string pS = p.Surplus.Replace("%", "");
                        string PcS = pck.Surplus.Replace("%", "");
                        pck.GrantCount = SendCount(doc, "//div[@class='bd fa'][1]//b[@id='gotCnt']", url); //(int.Parse(pS) - int.Parse(PcS)).ToString() + "%";
                    }
                    else
                    {
                        pck.GrantCount = (int.Parse(p.Surplus) - int.Parse(pck.Surplus)).ToString();
                    }
                }
                else
                {
                    pck.GrantCount = "0";
                }
            }
            return pck;
        }
        #endregion

        #region 插入礼包数据
        /// <summary>
        /// 插入礼包数据
        /// </summary>
        /// <param name="pck">礼包对象</param>
        public void InsertData()
        {
            if (this != null)
            {
                var o = Table.Object<Packs_Statistical>()
                              .Where(m => m.PackageUrl, this.PackageUrl)
                              .SelectList().FirstOrDefault();

                if (o != null)
                {
                    o.StartTime = o.CurrentTime;
                    o.CurrentTime = DateTime.Now;
                    o.Surplus = this.Surplus;
                    o.GrantCount = this.GrantCount;
                    DataAccess.Update(o);
                }
                else
                {
                    if (this.Surplus != "-1" && this.Surplus != "0" && this.Surplus != "0%")
                    {
                        DataAccess.Insert(this);
                    }
                }
            }
        }
        #endregion

    }
}
