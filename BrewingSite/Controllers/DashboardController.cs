using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrewingSite.Models;

namespace BrewingSite.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index(string id = "-1")
        {
            UserDashboardCollection dashboard = new UserDashboardCollection(User);

            if (id != "-1")
            {
                switch (Convert.ToInt32(id))
                {
                    case 1: dashboard.errorMessage = "Recipe is missing a necessary field to create the brewday.";
                        break;

                    case 2: dashboard.errorMessage = "Error during database transaction.";
                        break;

                    default: dashboard.errorMessage = "Error " + id;
                        break;
                }
                
                

            }
            
                

            return View(dashboard);
        }
    }
}