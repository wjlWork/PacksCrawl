using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PacksShow.Models
{
    [Map("statis")]
    public class Statis : TracableObject
    {
        [Map("id")]
        public int Id { get; set; }
        [Map("packslistconfig_id")]
        public int ListConfigId { get; set; }
        [Map("send_count")]
        public string SendCount { get; set; }
        [Map("star_time")]
        public DateTime StarTime { get; set; }
        [Map("current_time")]
        public DateTime CurrentTime { get; set; }

        public string SiteName { get; set; }


        public static List<Statis> GetStatis()
        {
            var oList = Table.Object<Statis>()
                        .SelectList();
            if (oList.Count != 0)
            {
                foreach (var o in oList)
                {
                    o.SiteName = PacksListConfig_Statistical.Get(o.ListConfigId).SiteName;
                }
            }

            return oList;
        }

    }

}