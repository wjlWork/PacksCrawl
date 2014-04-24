using PacksCrawl.QuantityMonitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacksCrawl.Common
{
    public class RunningCrawl
    {
        public static void Running()
        {
            List<PacksListConfig_Statistical> lcs = PacksListConfig_Statistical.GetAllConfig();
            foreach (var l in lcs)
            {
                PacksConfig_Statistical pckC = PacksConfig_Statistical.GetConfig(l.Id);
                PacksContainer_Statistical pl = new PacksContainer_Statistical(l.PacksListHomeUrl, l);
                foreach (var ul in pl.PackUrlList)
                {
                    Packs_Statistical pck = new Packs_Statistical();
                    pck = pck.GetPack(ul, pckC);
                    pck.InsertData();
                }
                Statis sta = new Statis();
                sta = sta.CreateStatis(l.Id);
            }





            //List<PacksStaListConfig> lcs = PacksStaListConfig.GetAllConfig();

            //PacksStaConfig pckC = PacksStaConfig.GetConfig(lcs[0].Id);
            //PacksStaContainer pl = new PacksStaContainer(lcs[0].PacksListHomeUrl, lcs[0]);
            //foreach (var ul in pl.PackUrlList)
            //{
            //    PacksSta pck = new PacksSta();
            //    pck = pck.GetPack("http://bbs.appgame.com/plugin.php?id=moeac_grantcard&ac=card&cid=44", pckC);
            //    pck.InsertData();
            //}


        }


    }
}
