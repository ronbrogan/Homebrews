//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BrewingSite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Recipe
    {
        public int id { get; set; }
        public string name { get; set; }
        public string authorId { get; set; }
        public Nullable<int> styleId { get; set; }
        public Nullable<double> batchSize { get; set; }
        public string authorName { get; set; }
        public Nullable<double> boilTime { get; set; }
        public Nullable<int> recipeType { get; set; }
        public Nullable<int> equipmentProfile { get; set; }
        public string mashSpargeType { get; set; }
        public Nullable<int> fermentationProfileId { get; set; }
        public Nullable<bool> isPublicRead { get; set; }
        public string recipeSummary { get; set; }
    }
}
