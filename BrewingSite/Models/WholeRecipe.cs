using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BrewingSite.Models
{
    public class WholeRecipe
    {
        brewappEntities dbConn = new brewappEntities();

        public Recipe sourceRecipe;

        public List<ViewFermentable> fermentables = new List<ViewFermentable>();
        public List<ViewHop> hops = new List<ViewHop>();
        public List<Yeast> yeasts = new List<Yeast>();
        public List<OtherIngredient> others = new List<OtherIngredient>();

        public List<RecipeMashEntry> mashEntries = new List<RecipeMashEntry>();

        public RecipeEquipmentProfile equipment;
        public RecipeFermentationProfile fermentation;

        public Style recipeStyle;

        public double boilTime;
        public int recipeType;
        public string recipeTypeDisplay;

        public string recipeTitle;
        public double batchSize;

        public WholeRecipe(Recipe inputRecipe)
        {
            sourceRecipe = inputRecipe;
            if(sourceRecipe.boilTime != null)
                boilTime = (double)sourceRecipe.boilTime;
            if(sourceRecipe.recipeType != null)
                recipeType = (int)sourceRecipe.recipeType;

            recipeTitle = sourceRecipe.name;

            if(sourceRecipe.batchSize != null) //Have to check if null, otherwise the cast causes failure
                batchSize = (double)sourceRecipe.batchSize;

            recipeStyle = dbConn.Styles.Find(sourceRecipe.styleId);

            equipment = new RecipeEquipmentProfile();
            equipment.name = "";
            equipment.kettleVolume = null;
            equipment.kettleVolumeLoss = null;
            equipment.kettleBoiloff = null;
            equipment.mashtunVolume = null;
            equipment.mashtunVolumeLoss = null;

            if (sourceRecipe.equipmentProfile != null)
            {
                equipment = dbConn.RecipeEquipmentProfiles.Find(sourceRecipe.equipmentProfile);
            }

            fermentation = new RecipeFermentationProfile();

            if(sourceRecipe.fermentationProfileId != null)
            {
                fermentation = dbConn.RecipeFermentationProfiles.Find(sourceRecipe.fermentationProfileId);
            }


            fermentables = dbConn.Database.SqlQuery<ViewFermentable>("exec dbo.FermentablesList " + sourceRecipe.id.ToString()).ToList<ViewFermentable>();

            hops = dbConn.Database.SqlQuery<ViewHop>("exec dbo.HopsList " + sourceRecipe.id.ToString()).ToList<ViewHop>();

            yeasts = dbConn.Database.SqlQuery<Yeast>("exec dbo.YeastList " + sourceRecipe.id.ToString()).ToList<Yeast>();

            others = dbConn.Database.SqlQuery<OtherIngredient>("exec dbo.OthersList " + sourceRecipe.id.ToString()).ToList<OtherIngredient>();

            mashEntries = dbConn.Database.SqlQuery<RecipeMashEntry>("exec dbo.MashEntryList " + sourceRecipe.id.ToString()).ToList<RecipeMashEntry>();
        }


    }
}