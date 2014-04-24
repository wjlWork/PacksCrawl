using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PacksShow.Models;

namespace PacksShow.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            Packs pck = new Packs();
            List<Packs> ls = new List<Packs>();
            ls = pck.SelectPacks();
            //pck = pck.SelectPack();

            return View(ls);
        }

        public ActionResult ShowList()
        {
            GameServer OpS = new GameServer();
            List<GameServer> ls = new List<GameServer>();
            ls = OpS.allOpenService();
            return View(ls);
        }

        public ActionResult Shift()
        {
            string[] IDs = Request.Form.GetValues(0);
            GameServer gs = new GameServer();
            foreach (var i in IDs)
            {
                int id = int.Parse(i);
                gs = gs.selectOpenService(id);
                Game_Server gs_1 = new Game_Server ();
                ViewBag.isSave = gs_1.SaveData(gs);
            }
            return View("ShowList");
        }

        public ActionResult UpdateList(int id)
        {
            ViewBag.ID = id;
            GameServer gs = new GameServer();
            gs = gs.selectOpenService(id);
            GameServer.Update();
            return View(gs);
        }


        public ActionResult Shuai()
        {
            List<GameServer> gsList = new List<GameServer> ();
            GameServer gs = new GameServer();
            gsList = gs.ShuaiGameServer();
            return View("ShowList",gsList);
        }

        public ActionResult MatchTime()
        {
            List<GameServer> gsList = new List<GameServer>();
            GameServer gs = new GameServer();
            gsList = gs.MatchTimeGameServer();
            return View("ShowList", gsList);
        }


        public ActionResult Saved()
        {
            List<GameServer> gsList = GameServerDuplicate.SelectSaved();
            ViewBag.Status = "Saved";
            return View("ShowList",gsList);
        }


        public ActionResult DeleteSaved(int id)
        {
            bool isDel = false;
            if (id > 0)
            {
                isDel = GameServerDuplicate.DeleteSaved(id);
            }
            if (isDel)
            {
                ViewBag.isDel = "del";
            }
            return View("ShowList");
        }

        public ActionResult ShowCount()
        {
            Packs_Statistical pck = new Packs_Statistical();
            List<Packs_Statistical> lsPck = pck.GetPcks();
            return View(lsPck);
        }


        public ActionResult ShowCounts()
        {
            ShowCounts sCs = new ShowCounts();
            List<ShowCounts> sCLs = sCs.Gets();
            return View();
        }


    }
}
