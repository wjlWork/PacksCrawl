using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenList.QuantityMonitoring
{
    public class RunningCrawl
    {
        public static void Running()
        {
            List<ListConfig> lcs = ListConfig.GetConfigs();
            foreach (var l in lcs)
            {
                PackageConfig pckC = PackageConfig.GetConfig(l.Id);
                PackageList pl = new PackageList(l.ListUrl, l);
                foreach (var ul in pl.PackUrlList)
                {
                    Package pck = new Package();
                    pck = pck.GetPack(ul, pckC);
                    pck.InsertData();
                }
            }


            //List<ListConfig> lcs = ListConfig.GetConfigs();

            //PackageConfig pckC = PackageConfig.GetConfig(lcs[2].Id);
            //PackageList pl = new PackageList(lcs[2].ListUrl, lcs[2]);
            //foreach (var ul in pl.PackUrlList)
            //{
            //    Package pck = new Package();
            //    pck = pck.GetPack(ul, pckC);
            //    pck.InsertData();
            //}


        }

    }
}
