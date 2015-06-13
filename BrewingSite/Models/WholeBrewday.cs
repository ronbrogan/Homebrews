using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrewingSite.Models
{
    public class WholeBrewday
    {
        brewappEntities dbConn = new brewappEntities();

        public Brewday brewday;

        public List<ViewFermentable> fermentables = new List<ViewFermentable>();
        public List<ViewHop> hops = new List<ViewHop>();
        public List<Yeast> yeasts = new List<Yeast>();
        public List<OtherIngredient> others = new List<OtherIngredient>();

        public List<BrewdayMashEntry> mashEntries = new List<BrewdayMashEntry>();

        public BrewdayEquipmentProfile equipment;
        public BrewdayFermentationProfile fermentation;

        public Style recipeStyle;

        public double boilTime;
        public int recipeType;
        public string recipeTypeDisplay;

        public string recipeTitle;
        public double batchSize;

        public WholeBrewday(Recipe inputRecipe)  //If passed a recipe object, we're to create a new brewday entry in the database from that recipe object then compile data to display
        {
            brewday = new Brewday();
            //Copy relevant data from inputRecipe to brewday object

            brewday.authorId = inputRecipe.authorId;
            brewday.authorName = inputRecipe.authorName;
            brewday.batchSize = inputRecipe.batchSize;
            brewday.boilTime = inputRecipe.boilTime;
            brewday.recipeName = inputRecipe.name;
            brewday.mashSpargeType = inputRecipe.mashSpargeType;
            brewday.styleId = inputRecipe.styleId;

            dbConn.Brewdays.Add(brewday);
            dbConn.SaveChanges();

            //Copy list of ferms, hops, yeast, others,  and mash steps to corresponding Brewday tables for a "snapshot" of the recipe at time of brewing
            dbConn.Database.ExecuteSqlCommand("exec dbo.CreateBrewdayEntries " + inputRecipe.id + ", " + brewday.id);

        }

        public WholeBrewday(Brewday inputBrewday)  //If we're given a Brewday object, we'll simply compile the necessary data to display the brewday.
        {
            


        }



    }
}