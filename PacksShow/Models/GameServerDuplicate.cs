using Qmigh.Framework.DataAccess;
using Qmigh.Framework.DataAccess.TableDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacksShow.Models
{
    [Map("game_server_duplicate")]
    public class GameServerDuplicate : DataFieldBase
    {
        [Map("Id")]
        public int Id { get; set; }

        [Map("save_gameserverid")]
        public int SaveGameServerid { get; set; }

        [Map("load_gameserverid")]
        public int LoadGameServerid { get; set; }

        [Map("server_name")]
        public string ServerName { get; set; }

        public static void SaveDuplicate(Game_Server _gs,GameServer gs)
        {
            GameServerDuplicate gsd = new GameServerDuplicate();
            if (_gs != null)
            {
                gsd.SaveGameServerid = _gs.Id;
                gsd.LoadGameServerid = gs.Id;
                gsd.ServerName = _gs.Server_Name;

                DataAccess.Insert(gsd);
            }
        }


        public static List<GameServer> SelectSaved()
        {
            var savedObject = Table.Object<GameServerDuplicate>()                                
                                   .SelectList();

            List<GameServer> o = new List<GameServer>();
            foreach(var so in savedObject)
            {
                var g = Table.Object<GameServer>()
                             .Where(m => m.Id, so.LoadGameServerid)
                             .SelectList().FirstOrDefault();
                if (g != null)
                {
                    o.Add(g);
                }
            }

            return o;
        }


        public static bool DeleteSaved(int id)
        {
            bool isDelete = false;
            var gs = Table.Object<GameServerDuplicate>()
                          .Where(m => m.LoadGameServerid,id)
                          .SelectList().FirstOrDefault();

            if (gs != null)
            {
                var _gs = Table.Object<Game_Server>()
                               .Where(m => m.Id, gs.SaveGameServerid)
                               .SelectList().FirstOrDefault();

                if (_gs != null)
                {
                    DataAccess.Delete(_gs);
                    DataAccess.Delete(gs);
                    isDelete = true;
                }

            }
            return isDelete;
        }






    }
}