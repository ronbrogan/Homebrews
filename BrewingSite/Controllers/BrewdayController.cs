using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrewingSite.Models;

namespace BrewingSite.Controllers
{
    public class BrewdayController : Controller
    {
        brewappEntities dbConn = new brewappEntities();

        // GET: Brewday
        public ActionResult Index()
        {
            return Redirect("/Dashboard/");
        }

        public ActionResult Index(string id="-1")
        {
            if (id == "-1")
                return HttpNotFound("Unable to lookup brewday. A brewday was not specified.");

            WholeBrewday brewday = new WholeBrewday(dbConn.Brewdays.Find(Convert.ToInt32(id)));

            return View(brewday);

        }



        //GET: Brewday/Create/x

        public ActionResult Create(string id = "-1")
        {
            if (id == "-1")
                return HttpNotFound("Unable to create brewday. Recipe ID was not specified.");
            WholeBrewday newBrewday;

            try
            {
                Recipe inputRecipe = dbConn.Recipes.Find(Convert.ToInt32(id));
                newBrewday = new WholeBrewday(inputRecipe);
            }
            catch
            {
                return HttpNotFound("Critical error creating new brewday from recipe id" + id);
            }

            

            return Redirect("/Brewday/" + newBrewday.brewday.id);
        }
    }
}