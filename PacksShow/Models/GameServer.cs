using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using System.Web;

namespace PacksShow.Models
{
    [Map("game_server")]
    public class GameServer : DataFieldBase
    {
        [Map("id")]
        public int Id { get; set; }

        [Map("WebName")]
        public string WebName { get; set; }

        [Map("GameName")]
        public string GameName { get; set; }

        [Map("Platform")]
        public int Platform { get; set; }

        [Map("ServerName")]
        public string ServerName { get; set; }

        [Map("TestStatus")]
        public string TestStatus { get; set; }

        [Map("OpenDate")]
        public DateTime? OpenDate { get; set; }

        [Map("OpenTime")]
        public DateTime? OpenTime { get; set; }

        [Map("Operator")]
        public string Operators { get; set; }

        [Map("Contact")]
        public string Contact { get; set; }

        [Map("DownloadUrl")]
        public string DownloadUrl { get; set; }

        //查询全部礼包
        public List<GameServer> allOpenService()
        {
            var o = Table.Object<GameServer>()
                         .Order("OpenTime",OrderType.Descending)
                         .SelectList();
            return o;
        }

        //大于当前时间的全部礼包
        public List<GameServer> GreatOpenService()
        {
            DateTime time = DateTime.Now;
            var o = Table.Object<GameServer>()
                         .Where(m => m.OpenTime, time,ValueCompareType.GreaterOrEquals)
                         .Order("OpenTime", OrderType.Descending)
                         .SelectList();
            return o;
        }


        //查询礼包
        public GameServer selectOpenService(int id)
        {
            var o = Table.Object<GameServer>()
                         .Where(m => m.Id, id)
                         .SelectList().FirstOrDefault();
            return o;
        }

        //查询礼包
        public static void UpdateGameServer(GameServer gs)
        {
            if (gs != null)
            {
                DataAccess.Update(gs);
            }
        }

        public static void Update()
        {
            string _Id = HttpContext.Current.Request.QueryString["Id"];
            string _GameName = HttpContext.Current.Request.QueryString["GameName"];
            string _Platform = HttpContext.Current.Request.QueryString["Platform"];
            string _ServerName = HttpContext.Current.Request.QueryString["ServerName"];
            string _TestStatus = HttpContext.Current.Request.QueryString["TestStatus"];
            string _OpenDate = HttpContext.Current.Request.QueryString["OpenDate"];
            string _OpenTime = HttpContext.Current.Request.QueryString["OpenTime"];
            string _Operators = HttpContext.Current.Request.QueryString["Operators"];
            string _Contact = HttpContext.Current.Request.QueryString["Contact"];

            if (!string.IsNullOrEmpty(_Id))
            {
                var o = Table.Object<GameServer>()
                             .Where(m => m.Id, _Id)
                             .SelectList().FirstOrDefault();

                if (o != null)
                {
                    if (!string.IsNullOrEmpty(_GameName))
                    {
                        o.GameName = _GameName;
                    }
                    o.Platform = int.Parse(_Platform);
                    o.ServerName = _ServerName;
                    o.TestStatus = _TestStatus;
                    if (!string.IsNullOrEmpty(_OpenDate))
                    {
                        o.OpenDate = DateTime.Parse(_OpenDate);
                    }
                    if (!string.IsNullOrEmpty(_OpenTime))
                    {
                        o.OpenTime = DateTime.Parse(_OpenTime);
                    }
                    o.Operators = _Operators;
                    o.Contact = _Contact;
                    DataAccess.Update(o);
                }
            }
        }


        public List<GameServer> ShuaiGameServer()
        {
            List<GameServer> gsList = new List<GameServer>();
            var g_os = allOpenService();
            foreach (var o in g_os)
            {
                var game = Game.Get(o.GameName);
                if (game != null)
                {
                    gsList.Add(o);
                }

            }
            return gsList;
        }

        public List<GameServer> MatchTimeGameServer()
        {
            List<GameServer> gsList = new List<GameServer>();
            var g_os = GreatOpenService();
            foreach (var o in g_os)
            {
                var game = Game.Get(o.GameName);
                if (game != null)
                {
                    gsList.Add(o);
                }

            }
            return gsList;
        }


    }
}
