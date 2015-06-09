using BrewingSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrewingSite.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult test()
        {

            brewappEntities storeDb = new brewappEntities();

            return View(storeDb.Fermentables.ToList<Fermentable>());
        }
    }
}