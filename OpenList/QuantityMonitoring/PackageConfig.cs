using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenList.QuantityMonitoring
{
    [Map("packageconfig")]
    public class PackageConfig:TracableObject
    {
        [Map("id")]
        public int Id { get; set; }

        [Map("listconfig_id")]
        public int ListConfigId { get; set; }

        [Map("game_name_xpath")]
        public string GameNameXpath { get; set; }

        [Map("total_xpath")]
        public string TotalXpath { get; set; }

        [Map("surplus_xpath")]
        public string SurplusXpath { get; set; }

        public List<PackageConfig> GetConfigs()
        {
            return Table.Object<PackageConfig>()
                        .SelectList();
        }

        public static PackageConfig GetConfig(int listconfigid)
        {
            return Table.Object<PackageConfig>()
                        .Where(m => m.ListConfigId, listconfigid)
                        .SelectList().FirstOrDefault();
        }


    }
}
