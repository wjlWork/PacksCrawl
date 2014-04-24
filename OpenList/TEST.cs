using HtmlAgilityPack;
using OpenList.QuantityMonitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenList
{
    public class TEST
    {
        public void Test1()
        {
            GameServer os = new GameServer();
            os = os.Test("http://www.91weile.com/kfb/indexrmtj.shtml", "微乐网");

        }


        public void Test2()
        {
            GameServer os = new GameServer();
            os.InsertOS();
        }

        public void Test3()
        {
            RunningCrawl.Running();
        }
    }
}
