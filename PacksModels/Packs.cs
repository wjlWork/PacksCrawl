using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;

namespace PacksModels
{
    [Map("packs")]
    public class Packs:DataFieldBase
    {
        //礼包名
        [Map("packs_name")]
        public string Packs_Name { get; set; }

        //礼包对应有效名
        [Map("game_name")]
        public string Game_Name { get; set; }

        //礼包有效期
        [Map("packs_valid")]
        public string Packs_Valid { get; set; }

        //礼包说明
        [Map("packs_exp")]
        public string Packs_Exp { get; set; }

        //礼包页面
        [Map("packs_url")]
        public string Packs_Url { get; set; }


        //查询
        public static Packs SelectPack(string packs_name)
        {
            var o = Table.Object<Packs>()
             .Where(m => m.Packs_Name, packs_name)
             .SelectList().FirstOrDefault();
            return o;
        }

        //查询
        public static void Insert()
        {
            var o = new Packs
            {
                Packs_Name ="qq1",
                Game_Name = "qq2",
                Packs_Valid = "qq3",
                Packs_Exp = "qq4",
                Packs_Url = "qq5"
            };

            DataAccess.Insert(o);
        }




    }
}
