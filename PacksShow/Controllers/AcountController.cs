using PacksShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PacksShow.Controllers
{
    public class AcountController : Controller
    {
        //
        // GET: /Acount/

        public ActionResult Index()
        {
            List<Statis> staList = Statis.GetStatis();
            return View(staList);
        }

        public ActionResult Detailed(int Id)
        {
            List<Packs_Statistical> packss = Packs_Statistical.GetPcks(Id);

            return View(packss);
        }

    }
}
