using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using PacksModels;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace PacksCrawl
{
    [Map("packs")]
    public class Packs : DataFieldBase
    {
        //ID
        [Map("packs_id")]
        public int Packs_ID { get; set; }

        //对应站点名
        [Map("web_name")]
        public string Web_Name { get; set; }

        //礼包名
        [Map("packs_name")]
        public string Packs_Name { get; set; }

        //礼包对应游戏名
        [Map("game_name")]
        public string Game_Name { get; set; }

        //礼包有效期
        [Map("packs_valid")]
        public string Packs_Valid { get; set; }

        //礼包开始时间
        [Map("start_time")]
        public DateTime Start_Time { get; set; }

        //礼包开始时间
        [Map("end_time")]
        public DateTime End_Time { get; set; }

        //礼包说明
        [Map("packs_exp")]
        public string Packs_Exp { get; set; }

        //礼包页面
        [Map("packs_url")]
        public string Packs_Url { get; set; }

        //礼包页面
        [Map("write_time")]
        public DateTime Write_Time { get; set; }


        //查询
        public static Packs SelectPack(string packs_name)
        {
            var o = Table.Object<Packs>()
             .Where(m => m.Packs_Name, packs_name)
             .SelectList().FirstOrDefault();
            return o;
        }

        //获得礼包数据
        public static Packs GetPacks(string url,PacksConfig config)
        {
            //url = "http://ka.mgamer.cn/id-260";
            //Helpers.WriteLog("进入获取礼包，下载资源：" + url, "Log\\error.log");
            string html = HtmlAgilityPackHelper.getHtml(url, "");
            //Helpers.WriteLog("资源下载完毕" + url, "Log\\error.log");
            HtmlDocument doc = new HtmlDocument();
            if (html != null)
            {
                doc.LoadHtml(html);
            }

            double count = PacksCount(doc, config);
            Packs packs = null;
            if (count > 0)
            {
                List<string> game_pack_name = PGame_PName(doc, config);
                string _Packs_Exp = PacksExp(doc, config);
                string _Packs_Valid = PacksTime.PacksValid(doc, config,url);
                List<DateTime> times = PacksTime._Final_Time(doc, config, url);

                packs = new Packs
                {
                    Web_Name = config.WebName,
                    Packs_Name = game_pack_name[0],
                    Game_Name = game_pack_name[1],
                    Packs_Valid = _Packs_Valid,
                    Start_Time = times[0],
                    End_Time = times[1],
                    Packs_Exp = _Packs_Exp,
                    Packs_Url = url,
                    Write_Time = DateTime.Now
                };
            }


            return packs;
        }


        //获得礼包对应游戏名称
        public static List<string> PGame_PName(HtmlDocument doc, PacksConfig config)
        {
            List<string> pack_n = new List<string>(2);
            string packs_name = "";
            string packs_game = "";
            string nodeText = "";

            if (config.PacksGameXpath != "value")
            {
                //礼包名
                packs_name = PacksName(doc, config);
                pack_n.Add(packs_name);

                HtmlNode node = null;
                //判断xpath是否带有参数,有就进行拆分
                string PacksGameXpath = "";
                int ChildCount = -1;
                if (config.PacksGameXpath.Contains("?"))
                {
                    string[] xps = config.PacksGameXpath.Split('?');
                    PacksGameXpath = xps[0];
                    ChildCount = int.Parse(xps[1]);
                }
                else
                {
                    PacksGameXpath = config.PacksGameXpath;
                }


                if (PacksGameXpath != null)
                {
                    node = HtmlAgilityPackHelper.GetNode(doc, PacksGameXpath);
                }

                nodeText = "";
                if (node != null)
                {
                    if (ChildCount != -1)
                    {
                        nodeText = node.ChildNodes[ChildCount].InnerText;
                    }
                    else
                    {
                        nodeText = node.ChildNodes[0].InnerText;
                    }

                    if (nodeText.Contains("游戏名："))
                    {
                        nodeText = nodeText.Replace("游戏名：", "").Trim();
                    }
                    if (nodeText.Contains("游戏名字："))
                    {
                        nodeText = nodeText.Replace("游戏名字：", "").Trim();
                    }

                    //nodeText = node.InnerHtml;
                    //string[] texts = nodeText.Split('<');
                    //nodeText = texts[0];
                }

                pack_n.Add(nodeText);
            }
            else
            {
                packs_name = PacksName(doc, config);
                pack_n.Add(packs_name);

                if (packs_name.Contains("《") && packs_name.Contains("》"))
                {
                    string[] parts1 = packs_name.Split('《');
                    string[] parts2 = parts1[1].Split('》');

                    packs_game = parts2[0];
                    pack_n.Add(packs_game);
                }
                else
                {
                    pack_n.Add(packs_name);
                }
            }
            return pack_n;
        }


        #region 获得礼包名称,并解析对应的游戏名称
        //获得礼包名称,并解析对应的游戏名称
        public static string PacksName(HtmlDocument doc, PacksConfig config)
        {
            HtmlNode node = null;
            if (config.PacksNameXpath != null)
            {
                node = HtmlAgilityPackHelper.GetNode(doc, config.PacksNameXpath);
            }
            string packs_name = "";

            if (node != null)
            {
                packs_name = node.InnerText;
            }
            return packs_name;
        } 
        #endregion

        #region 获得剩余数量
        //获得剩余数量
        public static double PacksCount(HtmlDocument doc, PacksConfig config)
        {
            double count = 0;

            try
            {
                if (config.PacksCountXpath != "value")
                {
                    HtmlNode node = HtmlAgilityPackHelper.GetNode(doc, config.PacksCountXpath);

                    if (node != null)
                    {

                        string ratio = node.InnerText;
                        //删除中文部分，留学数字部分进行分析

                        string result = System.Text.RegularExpressions.Regex.Replace(ratio, @"[\u4e00-\u9fa5]*", "").Replace("%", "").Replace("：", "");
                        string[] counts = result.Split('/');

                        if (counts[0] != null && !string.IsNullOrEmpty(counts[0]) && counts[0] != "")
                        {
                            count = System.Convert.ToDouble(counts[0]);
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                }
                else
                {
                    count = 1;
                }
            }
            catch (Exception ex)
            {
                Helpers.WriteLog("获取礼包数量时出错" + ex.ToString(), "Log\\error.log");//
            }

            return count;
        } 
        #endregion

        #region 获得礼包说明
        //获得礼包说明
        public static string PacksExp(HtmlDocument doc, PacksConfig config)
        {
            HtmlNode node = null;
            if (config.PacksExpXpath != null)
            {
                node = HtmlAgilityPackHelper.GetNode(doc, config.PacksExpXpath);
            }
            string nodeText = "";
            if (node != null)
            {
                nodeText = node.InnerText;
            }

            return nodeText;
        } 
        #endregion

        #region 插入数据
        //插入数据
        public static void Insert(Packs p)
        {
            var o = Table.Object<Packs>()
                         .Where(m => m.Packs_Url, p.Packs_Url)
                         .SelectList().FirstOrDefault();

            if (o == null)
            {
                DataAccess.Insert(p);
            }
        } 
        #endregion

        //#region 每秒钟去对比一下时间，看是否到点
        ////每秒钟去对比一下时间，看是否到点
        //public static void TimerMath()
        //{
        //    System.Timers.Timer aTimer = new System.Timers.Timer();
        //    aTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimeEvent);
        //    // 设置引发时间的时间间隔　此处设置为1秒（1000毫秒）
        //    aTimer.Interval = 1000;
        //    aTimer.Enabled = true;
        //} 
        //#endregion

        //#region 对比时间，如果到点，则执行方法
        ////对比时间，如果到点，则执行方法
        //private static void TimeEvent(object source, System.Timers.ElapsedEventArgs e)
        //{
        //    // 得到 hour minute second　如果等于某个值就开始执行某个程序。
        //    int intHour = e.SignalTime.Hour;
        //    int intMinute = e.SignalTime.Minute;
        //    int intSecond = e.SignalTime.Second;
        //    // 定制时间； 比如 在10：30 ：00 的时候执行某个函数
        //    int iHour = 10;
        //    int iMinute = 30;
        //    int iSecond = 00;

        //    //Console.WriteLine("开始时间" + DateTime.Now.ToString());

        //    // 设置　 每秒钟的开始执行一次
        //    if (intSecond == iSecond)
        //    {
        //        //Console.WriteLine("每秒钟的开始执行一次！");
        //    }
        //    // 设置　每个小时的３０分钟开始执行
        //    if (intMinute == iMinute && intSecond == iSecond)
        //    {
        //        //Console.WriteLine("每个小时的３０分钟开始执行一次！");
        //    }
        //    // 设置　每天的１０：３０：００开始执行程序
        //    if (intHour == iHour && intMinute == iMinute && intSecond == iSecond)
        //    {
        //        intoPacks();
        //        //Console.WriteLine("在每天１０点３０分开始执行！");
        //    }
        //} 
        //#endregion

        #region 更新礼包或插入新的礼包
        /// <summary>
        /// 更新礼包或插入新的礼包
        /// </summary>
        public static void intoPacks()
        {
            //获取所有配置项
            var pkcs = PacksConfig.allConfig();
            Console.WriteLine(">>>" + DateTime.Now.ToString() + "开始更新...");

            for (int i = 0; i < pkcs.Count; i++)
            {
                //礼包列表中的所有礼包地址
                List<string> ls = PacksList.allPackUrl(pkcs[i].ListUrl, pkcs[i]);
                //循环礼包地址，并验证数据库中是否存在，如果不存在测抓取数据，插入数据库
                for (int y = 0; y < ls.Count; y++)
                {
                    var o = Table.Object<Packs>()
                                 .Where(m => m.Packs_Url, ls[y])
                                 .SelectList().FirstOrDefault();
                    if (o == null)
                    {
                        //抓取礼包
                        Packs p = Packs.GetPacks(ls[y], pkcs[i]);
                        if (p != null)
                        {
                            DataAccess.Insert(p);
                        }
                    }
                    else
                    {
                        //抓取礼包
                        Packs p = Packs.GetPacks(ls[y], pkcs[i]);
                        if (p == null)
                        {
                            var po = Table.Object<Packs>()
                                          .Where(m => m.Packs_Url, ls[y])
                                          .SelectList().FirstOrDefault();

                            DataAccess.Delete(po);
                        }
                    }
                }
            }

            Console.WriteLine(">>>" + DateTime.Now.ToString() + "更新完毕");
        } 
        #endregion












        public static void intoPacks(object source, System.Timers.ElapsedEventArgs e)
        {
            //获取所有配置项
            var pkcs = PacksConfig.allConfig();
            Console.WriteLine(">>>" + DateTime.Now.ToString() + "开始更新...");

            for (int i = 0; i < pkcs.Count; i++)
            {
                //礼包列表中的所有礼包地址
                List<string> ls = PacksList.allPackUrl(pkcs[i].ListUrl, pkcs[i]);
                //循环礼包地址，并验证数据库中是否存在，如果不存在测抓取数据，插入数据库
                for (int y = 0; y < ls.Count; y++)
                {
                    var o = Table.Object<Packs>()
                                 .Where(m => m.Packs_Url, ls[y])
                                 .SelectList().FirstOrDefault();
                    if (o == null)
                    {
                        //抓取礼包
                        Packs p = Packs.GetPacks(ls[y], pkcs[i]);
                        if (p != null)
                        {
                            DataAccess.Insert(p);
                        }
                    }
                    else
                    {
                        //抓取礼包
                        Packs p = Packs.GetPacks(ls[y], pkcs[i]);
                        if (p == null)
                        {
                            var po = Table.Object<Packs>()
                                          .Where(m => m.Packs_Url, ls[y])
                                          .SelectList().FirstOrDefault();

                            DataAccess.Delete(po);
                        }
                    }
                }
            }

            Console.WriteLine(">>>" + DateTime.Now.ToString() + "更新完毕");
        }




    }
}
