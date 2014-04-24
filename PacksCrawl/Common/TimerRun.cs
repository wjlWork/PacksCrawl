using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacksCrawl.Common
{
    public class TimerRun
    {
        #region 每秒钟去对比一下时间，看是否到点
        //每秒钟去对比一下时间，看是否到点
        public static void TimerMath()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimeEvent);
            // 设置引发时间的时间间隔　此处设置为1秒（1000毫秒）
            aTimer.Interval = 1000;
            aTimer.Enabled = true;
        }
        #endregion

        #region 对比时间，如果到点，则执行方法
        //对比时间，如果到点，则执行方法
        private static void TimeEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            // 得到 hour minute second　如果等于某个值就开始执行某个程序。
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;
            // 定制时间； 比如 在10：30 ：00 的时候执行某个函数
            int iHour = 10;
            int iMinute = 30;
            int iSecond = 00;

            //Console.WriteLine("开始时间" + DateTime.Now.ToString());

            // 设置　 每秒钟的开始执行一次
            if (intSecond == iSecond)
            {
                //Console.WriteLine("每秒钟的开始执行一次！");
            }
            // 设置　每个小时的３０分钟开始执行
            if (intMinute == iMinute && intSecond == iSecond)
            {
                //Console.WriteLine("每个小时的３０分钟开始执行一次！");
            }
            // 设置　每天的１０：３０：００开始执行程序
            if (intHour == iHour && intMinute == iMinute && intSecond == iSecond)
            {
                Packs.intoPacks();
                RunningCrawl.Running();
                //Console.WriteLine("在每天１０点３０分开始执行！");
            }
        }
        #endregion
    }
}
