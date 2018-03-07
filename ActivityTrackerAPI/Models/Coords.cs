using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerAPI.Models
{
    public class Coords
    {
        public decimal accuracy { get; set; }
        public decimal altitude { get; set; }
        public decimal altitudeAccuracy { get; set; }
        public decimal heading { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public decimal speed { get; set; }

    }
}
