using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BrewingSite.Models
{
    public class BrewdayViewmodel
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

        public BrewdayMeasurement measurement = new BrewdayMeasurement();

        public Style recipeStyle;

        public int recipeType;
        public string recipeTypeDisplay;

        public BrewdayViewmodel(Recipe inputRecipe)  //If passed a recipe object, we're to create a new brewday entry in the database from that recipe object then compile data to display
        {
            //Ensure that we have the necessary data to construct a Brewday, if not throw an error.
            if (inputRecipe.equipmentProfile == null || inputRecipe.fermentationProfileId == null || inputRecipe.batchSize == null || inputRecipe.boilTime == null)
                throw new Exception("1");

            brewday = new Brewday();
            //Copy relevant data from inputRecipe to brewday object

            brewday.authorId = inputRecipe.authorId;
            brewday.authorName = inputRecipe.authorName;
            brewday.batchSize = inputRecipe.batchSize;
            brewday.boilTime = inputRecipe.boilTime;
            brewday.recipeName = inputRecipe.name;
            brewday.mashSpargeType = inputRecipe.mashSpargeType;
            brewday.styleId = inputRecipe.styleId;
            brewday.originalRecipeId = inputRecipe.id;
            brewday.timestamp = DateTime.Now;

            dbConn.Brewdays.Add(brewday);
            dbConn.SaveChanges();

            SqlParameter returnValue = new SqlParameter()
            {
                ParameterName = "@Return",
                SqlDbType = SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            if((from yeast in dbConn.RecipeYeasts where yeast.recipeId == inputRecipe.id select yeast).Count() == 0)
            {
                dbConn.Brewdays.Remove(brewday);
                dbConn.SaveChanges();
                throw new Exception("4");
            }

            if ((from ferms in dbConn.RecipeFermentables where ferms.recipeId == inputRecipe.id select ferms).Count() == 0)
            {
                dbConn.Brewdays.Remove(brewday);
                dbConn.SaveChanges();
                throw new Exception("6");
            }

            if ((from hop in dbConn.RecipeHops where hop.recipeId == inputRecipe.id select hop).Count() == 0)
            {
                dbConn.Brewdays.Remove(brewday);
                dbConn.SaveChanges();
                throw new Exception("7");
            }




            //Copy list of ferms, hops, yeast, others,  and mash steps to corresponding Brewday tables for a "snapshot" of the recipe at time of brewing
            dbConn.Database.ExecuteSqlCommand("exec @Return = dbo.CreateBrewdayEntries " + inputRecipe.id + ", " + brewday.id, returnValue);

            if ((int)returnValue.Value != 0)
            {
                dbConn.Brewdays.Remove(brewday);
                throw new Exception("2");
            }
                

            //Need to calculate OG, FG, ABV, IBU, SRM when brewday is created and store in measurements table
            double originalGravityCalc, finalGravityCalc, abvCalc, ibuCalc, srmCalc;


            //Get data that was copied to make calculations with
            yeasts = (from yeast in dbConn.BrewdayYeasts
                      join ingredients in dbConn.Yeasts on yeast.ingredientId equals ingredients.id
                      where yeast.brewdayId == brewday.id
                      orderby ingredients.Attenuation descending
                      select ingredients).ToList<Yeast>();


            fermentables = (from ferms in dbConn.BrewdayFermentables
                            join ingredients in dbConn.Fermentables on ferms.ingredientId equals ingredients.id
                            where ferms.brewdayId == brewday.id
                            select new ViewFermentable()
                            {
                                id = ferms.id,
                                name = ingredients.name,
                                amount = ferms.amount,
                                unit = ferms.unit,
                                lovibond = ingredients.lovibond,
                                ppg = ingredients.ppg,
                                isMashed = ferms.isMashed
                            }).ToList<ViewFermentable>();

            

            hops = (from hop in dbConn.BrewdayHops
                    join ingredients in dbConn.Hops on hop.ingredientId equals ingredients.id
                    where hop.brewdayId == brewday.id
                    select new ViewHop()
                    {
                        id = hop.id,
                        name = ingredients.name,
                        amount = hop.amount,
                        unit = hop.unit,
                        additionTime = hop.additionTime
                    }).ToList<ViewHop>();

            //Begin calculations
            BrewdayMeasurement measurements = new BrewdayMeasurement();

            double extractionEfficiency;
            double totalGravityPoints = 0;
            double fermenterVolume = (double)brewday.batchSize;
            double yeastAttenuation = (double)yeasts.FirstOrDefault().Attenuation;


            if (equipment.mashEfficiency != null)
                extractionEfficiency = (double)equipment.mashEfficiency;
            else
                extractionEfficiency = 75;

            foreach(var ferm in fermentables)
            {
                totalGravityPoints += ((double)ferm.amount * (double)ferm.ppg * Convert.ToInt32(ferm.isMashed));
            }

            double totalExtractedSugarPoints = (totalGravityPoints / fermenterVolume) * (extractionEfficiency / 100);

            originalGravityCalc = 1 + (totalExtractedSugarPoints / 1000);

            finalGravityCalc = 1 + (totalExtractedSugarPoints * ((100 - yeastAttenuation) / 100)) / 1000;

            abvCalc = (originalGravityCalc - finalGravityCalc) * 131.25;


            double bignessFactor = 1.65 * Math.Pow(0.000125, (originalGravityCalc - 1));
            ibuCalc = 0;

            foreach( var hop in hops)
            {
                double boilTimeFactor = (1 - Math.Pow(Math.E, (-0.04 * (double)hop.additionTime))) / 4.15;

                double utilization = bignessFactor * boilTimeFactor;

                ibuCalc += ((double)hop.amount * (double)hop.alphaAcid) * 75 * utilization / (double)brewday.batchSize;
            }

            double totalLovibondPoints = 0;
            double maltColorUnits;

            foreach(var ferm in fermentables)
            {
                totalLovibondPoints += ((double)ferm.amount * (double)ferm.lovibond);
            }

            maltColorUnits = totalLovibondPoints / (double)brewday.batchSize;

            srmCalc = 1.4922 * Math.Pow(maltColorUnits, 0.6859);

            measurements.fermenterGravityCalc = originalGravityCalc;
            measurements.finalGravityCalc = finalGravityCalc;
            measurements.ibuCalc = ibuCalc;
            measurements.srmCalc = srmCalc;
            measurements.abvCalc = abvCalc;
            measurements.brewdayId = brewday.id;

            dbConn.BrewdayMeasurements.Add(measurements);
            dbConn.SaveChanges();

        }

        public BrewdayViewmodel(Brewday inputBrewday)  //If we're given a Brewday object, we'll simply compile the necessary data to display the brewday.
        {

            brewday = inputBrewday;

            recipeStyle = dbConn.Styles.Find(brewday.styleId);

            measurement = (from measure in dbConn.BrewdayMeasurements where measure.brewdayId == brewday.id select measure).FirstOrDefault();

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
                    orderby hop.additionTime descending
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