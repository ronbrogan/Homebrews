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
    
    public partial class Fermentable
    {
        public int id { get; set; }
        public string name { get; set; }
        public int ppg { get; set; }
        public Nullable<double> attenuation { get; set; }
        public Nullable<double> lovibond { get; set; }
        public Nullable<int> diastaticPower { get; set; }
        public string phType { get; set; }
    }
}
