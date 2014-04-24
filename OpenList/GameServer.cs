using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qmigh.Framework.DataAccess;
using HtmlAgilityPack;
using PacksModels;
using Qmigh.Framework.DataAccess.TableDefine;

namespace OpenList
{
    [Map("game_server")]
    public class GameServer:DataFieldBase
    {
        [Map("id")]
        public int Id { get; set; }

        [Map("WebName")]
        public string WebName { get; set; }

        [Map("GameName")]
        public string GameName { get; set; }

        [Map("Platform")]
        public int Platform { get; set; }

        [Map("ServerName")]
        public string ServerName { get; set; }

        [Map("TestStatus")]
        public string TestStatus { get; set; }

        [Map("OpenDate")]
        public DateTime? OpenDate { get; set; }

        [Map("OpenTime")]
        public DateTime? OpenTime { get; set; }

        [Map("Operator")]
        public string Operators { get; set; }

        [Map("Contact")]
        public string Contact { get; set; }

        [Map("DownloadUrl")]
        public string DownloadUrl { get; set; }


        #region 获取开服列表到数据库中的方法
        public void InsertOS()
        {
            Console.WriteLine(">>>" + DateTime.Now.ToString() + "开始更新...");
            //所以配置项
            List<GameServerConfig> oscf = GameServerConfig.allConfig();
            //遍历配置项，拿出每个配置项取抓取列表
            foreach (var o in oscf)
            {
                //根据配置项，抓取列表
                List<GameServer> os = getOpenGameService(o);
                //遍历列表，拿出每一行
                foreach (var i in os)
                {
                    //判断抓取的列表行数据是否有效
                    if (i.GameName != null && i.DownloadUrl != null)
                    {
                        //判断数据库中是否已经存在这条数据
                        bool ck = IsNotNull(i.GameName,i.DownloadUrl,i.OpenDate,i.Platform);
                        if (ck != true)
                        {
                            //插入这条数据
                            DataAccess.Insert(i);
                        }
                    }
                }
                //删除数据库中存在，而网页中不存在的数据，即更新数据
                List<GameServer> dels = UpdateForData(o, os);
                if (dels.Count > 0)
                {
                    foreach (var d in dels)
                    {
                        DataAccess.Delete(d);
                    }
                }
            }
            Console.WriteLine(">>>" + DateTime.Now.ToString() + "更新完毕");
        }
        #endregion

        #region 验证数据表中是否有重复数据
        public bool IsNotNull(string gameName, string url, DateTime? openDate, int Platform)
        {
            bool ck = false;
            var o = Table.Object<GameServer>()
                         .Where(m => m.OpenDate, openDate)
                         .Where(m => m.GameName, gameName)
                         .Where(m => m.DownloadUrl, url)
                         .Where(m => m.Platform, Platform)
                         .SelectList().FirstOrDefault();
            if (o != null)
            {
                ck = true;
            }
            return ck;
        }
        #endregion

        #region 验证数据表中是否有更新
        public List<GameServer> UpdateForData(GameServerConfig config, List<GameServer> Gs)
        {
            //声明一个GameServer集合
            List<GameServer> OpsList = new List<GameServer>();
            //站点(模块)名，查询数据库中存在的数据
            var OpS = Table.Object<GameServer>()
                           .Where(m=>m.WebName,config.WebName)
                           .SelectList();
            //遍历查询出的数据
            foreach (var o1 in OpS)
            {
                bool f = false;
                //遍历抓取出来的数据
                foreach (var gs in Gs)
                {          
                    //比对数据库中的数据，是否存在于抓取的数据中
                    if (o1.GameName == gs.GameName && o1.OpenDate == gs.OpenDate && o1.DownloadUrl == gs.DownloadUrl&&o1.Platform==gs.Platform)
                    {
                        f = true;
                        break;
                    }
                }
                //如不存在，则放进列表中
                if (!f)
                {
                    OpsList.Add(o1);
                }
            }
            return OpsList;
        }
        #endregion

        #region 开服列表数据集
        public List<GameServer> getOpenGameService(GameServerConfig config)
        {
            HtmlDocument doc = getDoc(config.ListUrl);
            List<GameServer> GsList = new List<GameServer>();
            GameServer Oss = new GameServer();
            //列表中每一行的html代码块集合
            List<string> Trls = TrsData(doc, config, config.ListUrl);
            foreach(var ts in Trls)
            {
                if (ts != null && !string.IsNullOrEmpty(ts))
                {
                    Oss = getRow(config, ts);
                    GsList.Add(Oss);
                }
            }
            return GsList;
        }
        #endregion

