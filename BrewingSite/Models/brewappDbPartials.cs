using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BrewingSite.Models
{

    [MetadataType(typeof(RecipeFermentable))]
    public partial class RecipeFermentableTemp
    {
        public class RecipeFermentable
        {
            [Required]
            public Nullable<int> ingredientId { get; set; }

            [Required]
            public Nullable<double> amount { get; set; }
        }
    }
    
}