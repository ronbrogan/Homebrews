using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrewingSite.Models
{
    public partial class RecipeFermentationProfile
    {
        public int totalDays;
        public void absorbDetails(RecipeFermentationProfile source)
        {
            primaryDays = source.primaryDays;
            primaryNote = source.primaryNote;
            primaryTemp = source.primaryTemp;

            secondaryDays = source.secondaryDays;
            secondaryNote = source.secondaryNote;
            secondaryTemp = source.secondaryTemp;

            kegDays = source.kegDays;
            kegNote = source.kegNote;
            kegTemp = source.kegTemp;

            bottleDays = source.bottleDays;
            bottleNote = source.bottleNote;
            bottleTemp = source.bottleTemp;
        }

        public int totalFermentationDays()
        {
            try
            {
                totalDays = (int)primaryDays + (int)secondaryDays + (int)kegDays + (int)bottleDays;

                return totalDays;
            }
            catch
            {
                Console.WriteLine("Failed to sum days");
                return 0;
            }

        }

        public bool hasPrimary()
        {
            return (primaryDays > 0);
        }

        public bool hasSecondary()
        {
            return (secondaryDays > 0);
        }

        public bool hasKeg()
        {
            return (kegDays > 0);
        }

        public bool hasBottle()
        {
            return (bottleDays > 0);
        }

        public string primaryWidth()
        {
            return (((double)primaryDays / totalFermentationDays())*100).ToString() + "%";
        }

        public string secondaryWidth()
        {
            return (((double)secondaryDays / totalFermentationDays())*100).ToString() + "%";
        }

        public string kegWidth()
        {
            return (((double)kegDays / totalFermentationDays())*100).ToString() + "%";
        }

        public string bottleWidth()
        {
            return (((double)bottleDays / totalFermentationDays())*100).ToString() + "%";
        }

        
    }
}