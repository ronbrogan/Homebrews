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
                try
                {
                    switch (Convert.ToInt32(id))
                    {
                        case 1: dashboard.errorMessage = "Recipe is missing a necessary field to create the brewday.";
                            break;

                        case 2: dashboard.errorMessage = "Error during database transaction.";
                            break;

                        case 3: dashboard.errorMessage = "Unable to create new recipe for unspecified reason.";
                            break;

                        case 4: dashboard.errorMessage = "Recipe contains no yeast entity. Please add a yeast.";
                            break;

                        case 5: dashboard.errorMessage = "Recipe contains no mash entry. Please add a mash entry.";
                            break;

                        case 6: dashboard.errorMessage = "Recipe contains no fermentable entity. Please add a fermentable.";
                            break;

                        case 7: dashboard.errorMessage = "Recipe contains no hop entity. Please add a hop.";
                            break;

                        case 8: dashboard.errorMessage = "No parameter specified in request.";
                            break;

                        case -2147467261: dashboard.errorMessage = "Unknown error occured, please correct any errors and try again";
                            break;

                        default: dashboard.errorMessage = "Error " + id;
                            break;
                    }
                }

                catch
                {
                    dashboard.errorMessage = "It's broke. All hope is lost. Save yourself.";
                }
                
                
                

            }
            
                

            return View(dashboard);
        }
    }
}