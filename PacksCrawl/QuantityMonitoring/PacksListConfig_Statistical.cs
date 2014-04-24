using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;

namespace PacksCrawl.QuantityMonitoring
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

        #region 返回所有 获取 礼包列表 的配置数据
        /// <summary>
        /// 返回所有 获取 礼包列表 的配置数据
        /// </summary>
        /// <returns>配置数据列表</returns>
        public static List<PacksListConfig_Statistical> GetAllConfig()
        {
            return Table.Object<PacksListConfig_Statistical>()
                        .SelectList();
        } 
        #endregion

        #region 根据礼包列表首页地址 获得一条配置数据
        /// <summary>
        /// 根据礼包列表首页地址 获得一条配置数据
        /// </summary>
        /// <param name="url">礼包列表第一页地址</param>
        /// <returns>配置数据</returns>
        public static PacksListConfig_Statistical GetConfig(string url)
        {
            return Table.Object<PacksListConfig_Statistical>()
                        .Where(m => m.PacksListHomeUrl, url)
                        .SelectList().FirstOrDefault();
        } 
        #endregion

    }
}
