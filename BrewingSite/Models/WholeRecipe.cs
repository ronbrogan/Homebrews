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

        public List<Fermentable> fermentables = new List<Fermentable>();
        public List<Hop> hops = new List<Hop>();
        public List<Yeast> yeasts = new List<Yeast>();
        public List<OtherIngredient> others = new List<OtherIngredient>();

        public List<MashEntry> mashEntries = new List<MashEntry>();

        public EquipmentProfile equipment;
        public FermentationProfile fermentation;

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

            equipment = new EquipmentProfile();
            equipment.name = "";
            equipment.kettleVolume = null;
            equipment.kettleVolumeLoss = null;
            equipment.kettleBoiloff = null;
            equipment.mashtunVolume = null;
            equipment.mashtunVolumeLoss = null;

            if (sourceRecipe.equipmentProfile != null)
            {
                    equipment = dbConn.EquipmentProfiles.Find(sourceRecipe.equipmentProfile);
            }

            fermentation = new FermentationProfile();

            if(sourceRecipe.fermentationProfileId != null)
            {
                fermentation = dbConn.FermentationProfiles.Find(sourceRecipe.fermentationProfileId);
            }


            fermentables = dbConn.Database.SqlQuery<Fermentable>("exec dbo.FermentablesList " + sourceRecipe.id.ToString()).ToList<Fermentable>();

            hops = dbConn.Database.SqlQuery<Hop>("exec dbo.HopsList " + sourceRecipe.id.ToString()).ToList<Hop>();

            yeasts = dbConn.Database.SqlQuery<Yeast>("exec dbo.YeastList " + sourceRecipe.id.ToString()).ToList<Yeast>();

            others = dbConn.Database.SqlQuery<OtherIngredient>("exec dbo.OthersList " + sourceRecipe.id.ToString()).ToList<OtherIngredient>();

            mashEntries = dbConn.Database.SqlQuery<MashEntry>("exec dbo.MashEntryList " + sourceRecipe.id.ToString()).ToList<MashEntry>();
        }


    }
}