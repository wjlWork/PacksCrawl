using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using PacksModels;

namespace OpenList
{
    [Map("Game_Server_Config")]
    public class GameServerConfig:DataFieldBase
    {
        [Map("id")]
        public int Id { get; set; }

        [Map("Web_Name")]
        public string WebName { get; set; }

        [Map("Home_Url")]
        public string HomeUrl { get; set; }

        [Map("List_Url")]
        public string ListUrl { get; set; }

        [Map("Trs_Xpath")]
        public string TrsXpath { get; set; }

        [Map("GameName_Xpath")]
        public string GameNameXpath { get; set; }

        [Map("Platform_Xpath")]
        public string PlatformXpath { get; set; }

        [Map("ServerName_Xpath")]
        public string ServerNameXpath { get; set; }

        [Map("TestStatus_Xpath")]
        public string TestStatusXpath { get; set; }

        [Map("OpenDate_Xpath")]
        public string OpenDateXpath { get; set; }

        [Map("OpenTime_Xpath")]
        public string OpenTimeXpath { get; set; }

        [Map("Operator_Xpath")]
        public string OperatorXpath { get; set; }

        [Map("Contact_Xpath")]
        public string ContactXpath { get; set; }

        [Map("DownloadUrl_Xpath")]
        public string DownloadUrlXpath { get; set; }



        public static GameServerConfig selectConfig(string _listUrl)
        {
            GameServerConfig osc = null;
            var o = Table.Object<GameServerConfig>()
                         .Where(m => m.ListUrl, _listUrl)
                         .SelectList().FirstOrDefault();
            if (o != null)
            {
                osc = o;
            }
            return osc;
        }


        public static GameServerConfig selectConfig1(string webname)
        {
            GameServerConfig osc = null;
            var o = Table.Object<GameServerConfig>()
                         .Where(m => m.WebName, webname)
                         .SelectList().FirstOrDefault();
            if (o != null)
            {
                osc = o;
            }
            return osc;
        }


        public static List<GameServerConfig> allConfig()
        {
            List<GameServerConfig> oscs = new List<GameServerConfig>();
            var o = Table.Object<GameServerConfig>()
                         .SelectList();
            if (o != null)
            {
                oscs = o;
            }
            return oscs;
        }



    }
}
