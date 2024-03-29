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
    
    public partial class BrewdayMeasurement
    {
        public int id { get; set; }
        public Nullable<int> brewdayId { get; set; }
        public Nullable<double> fermenterGravityCalc { get; set; }
        public Nullable<double> fermenterGravityReal { get; set; }
        public Nullable<double> fermenterGravityVolume { get; set; }
        public Nullable<System.DateTime> fermenterGravityTimestamp { get; set; }
        public Nullable<double> finalGravityCalc { get; set; }
        public Nullable<double> finalGravityReal { get; set; }
        public Nullable<double> finalGravityVolume { get; set; }
        public Nullable<System.DateTime> finalGravityTimestamp { get; set; }
        public Nullable<double> preboilGravityVolume { get; set; }
        public Nullable<double> preboilGravityReal { get; set; }
        public Nullable<System.DateTime> preboilGravityTimestamp { get; set; }
        public Nullable<double> pitchTemperature { get; set; }
        public Nullable<System.DateTime> pitchTimestamp { get; set; }
        public Nullable<double> boiloffVolume { get; set; }
        public Nullable<double> abvCalc { get; set; }
        public Nullable<double> abvReal { get; set; }
        public Nullable<double> srmCalc { get; set; }
        public Nullable<double> ibuCalc { get; set; }
        public Nullable<double> fermenterGravityDeviation { get; set; }
        public Nullable<double> finalGravityDeviation { get; set; }
        public Nullable<double> preboilGravityDeviation { get; set; }
        public string srmRgb { get; set; }
    }
}
