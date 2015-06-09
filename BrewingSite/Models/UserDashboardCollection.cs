using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Security;

namespace BrewingSite.Models
{
    public class UserDashboardCollection
    {
        brewappEntities dbConn = new brewappEntities();

        public List<Recipe> recipes;
        //public List<Brewday> brewdays;
        //public List<Fermentation> fermentations;

        public UserDashboardCollection(IPrincipal user)
        {
            var recipesQuery = from recipe in dbConn.Recipes where recipe.authorId == user.Identity.Name select recipe;
            recipes = recipesQuery.ToList<Recipe>();
        }



    }
}