using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrewingSite.Models
{

    using System;
    using System.Collections.Generic;

    public partial class OtherIngredient
    {
        public int id { get; set; }
        public string amount { get; set; }
        public string item { get; set; }
        public string use { get; set; }
      
    }
}