        #region 开服列表一行数据
        public GameServer getRow(GameServerConfig config, string rowHtml)
        {
            HtmlDocument doc = new HtmlDocument();
            GameServer GS = new GameServer();
            try
            {
                doc.LoadHtml(rowHtml);
                //网站名
                GS.WebName = config.WebName;
                //游戏名
                GS.GameName = getGameName(doc, config, config.ListUrl);
                //使用平台
                GS.Platform = getPlatform(doc, config, config.ListUrl);
                //服务器名
                GS.ServerName = getServerName(doc, config, config.ListUrl);
                //测试状态
                GS.TestStatus = getTestStatus(doc, config, config.ListUrl);
                //开服日期
                GS.OpenDate = getOpenDate(doc, config, config.ListUrl);
                //开服时间
                GS.OpenTime = getOpenTime(doc, config, config.ListUrl);
                //运营商
                GS.Operators = getOperators(doc, config, config.ListUrl);
                //客服
                GS.Contact = getContact(doc, config, config.ListUrl);
                //下载地址
                GS.DownloadUrl = getDownloadUrl(doc, config, config.ListUrl);
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓一行数据出错" + ex.ToString() + config.ListUrl, "Log\\error.log");//
            }
            return GS;
        }
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

        #region 列表中每一行数据的html代码块集合
        public List<string> TrsData(HtmlDocument doc, GameServerConfig config, string url)
        {
            HtmlNodeCollection navNodes = null;
            string trHtml = "";
            List<string> Trs = new List<string>();
            try
            {
                if (config != null)
                {
                    navNodes = doc.DocumentNode.SelectNodes(config.TrsXpath);
                    if (navNodes != null)
                    {
                        foreach (var n in navNodes)
                        {
                            trHtml = n.InnerHtml;
                            Trs.Add(trHtml);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓行列表出错" + ex.ToString() + url, "Log\\error.log");//
            }

            return Trs;
        }
        #endregion

        #region 游戏名
        private string getGameName(HtmlDocument doc, GameServerConfig config, string url)
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
            catch(Exception ex)
            {
                Helpers.WriteLog("抓游戏名出错" + ex.ToString() + url, "Log\\error.log");//
            }
            return _Game.Trim();
        }
        #endregion

        #region 使用平台
        private int getPlatform(HtmlDocument doc, GameServerConfig config, string url)
        {
            HtmlNode node = null;
            string platformText = "";
            int platform = 0;
            try
            {
                if (doc != null && config.PlatformXpath != null)
                {
                    node = doc.DocumentNode.SelectSingleNode(config.PlatformXpath);
                    if (node != null)
                    {
                        platformText = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓游平台出错" + ex.ToString() + url, "Log\\error.log");//
            }

            if (platformText.Trim() == "iOS" || platformText.Contains("iphone"))
            {
                platform = 1;
            }
            else if (platformText.Trim() == "Android")
            {
                platform = 2;
            }
            else if (platformText.Contains("Android") && (platformText.Contains("iOS") || platformText.Contains("iphone")))
            {
                platform = 0;
            }
            return platform;
        }
        #endregion

        #region 服务器名
        private string getServerName(HtmlDocument doc, GameServerConfig config, string url)
        {
            HtmlNode node = null;
            string serverName = "";
            try
            {
                if (doc != null && config.ServerNameXpath != null)
                {
                    node = doc.DocumentNode.SelectSingleNode(config.ServerNameXpath);
                    if (node != null)
                    {
                        serverName = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓服务器名出错" + ex.ToString() + url, "Log\\error.log");//
            }

            return serverName;
        }
        #endregion

        #region 测试状态
        private string getTestStatus(HtmlDocument doc, GameServerConfig config, string url)
        {
            HtmlNode node = null;
            string _Status = null;
            try
            {
                if (doc != null && config.TestStatusXpath != null)
                {
                    node = doc.DocumentNode.SelectSingleNode(config.TestStatusXpath);
                    if (node != null)
                    {
                        _Status = node.InnerText;//
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓测试状态出错" + ex.ToString() + url, "Log\\error.log");
            }
            return _Status;
        }
        #endregion

        #region 开服日期
        private DateTime? getOpenDate(HtmlDocument doc, GameServerConfig config, string url)
        {
            HtmlNode node = null;
            DateTime? _Time = null;
            try
            {
                if (doc != null && config.OpenDateXpath != null)
                {
                    node = doc.DocumentNode.SelectSingleNode(config.OpenDateXpath);
                    if (node != null)
                    {
                        string timeText = OPHelper.getTimeText(node.InnerText);
                        if (timeText.Contains(":"))
                        {
                            string[] ts = timeText.Split(' ');
                            timeText = ts[0];
                        }
                        if (timeText != null && !string.IsNullOrEmpty(timeText))
                        {
                            _Time = DateTime.Parse(timeText);//
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓开服日期出错" + ex.ToString() + url, "Log\\error.log");
            }
            return _Time;
        }
        #endregion

        #region 开服时间
        private DateTime? getOpenTime(HtmlDocument doc, GameServerConfig config, string url)
        {
            HtmlNode node = null;
            DateTime? _Time = null;
            try
            {
                if (doc != null && config.OpenTimeXpath != null)
                {
                    node = doc.DocumentNode.SelectSingleNode(config.OpenTimeXpath);
                    if (node != null)
                    {
                        string timeText = OPHelper.getTimeText(node.InnerText);
                        if (timeText != null && !string.IsNullOrEmpty(timeText))
                        {
                            _Time = DateTime.Parse(timeText);//
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓开服时间出错" + ex.ToString() + url, "Log\\error.log");
            }
            return _Time;
        }
        #endregion

        #region 运营商
        private string getOperators(HtmlDocument doc, GameServerConfig config, string url)
        {
            HtmlNode node = null;
            string _Operators = null;
            try
            {
                if (doc != null && config.OperatorXpath != null)
                {
                    node = doc.DocumentNode.SelectSingleNode(config.OperatorXpath);
                    if (node != null)
                    {
                        _Operators = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓运营商出错" + ex.ToString() + url, "Log\\error.log");
            }
            return _Operators;
        }
        #endregion

        #region 客服方式
        private string getContact(HtmlDocument doc, GameServerConfig config, string url)
        {
            HtmlNode node = null;
            string _Customer = null;
            try
            {
                if (doc != null && config.ContactXpath != null)
                {
                    node = doc.DocumentNode.SelectSingleNode(config.ContactXpath);
                    if (node != null)
                    {
                        _Customer = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓客服方式出错" + ex.ToString() + url, "Log\\error.log");
            }
            return _Customer;
        }
        #endregion

        #region 下载地址
        private string getDownloadUrl(HtmlDocument doc, GameServerConfig config, string url)
        {
            HtmlNode node = null;
            string _DownloadUrl = null;
            try
            {
                if (doc != null && config.DownloadUrlXpath != null)
                {
                    node = doc.DocumentNode.SelectSingleNode(config.DownloadUrlXpath);
                    if (node != null)
                    {
                        _DownloadUrl = OPHelper.UpdateUrl(url,node.Attributes["href"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓下载地址出错" + ex.ToString() + url, "Log\\error.log");
            }
            return _DownloadUrl;
        }
        #endregion

        public GameServer Test(string url,string webname)
        {
            GameServer GS = new GameServer();
            HtmlDocument doc = GameServer.getDoc(url);
            GameServerConfig config = GameServerConfig.selectConfig1(webname);
            List<string> ls = TrsData(doc, config, url);
            try
            {
                foreach (var t in ls)
                {
                    doc.LoadHtml(t);
                    //网站名
                    GS.WebName = config.WebName;
                    //游戏名
                    GS.GameName = getGameName(doc, config, config.ListUrl);
                    //使用平台
                    GS.Platform = getPlatform(doc, config, config.ListUrl);
                    //服务器名
                    GS.ServerName = getServerName(doc, config, config.ListUrl);
                    //测试状态
                    GS.TestStatus = getTestStatus(doc, config, config.ListUrl);
                    //开服日期
                    GS.OpenDate = getOpenDate(doc, config, config.ListUrl);
                    //开服时间
                    GS.OpenTime = getOpenTime(doc, config, config.ListUrl);
                    //运营商
                    GS.Operators = getOperators(doc, config, config.ListUrl);
                    //客服
                    GS.Contact = getContact(doc, config, config.ListUrl);
                    //下载地址
                    GS.DownloadUrl = getDownloadUrl(doc, config, config.ListUrl);
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("抓一行数据出错" + ex.ToString() + config.ListUrl, "Log\\error.log");//
            }
            return GS;

        }

    }
}
