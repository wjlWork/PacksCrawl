using HtmlAgilityPack;
using PacksModels;
using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenList.QuantityMonitoring
{
    [Map("package")]
    public class Package:TracableObject
    {
        #region 属性
        [Map("id")]
        public int Id { get; set; }

        [Map("listconfig_id")]
        public int ListconfigId { get; set; }

        [Map("game_name")]
        public string GameName { get; set; }

        [Map("package_url")]
        public string PackageUrl { get; set; }

        [Map("total")]
        public string Total { get; set; }

        [Map("surplus")]
        public string Surplus { get; set; }

        [Map("grantcount")]
        public string GrantCount { get; set; }

        [Map("start_time")]
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
        private string getGameName(HtmlDocument doc, PackageConfig config, string url)
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
        private string getTotal(HtmlDocument doc, PackageConfig config, string url)
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
                Helpers.WriteLog("抓游戏名出错" + ex.ToString() + url, "Log\\error.log");//
            }
            if (_Total == "")
                return "-1";
            if (_Total.Contains("%"))
                return "100%";
            return _Total.Trim();
        }
        #endregion

        #region 礼包剩余数
        private string getSurplus(HtmlDocument doc, PackageConfig config, string url)
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
                Helpers.WriteLog("抓游戏名出错" + ex.ToString() + url, "Log\\error.log");//
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
        public Package GetPack(string url, PackageConfig pcfig)
        {
            Package pck = new Package();
            HtmlDocument doc = null;
            if (!string.IsNullOrEmpty(url) || url != "")
            {
                doc = getDoc(url);
            }

            if (doc != null)
            {
                pck.ListconfigId = pcfig.ListConfigId;
                pck.GameName = getGameName(doc, pcfig, url);
                pck.PackageUrl = url;
                pck.Total = getTotal(doc, pcfig, url);
                pck.Surplus = getSurplus(doc, pcfig, url);
                pck.GrantCount = "";
                pck.StartTime = DateTime.Now;
                pck.CurrentTime = DateTime.Now;

                var p = Table.Object<Package>()
                             .Where(m => m.PackageUrl, url)
                             .SelectList().FirstOrDefault();
                if (p != null)
                {
                    if (p.Surplus.Contains("%") && pck.Surplus.Contains("%"))
                    {
                        string pS = p.Surplus.Replace("%", "");
                        string PcS = pck.Surplus.Replace("%", "");
                        pck.GrantCount = (int.Parse(pS) - int.Parse(PcS)).ToString() + "%";
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
            if (this!= null)
            {
                var o = Table.Object<Package>()
                              .Where(m => m.PackageUrl, this.PackageUrl)
                              .SelectList().FirstOrDefault();

                if (o != null)
                {
                    o.CurrentTime = DateTime.Now;
                    o.Surplus = this.Surplus;
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
