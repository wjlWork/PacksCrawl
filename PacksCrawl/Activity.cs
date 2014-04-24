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
    [Map("activity_1")]
    public class Activity : DataFieldBase
    {
        [Map("id")]
        public int ID { get; set; }

        [Map("game_id")]
        public int Game_Id { get; set; }

        [Map("status")]
        public int Status { get; set; }

        [Map("title")]
        public string Title { get; set; }

        [Map("channel")]
        public string Channel { get; set; }

        [Map("details")]
        public int Details { get; set; }

        [Map("min_level")]
        public int Min_Level { get; set; }

        [Map("max_level")]
        public int Max_Level { get; set; }

        [Map("show_time")]
        public int Show_Time { get; set; }

        [Map("start_time")]
        public int Start_Time { get; set; }

        [Map("end_time")]
        public int End_Time { get; set; }

        [Map("type")]
        public int Type { get; set; }

        [Map("quota")]
        public int Quota { get; set; }

        [Map("joined_count")]
        public int Joined_Count { get; set; }

        [Map("platform")]
        public int Platform { get; set; }

        [Map("form_fields")]
        public int Form_Fields { get; set; }

        [Map("point_needed")]
        public int Point_Needed { get; set; }

        [Map("share_text")]
        public int Share_Text { get; set; }

        [Map("share_image")]
        public int Share_Image { get; set; }

        [Map("joined_text")]
        public int Joined_Text { get; set; }

        [Map("waiting_text")]
        public int Waiting_Text { get; set; }

        [Map("hit_text")]
        public int Hit_Text { get; set; }

        [Map("unhit_text")]
        public int Unhit_Text { get; set; }

        [Map("tofill_text")]
        public int Tofill_Text { get; set; }

        [Map("hide_time")]
        public int Hide_Time { get; set; }

        [Map("need_bind_phone")]
        public int Need_Bind_Phone { get; set; }

        [Map("data")]
        public int _Data { get; set; }



        //查询
        public static Activity SelectPack(string title)
        {
            var o = Table.Object<Activity>()
             .Where(m => m.Title, title)
             .SelectList().FirstOrDefault();
            return o;
        }

        //获得礼包数据
        public static Activity GetPacks(string url, PacksConfig config)
        {
            string html = HtmlAgilityPackHelper.getHtml(url, "");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            double count = PacksCount(doc, config);
            Activity activity = null;
            if (count > 0)
            {
                List<string> game_pack_name = PGame_PName(doc, config);
                string _Packs_Exp = PacksExp(doc, config);
                string _Packs_Valid = PacksValid(doc, config);

                activity = new Activity
                {
                    Channel = config.WebName,
                    Title = game_pack_name[1]
                    //Game_Name = game_pack_name[0],
                    //Packs_Valid = _Packs_Valid,
                    //Packs_Exp = _Packs_Exp,
                    //Packs_Url = url
                };
            }


            return activity;
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
                packs_name = PacksName(doc, config);
                pack_n.Add(packs_name);
                HtmlNode node = HtmlAgilityPackHelper.GetNode(doc, config.PacksGameXpath);

                nodeText = "";
                if (node != null)
                {
                    nodeText = node.InnerHtml;
                    string[] texts = nodeText.Split('<');

                    nodeText = texts[0];
                }

                pack_n.Add(nodeText);
            }
            else
            {
                packs_name = PacksName(doc, config);
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
                pack_n.Add(packs_name);
            }
            return pack_n;
        }


        //获得礼包名称,并解析对应的游戏名称
        public static string PacksName(HtmlDocument doc, PacksConfig config)
        {
            HtmlNode node = HtmlAgilityPackHelper.GetNode(doc, config.PacksNameXpath);
            string packs_name = "";

            if (node != null)
            {
                packs_name = node.InnerText;
            }
            return packs_name;
        }



        //获得剩余数量
        public static double PacksCount(HtmlDocument doc, PacksConfig config)
        {
            double count = 0;

            if (config.PacksCountXpath != "value")
            {
                HtmlNode node = HtmlAgilityPackHelper.GetNode(doc, config.PacksCountXpath);

                if (node != null)
                {

                    string ratio = node.InnerText;
                    //删除中文部分，留学数字部分进行分析
                    
                    string result = System.Text.RegularExpressions.Regex.Replace(ratio, @"[\u4e00-\u9fa5]*", "").Replace("%", "").Replace("：", "");
                    string[] counts = result.Split('/'); 
                    count = System.Convert.ToDouble(counts[0]);

                }
            }
            else
            {
                count = 1;
            }

            return count;
        }

        //获得礼包说明
        public static string PacksExp(HtmlDocument doc, PacksConfig config)
        {
            HtmlNode node = HtmlAgilityPackHelper.GetNode(doc, config.PacksExpXpath);
            string nodeText = "";
            if (node != null)
            {
                nodeText = node.InnerText;
            }

            return nodeText;
        }

        //获得礼包有效期
        public static string PacksValid(HtmlDocument doc, PacksConfig config)
        {
            HtmlNode node = HtmlAgilityPackHelper.GetNode(doc, config.PacksValidXpath);
            string nodeText = "";
            if (node != null)
            {
                nodeText = node.InnerText;
            }

            return nodeText;
        }






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


        public static void TimerMath()
        {
            int h = DateTime.Now.Hour;
            int m = DateTime.Now.Minute;
            int s = DateTime.Now.Second;

            long time = (((h < 9 ? 9 : 33) - h) * 60 * 60 - m * 60 - s) * 1000;

            //实例化Timer类，设置间隔时间为10000毫秒；  
            System.Timers.Timer t = new System.Timers.Timer(time);
            //到达时间的时候执行事件；
            t.Elapsed += new System.Timers.ElapsedEventHandler(GetPacks);
            //设置是执行一次（false）还是一直执行(true)；  
            t.AutoReset = true;
            //是否执行System.Timers.Timer.Elapsed事件；
            t.Enabled = true;
        }


        public static void GetPacks(object source,System.Timers.ElapsedEventArgs e)
        {
            //获取所有配置项
            var pkcs = PacksConfig.allConfig();

            for (int i = 0; i < pkcs.Count; i++)
            {
                //礼包列表中的所有礼包地址
                List<string> ls = PacksList.allPackUrl(pkcs[i].ListUrl, pkcs[i]);
                //循环礼包地址，并验证数据库中是否存在，如果不存在测抓取数据，插入数据库
                for (int y = 0; y < 3; y++)
                {
                    var o = Table.Object<Packs>()
                                 .Where(m => m.Packs_Url,ls[y])
                                 .SelectList().FirstOrDefault();
                    if (o == null)
                    {
                        Packs p = Packs.GetPacks(ls[y], pkcs[i]);
                        DataAccess.Insert(p);
                    }
                }

            }
        }



        public static void GetPacks1()
        {
            //获取所有配置项
            var pkcs = PacksConfig.allConfig();

            for (int i = 0; i < pkcs.Count; i++)
            {
                //礼包列表中的所有礼包地址
                List<string> ls = PacksList.allPackUrl(pkcs[i].ListUrl, pkcs[i]);
                //循环礼包地址，并验证数据库中是否存在，如果不存在测抓取数据，插入数据库
                for (int y = 0; y < 3; y++)
                {
                    var o = Table.Object<Packs>()
                                 .Where(m => m.Packs_Url, ls[y])
                                 .SelectList().FirstOrDefault();
                    if (o == null)
                    {
                        Packs p = Packs.GetPacks(ls[y], pkcs[i]);
                        DataAccess.Insert(p);
                    }
                }

            }
        }








    }
}
