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
        public virtual DbSet<EquipmentProfile> EquipmentProfiles { get; set; }
        public virtual DbSet<MashEntry> MashEntries { get; set; }
        public virtual DbSet<FermentationProfile> FermentationProfiles { get; set; }
    }
}
