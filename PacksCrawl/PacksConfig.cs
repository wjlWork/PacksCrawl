using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using PacksModels;

namespace PacksCrawl
{
    [Map("packsconfig")]
    public class PacksConfig : DataFieldBase
    {
        //ID
        [Map("id")]
        public string ID { get; set; }

        //礼包主页
        [Map("home_url")]
        public string HomeUrl { get; set; }

        //礼包主页
        [Map("web_name")]
        public string WebName { get; set; }

        //礼包列表页
        [Map("list_url")]
        public string ListUrl { get; set; }

        //礼包列表页xpath
        [Map("list_xpath")]
        public string ListXpath { get; set; }

        //下一页xpath
        [Map("page_xpath")]
        public string PageXpath { get; set; }

        //礼包名xpath
        [Map("packs_name_xpath")]
        public string PacksNameXpath { get; set; }

        //礼包剩余数量
        [Map("packs_count_xpath")]
        public string PacksCountXpath { get; set; }

        //礼包说明
        [Map("packs_exp_xpath")]
        public string PacksExpXpath { get; set; }

        //礼包有效期
        [Map("packs_valid_xpath")]
        public string PacksValidXpath { get; set; }

        //礼包开始时间
        [Map("strat_time_xpath")]
        public string StratTimeXpath { get; set; }

        //对应游戏名
        [Map("packs_game_xpath")]
        public string PacksGameXpath { get; set; }


        //查询所有配置数据
        public static List<PacksConfig> allConfig()
        {
            var o = Table.Object<PacksConfig>()
                         .SelectList();
            return o;
        }


        //查询
        public static PacksConfig SelectConfig(string home_url)
        {
            var o = Table.Object<PacksConfig>()
             .Where(m => m.HomeUrl, home_url)
             .SelectList().FirstOrDefault();

            return o;
        }


        public static void Insert(string insertData)       
        {
            string[] Fields = insertData.Split(',');
            string home_url = Fields[0].Split(':')[1] + ":" + Fields[0].Split(':')[2];
            string list_url = Fields[1].Split(':')[1] + ":" + Fields[1].Split(':')[2];

            var pc = new PacksConfig
            {
                HomeUrl = home_url,
                ListUrl =list_url
            };

            if (pc != null)
            {
                DataAccess.Insert(pc);
            }
        }



    }
}
