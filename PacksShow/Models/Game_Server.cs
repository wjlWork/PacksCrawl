using Qmigh.Framework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PacksShow.Models
{
    [Map("game_server")]
    public class Game_Server : DataFieldBase
    {
        [Map("Id")]
        public int Id { get; set; }

        [Map("game_id")]
        public int Game_Id { get; set; }

        [Map("platform")]
        public int Platform { get; set; }

        [Map("server_name")]
        public string Server_Name { get; set; }

        [Map("test_status")]
        public string Test_Status { get; set; }

        [Map("open_date")]
        public DateTime? Open_Date { get; set; }

        [Map("open_time")]
        public DateTime? Open_Time { get; set; }

        [Map("weight")]
        public int Weight { get; set; }

        [Map("status2")]
        public int Status2 { get; set; }

        public string Operator
        {
            get { return GetData("运营商"); }
            set { SetData("运营商", value); }
        }

        public string Contact
        {
            get { return GetData("客服方式"); }
            set { SetData("客服方式", value); }
        }

        public string DownloadUrl
        {
            get { return GetData("下载地址"); }
            set { SetData("下载地址", value); }
        }

        
        public bool SaveData(GameServer gs)
        {
            bool isSave = false;
            string _Operator = "null";
            string _Contact = "null";
            string _DownloadUrl = "null";
            Game_Server gs_1 = new Game_Server();
            if (gs != null)
            {
                gs_1.Game_Id = getGameId(gs.GameName);
                gs_1.Platform = gs.Platform;
                gs_1.Server_Name = gs.ServerName;
                gs_1.Test_Status = gs.TestStatus;
                gs_1.Open_Date = gs.OpenDate;
                gs_1.Open_Time = gs.OpenTime;

                if (!string.IsNullOrEmpty(gs.Operators))
                {
                    _Operator = gs.Operators;
                }
                gs_1.Operator = _Operator;
                if (!string.IsNullOrEmpty(gs.Contact))
                {
                    _Contact = gs.Contact;
                }
                gs_1.Contact = _Contact;
                if (!string.IsNullOrEmpty(gs.DownloadUrl))
                {
                    _DownloadUrl = gs.DownloadUrl;
                }
                gs_1.DownloadUrl = _DownloadUrl;
                gs_1.SerializeData();

            }

            if (!string.IsNullOrEmpty(gs_1.Game_Id.ToString()) &&!string.IsNullOrEmpty(gs_1.Platform.ToString()) && !string.IsNullOrEmpty(gs_1.Server_Name) && !string.IsNullOrEmpty(gs_1.Test_Status) && gs_1.Open_Date != null && gs_1.Open_Time != null)
            {
                DataAccess.Insert(gs_1);
                if (gs_1.Id != 0)
                {
                    //DataAccess.Delete(gs);
                    GameServerDuplicate.SaveDuplicate(gs_1,gs);
                    isSave = true;
                }
            }

            return isSave;
        }

        //游戏ID
        private static int getGameId(string name)
        {
            Game g = Game.Get(name);
            if (g != null)
            {
                return g.Id;
            }
            return 0;
        }






    }
}