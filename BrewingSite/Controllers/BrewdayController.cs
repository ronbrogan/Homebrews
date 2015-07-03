using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrewingSite.Models;

namespace BrewingSite.Controllers
{
    public class BrewdayController : Controller
    {
        brewappEntities dbConn = new brewappEntities();

        // GET: Brewday/x
        public ActionResult Index(string id="-1")
        {
            if (id == "-1")
                return HttpNotFound("Unable to lookup brewday. A brewday was not specified.");

            BrewdayViewmodel brewday = new BrewdayViewmodel(dbConn.Brewdays.Find(Convert.ToInt32(id)));
            return View(brewday);
        }

        //GET: Brewday/Create/x
        [Authorize]
        public ActionResult Create(string id = "-1")
        {
            if (id == "-1")
                return HttpNotFound("Unable to create brewday. Recipe ID was not specified.");
            BrewdayViewmodel newBrewday;

            try
            {
                Recipe inputRecipe = dbConn.Recipes.Find(Convert.ToInt32(id));
                newBrewday = new BrewdayViewmodel(inputRecipe);
            }
            catch (Exception ex)
            {
                return Redirect("/Dashboard/Index/" + ex.Message);
            }
            

            

            return Redirect("/Brewday/" + newBrewday.brewday.id);
        }


        public ActionResult ShowLogFermenterGravityDialog(string id = "-1")
        {
            BrewdayMeasurement measurement;

            measurement = dbConn.BrewdayMeasurements.Find(Convert.ToInt32(id));

            return PartialView("_LogFermenterGravity", measurement);
        }

        public ActionResult ShowLogFinalGravityDialog(string id = "-1")
        {
            BrewdayMeasurement measurement;

            measurement = dbConn.BrewdayMeasurements.Find(Convert.ToInt32(id));

            return PartialView("_LogFinalGravity", measurement);
        }

        public ActionResult ShowLogPreboilGravityDialog(string id = "-1")
        {
            BrewdayMeasurement measurement;

            measurement = dbConn.BrewdayMeasurements.Find(Convert.ToInt32(id));

            return PartialView("_LogPreboilGravity", measurement);
        }






        [HttpPost]
        public string logPreboilGravity(BrewdayMeasurement measure, string id = "-1")
        {
            if(id == "-1")
            {
                Response.StatusCode = 404;
                return "No Brewday specified";
            }

            Brewday brewday = dbConn.Brewdays.Find(Convert.ToInt32(id));

            BrewdayMeasurement measurement = (from measures in dbConn.BrewdayMeasurements where measures.brewdayId == brewday.id select measures).FirstOrDefault();

            double calcPoints = (double)(measurement.fermenterGravityCalc / brewday.batchSize);


            measurement.preboilGravityReal = measure.preboilGravityReal;
            measurement.preboilGravityVolume = measure.preboilGravityVolume;
            measurement.preboilGravityTimestamp = DateTime.Now;

            dbConn.BrewdayMeasurements.Attach(measurement);
            dbConn.Entry(measurement).State = System.Data.Entity.EntityState.Modified;
            dbConn.SaveChanges();

            Response.StatusCode = 200;

            return null;
        }

        [HttpPost]
        public string logFermenterGravity(BrewdayMeasurement measure,  string id = "-1")
        {
            if (id == "-1")
            {
                Response.StatusCode = 404;
                return "No Brewday specified";
            }

            try
            {
                Brewday brewday = dbConn.Brewdays.Find(Convert.ToInt32(id));

                BrewdayMeasurement measurement = (from measures in dbConn.BrewdayMeasurements where measures.brewdayId == brewday.id select measures).FirstOrDefault();

                //Get amount of points per gallon expected
                double calcPoints = (double)(((measurement.fermenterGravityCalc - 1) * 1000) * brewday.batchSize);

                //Calculate points per gallon of input data
                double realPoints = (double)(((measure.fermenterGravityReal - 1) * 1000) * measure.fermenterGravityVolume);

                measurement.fermenterGravityDeviation = ((realPoints - calcPoints) / calcPoints) * 100;
                measurement.fermenterGravityReal = measure.fermenterGravityReal;
                measurement.fermenterGravityVolume = measure.fermenterGravityVolume;
                measurement.fermenterGravityTimestamp = DateTime.Now;

                dbConn.BrewdayMeasurements.Attach(measurement);
                dbConn.Entry(measurement).State = System.Data.Entity.EntityState.Modified;
                dbConn.SaveChanges();

                Response.StatusCode = 200;

                return null;
            }
            catch (Exception ex)
            {
                Response.StatusCode = 501;
                return ex.Message;
            }
        }

        [HttpPost]
        public string logFinalGravity(BrewdayMeasurement measure, string id = "-1")
        {
            if (id == "-1")
            {
                Response.StatusCode = 404;
                return "No Brewday specified";
            }

            try
            {
                Brewday brewday = dbConn.Brewdays.Find(Convert.ToInt32(id));

                BrewdayMeasurement measurement = (from measures in dbConn.BrewdayMeasurements where measures.brewdayId == brewday.id select measures).FirstOrDefault();

                //Get amount of points per gallon expected
                double calcPoints = (double)(((measurement.finalGravityCalc - 1) * 1000) * brewday.batchSize);

                //Calculate points per gallon of input data
                double realPoints = (double)(((measure.finalGravityReal - 1) * 1000) * measure.finalGravityVolume);

                measurement.finalGravityDeviation = ((realPoints - calcPoints) / calcPoints) * 100;
                measurement.finalGravityReal = measure.finalGravityReal;
                measurement.finalGravityVolume = measure.finalGravityVolume;
                measurement.finalGravityTimestamp = DateTime.Now;

                dbConn.BrewdayMeasurements.Attach(measurement);
                dbConn.Entry(measurement).State = System.Data.Entity.EntityState.Modified;
                dbConn.SaveChanges();

                Response.StatusCode = 200;

                return null;
            }
            catch (Exception ex)
            {
                Response.StatusCode = 501;
                return ex.Message;
            }
        }
    }
}