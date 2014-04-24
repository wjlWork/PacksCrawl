﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacksModels;
using PacksCrawl.Common;

namespace PacksCrawl
{
    class Program
    {
        static void Main(string[] args)
        {
            PacksCrawl.Common.Ex.Init();
            Console.WriteLine("输入“AC”运行定时抓取礼包数据，输入“exit”退出程序");
            Console.Write(">>>");
            string et = Console.ReadLine();
            while (et != "exit")
            {
                if (et == "Insert")
                {
                    string InsertData = Console.ReadLine();
                    PacksConfig.Insert(InsertData);
                    et = "";

                }
                else if (et == "AC" || et == "ac")
                {
                    //定时执行抓取任务
                    TimerRun.TimerMath();
                    Console.WriteLine("抓取礼包，运行定时...");
                    //Test.Test3();
                    et = "";
                }

                else
                {
                    //Console.Write(">>>");
                    et = Console.ReadLine();
                }

            }



        }
    }
}
