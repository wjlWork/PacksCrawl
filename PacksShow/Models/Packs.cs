using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;


namespace PacksShow.Models
{
    [Map("packs")]
    public class Packs : DataFieldBase
    {
        //ID
        [Map("packs_id")]
        public int Packs_ID { get; set; }

        //对应站点名
        [Map("web_name")]
        public string Web_Name { get; set; }

        //礼包名
        [Map("packs_name")]
        public string Packs_Name { get; set; }

        //礼包对应游戏名
        [Map("game_name")]
        public string Game_Name { get; set; }

        //礼包有效期
        [Map("packs_valid")]
        public string Packs_Valid { get; set; }

        //礼包开始时间
        [Map("start_time")]
        public string Start_Time { get; set; }


        //礼包结束时间
        [Map("end_time")]
        public string End_Time { get; set; }

        //礼包说明
        [Map("packs_exp")]
        public string Packs_Exp { get; set; }

        //礼包页面
        [Map("packs_url")]
        public string Packs_Url { get; set; }

        //礼包页面
        [Map("write_time")]
        public DateTime Write_Time { get; set; }


        //查询全部礼包
        public List<Packs> SelectPacks()
        {
            var o = Table.Object<Packs>()
                         .Order(m => m.Write_Time, OrderType.Descending)
                         .SelectList();
            return o;
        }


        //查询礼包
        public Packs SelectPack()
        {
            var o = Table.Object<Packs>()
                         .SelectList().FirstOrDefault();
            return o;
        }





    }
}