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
            string query = "select top 50 * from recipes where isRecipe=1";

            var data = dbConn.Recipes.SqlQuery(query);

            return View(data);
        }

        //GET: Recipes/recipe/#
        public ActionResult Recipe(string id = "")
        {
            if (id == "")
            {
                string query = "select top 50 * from recipes where isRecipe=1";

                var data = dbConn.Recipes.SqlQuery(query);

                return View("Index", data);
            }
                
            try
            {
                Recipe requestedRecipe = dbConn.Recipes.Find(Convert.ToInt32(id));

                if (requestedRecipe.isRecipe != true)
                    return HttpNotFound("This is not a recipe entry");

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
            EquipmentProfile profile;

            if (dbConn.Recipes.Find(Convert.ToInt32(id)).equipmentProfile == null)
                profile = new EquipmentProfile();
            else
                profile = dbConn.EquipmentProfiles.Find(dbConn.Recipes.Find(Convert.ToInt32(id)).equipmentProfile);

            return PartialView("_EquipmentDialog", profile);
        }

        public ActionResult ShowFermentationDialog(string id = "-1")
        {
            FermentationProfile profile;

            if (dbConn.Recipes.Find(Convert.ToInt32(id)).fermentationProfileId == null)
                profile = new FermentationProfile();
            else
                profile = dbConn.FermentationProfiles.Find(dbConn.Recipes.Find(Convert.ToInt32(id)).fermentationProfileId);

            return PartialView("_FermentationDialog", profile);
        }


        public ActionResult StylesDropdown()
        {
            return PartialView("_StylesDropdown", dbConn.Styles.ToList<Style>());
        }






        [HttpPost]
        public String Delete(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    Recipe toRemove = dbConn.Recipes.Find(Convert.ToInt32(id));

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

            return "Removed entry from database";
        }

        [HttpPost]
        public ActionResult NewRecipe(Recipe recipe)
        {
            try
            {
                recipe.authorId = User.Identity.Name;
                recipe.isRecipe = true;
                recipe.isIngredient = false;
                recipe.isNote = false;



                dbConn.Recipes.Add(recipe);
                dbConn.SaveChanges();
                return RedirectToAction("Recipe", new { recipe.id });
            }
            catch
            {
                return HttpNotFound("Unable to create new recipe");
            }
            
        }

        [HttpPost]
        public String DeleteMashEntry(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    MashEntry toRemove = dbConn.MashEntries.Find(Convert.ToInt32(id));

                    dbConn.MashEntries.Remove(toRemove);
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

            return "Removed entry from database";
        }

        public ActionResult FermentablePane(string id = "-1")
        {
            if (id != "-1")
            {
                try
                {
                    List<Fermentable> ferms = new List<Fermentable>();

                    ferms = dbConn.Database.SqlQuery<Fermentable>("exec dbo.FermentablesList " + id).ToList<Fermentable>();

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
        public String FermentablePanePostOld(string id = "-1")
        {
            try
            {
                string amount = Request["fermentableAmount"].ToString();
                string fermId = Request["fermentableId"].ToString();

                try
                {
                    Convert.ToDouble(amount);  //Converting to doubles will throw an error if text was submitted maliciously.
                    Convert.ToDouble(fermId);
                }
                catch
                {
                    return "No SQL Injections here...";
                }

                Recipe newFermentable = new Recipe();

                newFermentable.amountUnit = "lbs";
                newFermentable.parentId = Convert.ToInt32(id);
                newFermentable.isIngredient = true;
                newFermentable.isNote = false;
                newFermentable.isRecipe = false;
                newFermentable.ingredientId = Convert.ToInt32(fermId);
                newFermentable.ingredientAmount = Convert.ToDouble(amount);
                newFermentable.ingredientType = "Fermentables";

                dbConn.Recipes.Add(newFermentable);
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
                    List<Hop> hops = new List<Hop>();

                    hops = dbConn.Database.SqlQuery<Hop>("exec dbo.HopsList " + id).ToList<Hop>();

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
        public String HopPanePost(string id = "-1")
        {
            try
            {
                string amount = Request["hopAmount"].ToString();
                string hopId = Request["hopId"].ToString(); 
                string hopTime = Request["hopTime"].ToString();
                string isLeaf = Request["isLeaf"].ToString();

                try
                {
                    Convert.ToDouble(amount);  //Converting to doubles will throw an error if text was submitted maliciously.
                    Convert.ToDouble(hopId);
                    Convert.ToDouble(hopTime);
                }
                catch
                {
                    return "No SQL Injections here...";
                }

                Recipe newHop = new Recipe();

                newHop.amountUnit = "oz";
                newHop.parentId = Convert.ToInt32(id);
                newHop.isIngredient = true;
                newHop.isNote = false;
                newHop.isRecipe = false;
                newHop.ingredientId = Convert.ToInt32(hopId);
                newHop.ingredientAmount = Convert.ToDouble(amount);
                newHop.ingredientType = "Hops";
                newHop.additionTime = Convert.ToInt32(hopTime);
                newHop.isLeafHop = false;

                if (isLeaf == "true,false")
                    newHop.isLeafHop = true;


                dbConn.Recipes.Add(newHop);
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
        public String YeastPanePost(string id = "-1")
        {
            try
            {

                int yeastId;

                try
                {
                    yeastId = Convert.ToInt32(Request["yeastId"].ToString());
                }
                catch
                {
                    return "No SQL Injections here...";
                }

                Recipe newYeast = new Recipe();

                newYeast.parentId = Convert.ToInt32(id);
                newYeast.isIngredient = true;
                newYeast.isNote = false;
                newYeast.isRecipe = false;
                newYeast.ingredientId = yeastId;
                newYeast.ingredientType = "Yeast";

                dbConn.Recipes.Add(newYeast);
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

                    if (requestedRecipe.isRecipe != true)
                        return HttpNotFound("This is not a recipe entry");

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
        public String RecipePanePost(string id = "-1")
        {
            try
            {
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

                    EquipmentProfile profile;

                    if (requestedRecipe.equipmentProfile != null)
                    {
                        int equipmentProfileId = (int)requestedRecipe.equipmentProfile;
                        profile = dbConn.EquipmentProfiles.Find(equipmentProfileId);
                    }
                    else
                    {
                        profile = new EquipmentProfile();
                    }

                    if (requestedRecipe.isRecipe != true)
                        return HttpNotFound("This is not a recipe entry");

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
        public String EquipmentPanePost(string id = "-1")
        {
            try
            {
                int recipeId = Convert.ToInt32(id);

                Recipe recipe = dbConn.Recipes.Find(recipeId);

                EquipmentProfile profile;

                double kettleVolume = Convert.ToDouble(Request["kettleVolume"]);
                double kettleVolumeLoss = Convert.ToDouble(Request["kettleVolumeLoss"]);
                double kettleBoiloff = Convert.ToDouble(Request["kettleBoiloff"]);

                double mashtunVolume = Convert.ToDouble(Request["mashtunVolume"]);
                double mashtunVolumeLoss = Convert.ToDouble(Request["mashtunVolumeLoss"]);
                double mashEfficiency = Convert.ToDouble(Request["mashEfficiency"]);


                if(recipe.equipmentProfile != null)
                {
                    int equipmentProfileId = (int)recipe.equipmentProfile;
                    profile = dbConn.EquipmentProfiles.Find(equipmentProfileId);
                }
                else
                {
                    profile = new EquipmentProfile();

                }

                profile.kettleVolume = kettleVolume;
                profile.kettleVolumeLoss = kettleVolumeLoss;
                profile.kettleBoiloff = kettleBoiloff;

                profile.mashtunVolume = mashtunVolume;
                profile.mashtunVolumeLoss = mashtunVolumeLoss;
                profile.mashEfficiency = mashEfficiency;


                if(recipe.equipmentProfile != null)
                {
                    dbConn.EquipmentProfiles.Attach(profile);
                    dbConn.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                    dbConn.SaveChanges();
                }
                else
                {
                    dbConn.EquipmentProfiles.Add(profile);

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
        public String OthersPanePost(string id = "-1")
        {
            try
            {
                int recipeId = Convert.ToInt32(id);

                string otherAmount = Request["otherAmount"];
                string otherItem = Request["otherItem"];
                string otherUse = Request["otherUse"];

                Recipe newOther = new Recipe();

                newOther.parentId = recipeId;
                newOther.isIngredient = true;
                newOther.isNote = false;
                newOther.isRecipe = false;
                newOther.ingredientType = "Other";
                newOther.otherAmount = otherAmount;
                newOther.otherItem = otherItem;
                newOther.otherUse = otherUse;
                
                dbConn.Recipes.Add(newOther);
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
                    List<MashEntry> mashEntries = new List<MashEntry>();

                    mashEntries = dbConn.Database.SqlQuery<MashEntry>("exec dbo.MashEntryList " + id).ToList<MashEntry>();

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
        public String MashPanePost(string id = "-1")
        {
            try
            {
                int recipeId = Convert.ToInt32(id);

                string time = Request["time"];
                string temperature = Request["temperature"];
                string method = Request["method"];

                MashEntry newMashEntry = new MashEntry();

                newMashEntry.recipeId = recipeId;
                newMashEntry.time = Convert.ToInt32(time);
                newMashEntry.temperature = Convert.ToInt32(temperature);
                newMashEntry.method = method;

                dbConn.MashEntries.Add(newMashEntry);
                dbConn.SaveChanges();
            }
            catch
            {
                return "Critical failure updating database with new mash entry.";
            }
            return "Success";
        }

        [HttpPost]
        public String UpdateMashSpargeType(string id = "-1")
        {
            try
            {
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
                    FermentationProfile profile;

                    if (dbConn.Recipes.Find(Convert.ToInt32(id)).fermentationProfileId == null)
                        profile = new FermentationProfile();
                    else
                        profile = dbConn.FermentationProfiles.Find(dbConn.Recipes.Find(Convert.ToInt32(id)).fermentationProfileId);

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
        public String FermentationPanePost(FermentationProfile inputProfile, string id = "-1")
        {
            try
            {
                int recipeId = Convert.ToInt32(id);

                Recipe recipe = dbConn.Recipes.Find(recipeId);

                FermentationProfile profile;


                if (recipe.fermentationProfileId != null)
                {
                    profile = dbConn.FermentationProfiles.Find((int)recipe.fermentationProfileId);
                }
                else
                {
                    profile = new FermentationProfile();

                }

                profile.absorbDetails(inputProfile);
                profile.recipeId = recipe.id;


                if (recipe.fermentationProfileId != null)
                {
                    dbConn.FermentationProfiles.Attach(profile);
                    dbConn.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                    dbConn.SaveChanges();
                }
                else
                {
                    dbConn.FermentationProfiles.Add(profile);

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