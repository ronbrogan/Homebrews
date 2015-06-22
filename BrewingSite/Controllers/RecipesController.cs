using BrewingSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrewingSite.Controllers
{
    public class RecipesController : Controller
    {
        brewappEntities dbConn = new brewappEntities();

        // GET: Recipes
        public ActionResult Index()
        {
            var data = from recipes in dbConn.Recipes where (bool)recipes.isPublicRead==true select recipes;

            return View(data);
        }

        //GET: Recipes/recipe/#
        [Authorize]
        public ActionResult Recipe(string id = "")
        {
            
            if (id == "")
            {
                return Redirect("/Dashboard/");
            }
                
            try
            {
                Recipe requestedRecipe = dbConn.Recipes.Find(Convert.ToInt32(id));

                if(User.Identity.Name != requestedRecipe.authorId.TrimEnd(' ') && !(bool)requestedRecipe.isPublicRead)
                    return HttpNotFound("Invalid user for access to recipe!");

                WholeRecipe wholeShebang = new WholeRecipe(requestedRecipe);

                return View(wholeShebang);
            }

            catch
            {
                return HttpNotFound("Error during lookup");
            }
        }



        public ActionResult ShowNewRecipeDialog()
        {
            return PartialView("_NewRecipeDialog");
        }

        public ActionResult AddFermentable(string id = "")
        {
            return View(dbConn.Fermentables.ToList<Fermentable>());
        }

        public ActionResult ShowFermentableDialog()
        {
            return PartialView("_FermentableDialog", dbConn.Fermentables.ToList<Fermentable>());
        }

        public ActionResult ShowHopDialog()
        {
            return PartialView("_HopDialog", dbConn.Hops.ToList<Hop>());
        }

        public ActionResult ShowYeastDialog()
        {
            return PartialView("_YeastDialog", dbConn.Yeasts.ToList<Yeast>());
        }

        public ActionResult ShowRecipeDialog(string id="-1")
        {
            return PartialView("_RecipeDialog", dbConn.Recipes.Find(Convert.ToInt32(id)));
        }

        public ActionResult ShowEquipmentDialog(string id = "-1")
        {
            RecipeEquipmentProfile profile;

            if (dbConn.Recipes.Find(Convert.ToInt32(id)).equipmentProfile == null)
                profile = new RecipeEquipmentProfile();
            else
                profile = dbConn.RecipeEquipmentProfiles.Find(dbConn.Recipes.Find(Convert.ToInt32(id)).equipmentProfile);

            return PartialView("_EquipmentDialog", profile);
        }

        public ActionResult ShowFermentationDialog(string id = "-1")
        {
            RecipeFermentationProfile profile;

            if (dbConn.Recipes.Find(Convert.ToInt32(id)).fermentationProfileId == null)
                profile = new RecipeFermentationProfile();
            else
                profile = dbConn.RecipeFermentationProfiles.Find(dbConn.Recipes.Find(Convert.ToInt32(id)).fermentationProfileId);

            return PartialView("_FermentationDialog", profile);
        }


        public ActionResult StylesDropdown()
        {
            return PartialView("_StylesDropdown", dbConn.Styles.ToList<Style>());
        }






        [HttpPost]
        [Authorize]
        public String Delete(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    Recipe toRemove = dbConn.Recipes.Find(Convert.ToInt32(id));

                    if (User.Identity.Name != toRemove.authorId.TrimEnd(' '))
                        return "Invalid user for access to recipe!";

                    dbConn.Recipes.Remove(toRemove);
                    dbConn.SaveChanges();
                }
                catch
                {
                    return "Error deleting record";
                }
                
            }
            else
            {
                return "Not valid entry";
            }

            return "Success";
        }

        [HttpPost]
        [Authorize]
        public String DeleteFermentable(string id = "-1")
        {
            
            if (id != "-1")
            {
                try
                {
                    RecipeFermentable toRemove = dbConn.RecipeFermentables.Find(Convert.ToInt32(id));

                    if (User.Identity.Name != dbConn.Recipes.Find(toRemove.recipeId).authorId.TrimEnd(' '))
                        return "Invalid user for access to recipe!";

                    dbConn.RecipeFermentables.Remove(toRemove);
                    dbConn.SaveChanges();
                }
                catch
                {
                    return "Error deleting record";
                }

            }
            else
            {
                return "Not valid entry";
            }

            return "Success";
        }

        [HttpPost]
        [Authorize]
        public String DeleteHop(string id = "-1")
        {

            if (id != "-1")
            {
                try
                {
                    RecipeHop toRemove = dbConn.RecipeHops.Find(Convert.ToInt32(id));

                    if (User.Identity.Name != dbConn.Recipes.Find(toRemove.recipeId).authorId.TrimEnd(' '))
                        return "Invalid user for access to recipe!";

                    dbConn.RecipeHops.Remove(toRemove);
                    dbConn.SaveChanges();
                }
                catch
                {
                    return "Error deleting record";
                }

            }
            else
            {
                return "Not valid entry";
            }

            return "Success";
        }


        [HttpPost]
        [Authorize]
        public String DeleteOther(string id = "-1")
        {

            if (id != "-1")
            {
                try
                {
                    RecipeOther toRemove = dbConn.RecipeOthers.Find(Convert.ToInt32(id));

                    if (User.Identity.Name != dbConn.Recipes.Find(toRemove.recipeId).authorId.TrimEnd(' '))
                        return "Invalid user for access to recipe!";

                    dbConn.RecipeOthers.Remove(toRemove);
                    dbConn.SaveChanges();
                }
                catch
                {
                    return "Error deleting record";
                }

            }
            else
            {
                return "Not valid entry";
            }

            return "Success";
        }

        [HttpPost]
        [Authorize]
        public String DeleteYeast(string id = "-1")
        {

            if (id != "-1")
            {
                try
                {
                    RecipeYeast toRemove = dbConn.RecipeYeasts.Find(Convert.ToInt32(id));

                    if (User.Identity.Name != dbConn.Recipes.Find(toRemove.recipeId).authorId.TrimEnd(' '))
                        return "Invalid user for access to recipe!";

                    dbConn.RecipeYeasts.Remove(toRemove);
                    dbConn.SaveChanges();
                }
                catch
                {
                    return "Error deleting record";
                }

            }
            else
            {
                return "Not valid entry";
            }

            return "Success";
        }

        [HttpPost]
        [Authorize]
        public String DeleteMashEntry(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    RecipeMashEntry toRemove = dbConn.RecipeMashEntries.Find(Convert.ToInt32(id));

                    if (User.Identity.Name != dbConn.Recipes.Find(toRemove.recipeId).authorId.TrimEnd(' '))
                        return "Invalid user for access to recipe!";

                    dbConn.RecipeMashEntries.Remove(toRemove);
                    dbConn.SaveChanges();
                }
                catch
                {
                    return "Error deleting record";
                }

            }
            else
            {
                return "Not valid entry";
            }

            return "Success";
        }




        [HttpPost]
        public ActionResult NewRecipe(Recipe recipe)
        {
            try
            {
                recipe.authorId = User.Identity.Name;
                recipe.isPublicRead = false;
                recipe.mashSpargeType = "Batch";


                dbConn.Recipes.Add(recipe);
                dbConn.SaveChanges();
                return RedirectToAction("Recipe", new { recipe.id });
            }
            catch
            {
                return HttpNotFound("Unable to create new recipe");
            }
            
        }

        

        public ActionResult FermentablePane(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    List<ViewFermentable> ferms = new List<ViewFermentable>();

                    ferms = dbConn.Database.SqlQuery<ViewFermentable>("exec dbo.FermentablesList " + id).ToList<ViewFermentable>();

                    return PartialView("_FermentablePane", ferms);
                }
                catch
                {
                    return HttpNotFound("Error while getting fermentable list");
                }
                
            }
            return HttpNotFound("Not valid recipe ID");
        }


        [HttpPost]
        [ActionName("FermentablePane")]
        [Authorize]
        public String FermentablePanePost(RecipeFermentable newFermentable, string id = "-1")
        {
            try
            {
                if (User.Identity.Name != dbConn.Recipes.Find(Convert.ToInt32(id)).authorId.TrimEnd(' '))
                    return "Invalid user for access to recipe!";

                newFermentable.recipeId = Convert.ToInt32(id);
                newFermentable.unit = "lbs";

                dbConn.RecipeFermentables.Add(newFermentable);
                dbConn.SaveChanges();
            }
            catch
            {
                return "Critical failure adding fermentable to the database.";
            }
            return "Success";
        }

        public ActionResult HopPane(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    List<ViewHop> hops = new List<ViewHop>();

                    hops = dbConn.Database.SqlQuery<ViewHop>("exec dbo.HopsList " + id).ToList<ViewHop>();

                    return PartialView("_HopPane", hops);
                }
                catch
                {
                    return HttpNotFound("Error while getting hop list");
                }

            }
            return HttpNotFound("Not valid recipe ID");
        }

        [HttpPost]
        [ActionName("HopPane")]
        [Authorize]
        public String HopPanePost(RecipeHop newHop, string id = "-1")
        {
            try
            {
                if (User.Identity.Name != dbConn.Recipes.Find(Convert.ToInt32(id)).authorId.TrimEnd(' '))
                    return "Invalid user for access to recipe!";

                newHop.recipeId = Convert.ToInt32(id);
                newHop.unit = "oz";

                dbConn.RecipeHops.Add(newHop);
                dbConn.SaveChanges();
            }
            catch
            {
                return "Critical failure adding hop to the database.";
            }
            return "Success";
        }


        public ActionResult YeastPane(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    List<Yeast> yeasts = new List<Yeast>();

                    yeasts = dbConn.Database.SqlQuery<Yeast>("exec dbo.YeastList " + id).ToList<Yeast>();

                    return PartialView("_YeastPane", yeasts);
                }
                catch
                {
                    return HttpNotFound("Error while getting yeasts list");
                }

            }
            return HttpNotFound("Not valid recipe ID");
        }

        [HttpPost]
        [ActionName("YeastPane")]
        [Authorize]
        public String YeastPanePost(RecipeYeast newYeast, string id = "-1")
        {
            try
            {
                if (User.Identity.Name != dbConn.Recipes.Find(Convert.ToInt32(id)).authorId.TrimEnd(' '))
                    return "Invalid user for access to recipe!";

                newYeast.recipeId = Convert.ToInt32(id);

                dbConn.RecipeYeasts.Add(newYeast);
                dbConn.SaveChanges();
            }
            catch
            {
                return "Critical failure adding yeast to the database.";
            }
            return "Success";
        }



        public ActionResult RecipePane(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    Recipe requestedRecipe = dbConn.Recipes.Find(Convert.ToInt32(id));

                    WholeRecipe wholeShebang = new WholeRecipe(requestedRecipe);

                    return PartialView("_RecipePane", wholeShebang);
                }
                catch
                {
                    return HttpNotFound("Error while getting recipe details");
                }

            }
            return HttpNotFound("Not valid recipe ID");
        }

        [HttpPost]
        [ActionName("RecipePane")]
        [Authorize]
        public String RecipePanePost(string id = "-1")
        {
            try
            {
                if (User.Identity.Name != dbConn.Recipes.Find(Convert.ToInt32(id)).authorId.TrimEnd(' '))
                    return "Invalid user for access to recipe!";

                int recipeId = Convert.ToInt32(id);

                string styleId = Request["styleId"].ToString();

                if (styleId != "" && styleId != "-1")
                    dbConn.Recipes.Find(recipeId).styleId = Convert.ToInt32(styleId);

                dbConn.Recipes.Find(recipeId).name = Request["name"].ToString();
                
                dbConn.Recipes.Find(recipeId).authorName = Request["authorName"].ToString();
                dbConn.Recipes.Find(recipeId).batchSize = Convert.ToDouble(Request["batchSize"].ToString());
                dbConn.Recipes.Find(recipeId).boilTime = Convert.ToDouble(Request["boilTime"].ToString());

                dbConn.SaveChanges();
            }
            catch
            {
                return "Critical failure updating database with new information.";
            }
            return "Success";
        }



        public ActionResult EquipmentPane(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    Recipe requestedRecipe = dbConn.Recipes.Find(Convert.ToInt32(id));

                    RecipeEquipmentProfile profile;

                    if (requestedRecipe.equipmentProfile != null)
                    {
                        int equipmentProfileId = (int)requestedRecipe.equipmentProfile;
                        profile = dbConn.RecipeEquipmentProfiles.Find(equipmentProfileId);
                    }
                    else
                    {
                        profile = new RecipeEquipmentProfile();
                    }

                    return PartialView("_EquipmentPane", profile);
                }
                catch
                {
                    return HttpNotFound("Error while getting recipe details");
                }

            }
            return HttpNotFound("Not valid recipe ID");
        }

        [HttpPost]
        [ActionName("EquipmentPane")]
        [Authorize]
        public String EquipmentPanePost(string id = "-1")
        {
            try
            {
                if (User.Identity.Name != dbConn.Recipes.Find(Convert.ToInt32(id)).authorId.TrimEnd(' '))
                    return "Invalid user for access to recipe!";

                int recipeId = Convert.ToInt32(id);

                Recipe recipe = dbConn.Recipes.Find(recipeId);

                RecipeEquipmentProfile profile;

                double kettleVolume = Convert.ToDouble(Request["kettleVolume"]);
                double kettleVolumeLoss = Convert.ToDouble(Request["kettleVolumeLoss"]);
                double kettleBoiloff = Convert.ToDouble(Request["kettleBoiloff"]);

                double mashtunVolume = Convert.ToDouble(Request["mashtunVolume"]);
                double mashtunVolumeLoss = Convert.ToDouble(Request["mashtunVolumeLoss"]);
                double mashEfficiency = Convert.ToDouble(Request["mashEfficiency"]);


                if(recipe.equipmentProfile != null)
                {
                    int equipmentProfileId = (int)recipe.equipmentProfile;
                    profile = dbConn.RecipeEquipmentProfiles.Find(equipmentProfileId);
                }
                else
                {
                    profile = new RecipeEquipmentProfile();

                }

                profile.kettleVolume = kettleVolume;
                profile.kettleVolumeLoss = kettleVolumeLoss;
                profile.kettleBoiloff = kettleBoiloff;

                profile.mashtunVolume = mashtunVolume;
                profile.mashtunVolumeLoss = mashtunVolumeLoss;
                profile.mashEfficiency = mashEfficiency;


                if(recipe.equipmentProfile != null)
                {
                    dbConn.RecipeEquipmentProfiles.Attach(profile);
                    dbConn.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                    dbConn.SaveChanges();
                }
                else
                {
                    dbConn.RecipeEquipmentProfiles.Add(profile);

                    dbConn.SaveChanges();

                    dbConn.Recipes.Find(recipeId).equipmentProfile = profile.id;

                    dbConn.SaveChanges();
                }
                

                
            }
            catch
            {
                return "Critical failure updating database with new information.";
            }
            return "Success";
        }


        public ActionResult OthersPane(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    List<OtherIngredient> others = new List<OtherIngredient>();

                    others = dbConn.Database.SqlQuery<OtherIngredient>("exec dbo.OthersList " + id).ToList<OtherIngredient>();

                    return PartialView("_OthersPane", others);
                }
                catch
                {
                    return HttpNotFound("Error while getting other ingredients list");
                }

            }
            return HttpNotFound("Not valid recipe ID");
        }

        [HttpPost]
        [ActionName("OthersPane")]
        [Authorize]
        public String OthersPanePost(RecipeOther newOther, string id = "-1")
        {
            try
            {
                if (User.Identity.Name != dbConn.Recipes.Find(Convert.ToInt32(id)).authorId.TrimEnd(' '))
                    return "Invalid user for access to recipe!";

                newOther.recipeId = Convert.ToInt32(id);

                dbConn.RecipeOthers.Add(newOther);
                dbConn.SaveChanges();
            }
            catch
            {
                return "Critical failure updating database with new information.";
            }
            return "Success";
        }



        public ActionResult MashPane(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    List<RecipeMashEntry> mashEntries = new List<RecipeMashEntry>();

                    mashEntries = dbConn.Database.SqlQuery<RecipeMashEntry>("exec dbo.MashEntryList " + id).ToList<RecipeMashEntry>();

                    return PartialView("_MashPane", mashEntries);
                }
                catch
                {
                    return HttpNotFound("Error while getting other ingredients list");
                }

            }
            return HttpNotFound("Not valid recipe ID");
        }

        [HttpPost]
        [ActionName("MashPane")]
        [Authorize]
        public String MashPanePost(string id = "-1")
        {
            try
            {
                if (User.Identity.Name != dbConn.Recipes.Find(Convert.ToInt32(id)).authorId.TrimEnd(' '))
                    return "Invalid user for access to recipe!";

                int recipeId = Convert.ToInt32(id);

                string time = Request["time"];
                string temperature = Request["temperature"];
                string method = Request["method"];

                RecipeMashEntry newMashEntry = new RecipeMashEntry();

                newMashEntry.recipeId = recipeId;
                newMashEntry.time = Convert.ToInt32(time);
                newMashEntry.temperature = Convert.ToInt32(temperature);
                newMashEntry.method = method;

                dbConn.RecipeMashEntries.Add(newMashEntry);
                dbConn.SaveChanges();
            }
            catch
            {
                return "Critical failure updating database with new mash entry.";
            }
            return "Success";
        }

        [HttpPost]
        [Authorize]
        public String UpdateMashSpargeType(string id = "-1")
        {
            try
            {
                if (User.Identity.Name != dbConn.Recipes.Find(Convert.ToInt32(id)).authorId.TrimEnd(' '))
                    return "Invalid user for access to recipe!";

                int recipeId = Convert.ToInt32(id);

                string mashSpargeMethod = Request["mashSpargeMethod"];

                dbConn.Recipes.Find(recipeId).mashSpargeType = mashSpargeMethod;
                dbConn.SaveChanges();
            }
            catch
            {
                return "Critical failure updating database with new mash entry.";
            }
            return "Success";
        }


        public ActionResult FermentationPane(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    RecipeFermentationProfile profile;

                    if (dbConn.Recipes.Find(Convert.ToInt32(id)).fermentationProfileId == null)
                        profile = new RecipeFermentationProfile();
                    else
                        profile = dbConn.RecipeFermentationProfiles.Find(dbConn.Recipes.Find(Convert.ToInt32(id)).fermentationProfileId);

                    return PartialView("_FermentationPane", profile);
                }
                catch
                {
                    return HttpNotFound("Error while getting recipe details");
                }

            }
            return HttpNotFound("Not valid recipe ID");
        }

        [HttpPost]
        [ActionName("FermentationPane")]
        [Authorize]
        public String FermentationPanePost(RecipeFermentationProfile inputProfile, string id = "-1")
        {
            try
            {
                if (User.Identity.Name != dbConn.Recipes.Find(Convert.ToInt32(id)).authorId.TrimEnd(' '))
                    return "Invalid user for access to recipe!";

                int recipeId = Convert.ToInt32(id);

                Recipe recipe = dbConn.Recipes.Find(recipeId);

                RecipeFermentationProfile profile;


                if (recipe.fermentationProfileId != null)
                {
                    profile = dbConn.RecipeFermentationProfiles.Find((int)recipe.fermentationProfileId);
                }
                else
                {
                    profile = new RecipeFermentationProfile();

                }

                profile.absorbDetails(inputProfile);
                profile.recipeId = recipe.id;


                if (recipe.fermentationProfileId != null)
                {
                    dbConn.RecipeFermentationProfiles.Attach(profile);
                    dbConn.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                    dbConn.SaveChanges();
                }
                else
                {
                    dbConn.RecipeFermentationProfiles.Add(profile);

                    dbConn.SaveChanges();

                    dbConn.Recipes.Find(recipeId).fermentationProfileId = profile.id;

                    dbConn.SaveChanges();
                }



            }
            catch
            {
                return "Critical failure updating database with new information.";
            }
            return "Success";
        }



        public String srmToRgb(string srm = "-1")
        {
            if (srm == "-1")
                return "Invalid";

            double srmDouble;

            try
            {
                srmDouble = Convert.ToDouble(srm);

                if(srmDouble >= 0 && srmDouble <= 30)
                {
                    SRMtoRGB rgb = dbConn.Database.SqlQuery<SRMtoRGB>("exec dbo.srmToRgbProcedure " + srm.ToString()).ToList<SRMtoRGB>().First();

                    return rgb.Red.ToString() + "," + rgb.Green.ToString() + "," + rgb.Blue.ToString();
                }
                else if(srmDouble < 0)
                {
                    return "255,255,255";
                }
                else
                {
                    return "0,0,0";
                }

            }
            catch
            {
                return "No SQL Injections Here...";
            }

            

        }


    }
}