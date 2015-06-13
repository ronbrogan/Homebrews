using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrewingSite.Models
{
    public partial class ViewHop
    {
        public int id { get; set; }
        public string name { get; set; }
        public double alphaAcid { get; set; }
        public Nullable<double> amount { get; set; }
        public string unit { get; set; }
        public Nullable<int> additionTime { get; set; }
        public Nullable<bool> isLeaf { get; set; }
    }
}