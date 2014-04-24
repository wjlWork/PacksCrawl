using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacksShow.Models
{
    [Map("packs_statistical")]
    public class Packs_Statistical : TracableObject
    {
        #region 属性
        [Map("id")]
        public int Id { get; set; }

        [Map("packslistconfig_id")]
        public int ListConfigId { get; set; }

        [Map("game_name")]
        public string GameName { get; set; }

        [Map("packs_url")]
        public string PackageUrl { get; set; }

        [Map("total")]
        public string Total { get; set; }

        [Map("surplus")]
        public string Surplus { get; set; }

        [Map("send_count")]
        public string GrantCount { get; set; }

        [Map("star_time")]
        public DateTime StartTime { get; set; }

        [Map("current_time")]
        public DateTime CurrentTime { get; set; }
        #endregion

        #region 返回所有的礼包数据
        /// <summary>
        /// 返回所有的礼包数据
        /// </summary>
        /// <returns>礼包列表</returns>
        public List<Packs_Statistical> GetPcks()
        {
            return Table.Object<Packs_Statistical>()
                        .SelectList();
        } 
        #endregion

        public static List<Packs_Statistical> GetPcks(int listConfigId)
        {
            return Table.Object<Packs_Statistical>()
                        .Where(m => m.ListConfigId, listConfigId)
                        .SelectList();
        } 

    }
}