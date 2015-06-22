﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class brewappEntities : DbContext
    {
        public brewappEntities()
            : base("name=brewappEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Fermentable> Fermentables { get; set; }
        public virtual DbSet<Hop> Hops { get; set; }
        public virtual DbSet<Style> Styles { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<SRMtoRGB> SRMtoRGBs { get; set; }
        public virtual DbSet<Yeast> Yeasts { get; set; }
        public virtual DbSet<RecipeFermentable> RecipeFermentables { get; set; }
        public virtual DbSet<Brewday> Brewdays { get; set; }
        public virtual DbSet<BrewdayEquipmentProfile> BrewdayEquipmentProfiles { get; set; }
        public virtual DbSet<BrewdayFermentable> BrewdayFermentables { get; set; }
        public virtual DbSet<BrewdayFermentationProfile> BrewdayFermentationProfiles { get; set; }
        public virtual DbSet<BrewdayMashEntry> BrewdayMashEntries { get; set; }
        public virtual DbSet<RecipeHop> RecipeHops { get; set; }
        public virtual DbSet<RecipeOther> RecipeOthers { get; set; }
        public virtual DbSet<RecipeYeast> RecipeYeasts { get; set; }
        public virtual DbSet<RecipeEquipmentProfile> RecipeEquipmentProfiles { get; set; }
        public virtual DbSet<RecipeFermentationProfile> RecipeFermentationProfiles { get; set; }
        public virtual DbSet<RecipeMashEntry> RecipeMashEntries { get; set; }
        public virtual DbSet<BrewdayHop> BrewdayHops { get; set; }
        public virtual DbSet<BrewdayOther> BrewdayOthers { get; set; }
        public virtual DbSet<BrewdayYeast> BrewdayYeasts { get; set; }
        public virtual DbSet<BrewdayMeasurement> BrewdayMeasurements { get; set; }
    }
}
