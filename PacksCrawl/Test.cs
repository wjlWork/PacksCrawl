using PacksCrawl.Common;
using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacksCrawl
{
    public class Test
    {
        public static void getPacks()
        {
            //礼包
            PacksConfig pkc = PacksConfig.SelectConfig("http://fahao.mofang.com");
            Packs p = Packs.GetPacks("http://fahao.mofang.com/libao/344.html", pkc);

            //Packs.intoPacks1();
            //礼包列表
            //List<string> ls = PacksList.allPackUrl("http://ka.gamedog.cn/list/xinshouka/", pkc);


            //Packs.selectPacks("http://www.youba.com/ka/1403/360.htm");
           

            Console.WriteLine(">>>测试");
        }


        public static void TimerMath()
        {
            int h = DateTime.Now.Hour;
            int m = DateTime.Now.Minute;
            int s = DateTime.Now.Second;

            //long time = (((h < 9 ? 9 : 33) - h) * 60 * 60 - m * 60 - s) * 1000;
            long time = 10 * 1000;

            //实例化Timer类，设置间隔时间为10000毫秒；  
            System.Timers.Timer t = new System.Timers.Timer(time);
            //到达时间的时候执行事件；
            t.Elapsed += new System.Timers.ElapsedEventHandler(intoPacks2);
            //设置是执行一次（false）还是一直执行(true)；  
            t.AutoReset = true;
            //是否执行System.Timers.Timer.Elapsed事件；
            t.Enabled = true;
        }


    



        public static void intoPacks2(object source, System.Timers.ElapsedEventArgs e)
        {
            //获取所有配置项
            var pkcs = PacksConfig.allConfig();
            Console.WriteLine(">>>" + DateTime.Now.ToString() + "开始更新...");

            System.Threading.Thread.Sleep(2000);

            Console.WriteLine(">>>" + DateTime.Now.ToString() + "更新完毕");

        }


        public static void TimerMath1()
        {
            int h = DateTime.Now.Hour;
            int m = DateTime.Now.Minute;
            int s = DateTime.Now.Second;

            long time = (((h < 6 ? 6 : 30) - h) * 60 * 60 - m * 60 - s) * 1000;

            //实例化Timer类，设置间隔时间为10000毫秒；  
            System.Timers.Timer t = new System.Timers.Timer(time);
            //到达时间的时候执行事件；
            t.Elapsed += new System.Timers.ElapsedEventHandler(Packs.intoPacks);
            //设置是执行一次（false）还是一直执行(true)；  
            t.AutoReset = true;
            //是否执行System.Timers.Timer.Elapsed事件；
            t.Enabled = true;
        }



        public static void intoPacks1()
        {
            //获取所有配置项
            var pkcs = PacksConfig.allConfig();
            Console.WriteLine(">>>正在抓取...");

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

            Console.WriteLine(">>>抓取完毕");
        }


        public static Packs selectPacks(string url)
        {
            var o = Table.Object<Packs>()
                         .Where(m => m.Packs_Url, url)
                         .SelectList().FirstOrDefault();

            return o;
        }



        public static void intoPacks1(string url)
        {
            //获取所有配置项

            PacksConfig pkc = PacksConfig.SelectConfig("http://kf.91.com");
            //Packs p1 = Packs.GetPacks(url, pkc);


            var o = Table.Object<Packs>()
                         .Where(m => m.Packs_Url, url)
                         .SelectList().FirstOrDefault();

            if (o == null)
            {
                Packs p = Packs.GetPacks(url, pkc);
                if (p != null)
                {
                    DataAccess.Insert(p);
                }
                else
                {
                    var po = Table.Object<Packs>()
                                 .Where(m => m.Packs_Url, url)
                                 .SelectList().FirstOrDefault();

                    DataAccess.Delete(po);
                }
            }

        }





        public static void Test3()
        {
            RunningCrawl.Running();
        }







    }
}
