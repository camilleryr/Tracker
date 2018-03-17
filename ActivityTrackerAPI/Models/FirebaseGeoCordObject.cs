using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerAPI.Models
{
    public class FirebaseGeoCordObject
    {
        public string key { get; set; }
        public GeoLocation[] value { get; set; }

        public FirebaseGeoCordObject(string _key, GeoLocation[] _value)
        {
            key = _key;
            value = _value;
        }
    }
}
