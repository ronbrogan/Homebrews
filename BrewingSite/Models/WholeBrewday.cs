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
        public List<BrewdayOther> others = new List<BrewdayOther>();

        public List<BrewdayMashEntry> mashEntries = new List<BrewdayMashEntry>();

        public BrewdayEquipmentProfile equipment = new BrewdayEquipmentProfile();
        public BrewdayFermentationProfile fermentation = new BrewdayFermentationProfile();

        public Style recipeStyle;

        public int recipeType;
        public string recipeTypeDisplay;

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


            //Need to calculate OG, FG, ABV, IBU, SRM when brewday is created and store in measurements table

        }

        public WholeBrewday(Brewday inputBrewday)  //If we're given a Brewday object, we'll simply compile the necessary data to display the brewday.
        {

            brewday = inputBrewday;

            recipeStyle = dbConn.Styles.Find(brewday.styleId);


            equipment = (from equip in dbConn.BrewdayEquipmentProfiles where equip.brewdayId == brewday.id select equip).FirstOrDefault();

            fermentation = (from ferm in dbConn.BrewdayFermentationProfiles where ferm.brewdayId == brewday.id select ferm).FirstOrDefault();

            fermentables = (from ferms in dbConn.BrewdayFermentables 
                            join ingredients in dbConn.Fermentables on ferms.ingredientId equals ingredients.id 
                            where ferms.brewdayId == brewday.id 
                            select new ViewFermentable()
                            { 
                                id=ferms.id, 
                                name=ingredients.name, 
                                amount=ferms.amount, 
                                unit=ferms.unit 
                            }).ToList<ViewFermentable>();

            hops = (from hop in dbConn.BrewdayHops
                    join ingredients in dbConn.Hops on hop.ingredientId equals ingredients.id
                    where hop.brewdayId == brewday.id
                    select new ViewHop() 
                    { 
                        id=hop.id,
                        name=ingredients.name,
                        amount=hop.amount,
                        unit=hop.unit,
                        additionTime=hop.additionTime
                    }).ToList<ViewHop>();

            mashEntries = (from entry in dbConn.BrewdayMashEntries where entry.brewdayId == brewday.id select entry).ToList<BrewdayMashEntry>();

            yeasts = (from yeast in dbConn.BrewdayYeasts 
                      join ingredients in dbConn.Yeasts on yeast.ingredientId equals ingredients.id 
                      where yeast.brewdayId == brewday.id 
                      select ingredients).ToList<Yeast>();


            others = (from other in dbConn.BrewdayOthers where other.brewdayId == brewday.id select other).ToList<BrewdayOther>();

        }



    }
}