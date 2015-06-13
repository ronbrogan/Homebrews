using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrewingSite.Models
{

    public partial class ViewFermentable
    {
        public string unit { get; set; }
        public string url { get; set; }
        public Nullable<double> amount { get; set; }
        public Nullable<bool> isMashed { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int ppg { get; set; }
        public Nullable<double> attenuation { get; set; }
        public Nullable<double> lovibond { get; set; }
        public Nullable<int> diastaticPower { get; set; }
        public string phType { get; set; }
    }

}