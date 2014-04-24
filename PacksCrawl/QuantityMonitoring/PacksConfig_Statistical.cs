using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacksCrawl.QuantityMonitoring
{

    [Map("packsconfig_statistical")]
    public class PacksConfig_Statistical : TracableObject
    {
        [Map("id")]
        public int Id { get; set; }

        [Map("packslistconfig_id")]
        public int ListConfigId { get; set; }

        [Map("game_name_xpath")]
        public string GameNameXpath { get; set; }

        [Map("total_xpath")]
        public string TotalXpath { get; set; }

        [Map("surplus_xpath")]
        public string SurplusXpath { get; set; }


        public List<PacksConfig_Statistical> GetConfigs()
        {
            return Table.Object<PacksConfig_Statistical>()
                        .SelectList();
        }

        public static PacksConfig_Statistical GetConfig(int listconfigid)
        {
            return Table.Object<PacksConfig_Statistical>()
                        .Where(m => m.ListConfigId, listconfigid)
                        .SelectList().FirstOrDefault();
        }

    }

}
