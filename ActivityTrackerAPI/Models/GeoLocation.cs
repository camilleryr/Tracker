using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerAPI.Models
{
    public class GeoLocation
    {
        public Coords coords { get; set; }
        public decimal timestamp { get; set; }
    }
}
