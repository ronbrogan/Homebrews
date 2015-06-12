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
            return View();
        }

        //GET: Brewday/Create/x

        public ActionResult Create(string id = "-1")
        {
            if (id == "-1")
                return HttpNotFound("Unable to create brewday. Recipe ID is not specified.");

            List<RecipeFermentable> recipeFerms = new List<RecipeFermentable>();
            List<BrewdayFermentable> brewdayFerms = new List<BrewdayFermentable>();

            Recipe inputRecipe;
            int brewdayId = 0;

            try
            {
                inputRecipe = dbConn.Recipes.Find(Convert.ToInt32(id));
            }
            catch
            {
                return HttpNotFound("Unable to create brewday. Recipe ID is not valid");
            }

            try
            {
                brewdayFerms = dbConn.Database.SqlQuery<BrewdayFermentable>("exec dbo.BrewdayFermList " + id).ToList<BrewdayFermentable>();

            }
            catch
            {
                return HttpNotFound("Unable to construct objects properly.");
            }




            return null;

            //return Redirect("/Brewday/" + brewdayId);
        }
    }
}