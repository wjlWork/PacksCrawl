using Qmigh.Framework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacksShow.Models
{
    [Map("packslistconfig_statistical")]
    public class PacksListConfig_Statistical : TracableObject
    {
        #region 属性
        [Map("id")]
        public int Id { get; set; }
        [Map("site_name")]
        public string SiteName { get; set; }
        [Map("packslist_homeurl")]
        public string PacksListHomeUrl { get; set; }
        [Map("packslist_urlsxpath")]
        public string PacksListUrlsXpath { get; set; }
        [Map("nextpage_urlxpath")]
        public string NextPageUrlXpath { get; set; }
        #endregion

        public static PacksListConfig_Statistical Get(int id)
        {
            return DataAccess.Select<PacksListConfig_Statistical>(id);
        }
    }

}