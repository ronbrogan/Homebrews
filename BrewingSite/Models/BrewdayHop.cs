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
    
    public partial class BrewdayHop
    {
        public int id { get; set; }
        public Nullable<int> brewdayId { get; set; }
        public Nullable<int> ingredientId { get; set; }
        public Nullable<double> amount { get; set; }
        public string unit { get; set; }
        public Nullable<int> additionTime { get; set; }
        public Nullable<bool> isLeaf { get; set; }
    }
}
