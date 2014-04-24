using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenList.QuantityMonitoring
{
    [Map("listconfig")]
    public class ListConfig : TracableObject
    {
        [Map("id")]
        public int Id { get; set; }

        [Map("web_name")]
        public string WebName { get; set; }

        [Map("list_url")]
        public string ListUrl { get; set; }

        [Map("list_pack_urlxpath")]
        public string ListPackUrlXpath { get; set; }

        [Map("next_page_xpath")]
        public string NextPageXpath { get; set; }


        public static List<ListConfig> GetConfigs()
        {
            return Table.Object<ListConfig>()
                        .SelectList();
        }

        public static ListConfig GetConfig(string url)
        {
            return Table.Object<ListConfig>()
                        .Where(m => m.ListUrl, url)
                        .SelectList().FirstOrDefault();
        }

    }
}
