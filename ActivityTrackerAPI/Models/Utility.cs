using Geolocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace ActivityTrackerAPI.Models
{
    public class Utility
    {
        public static double GetDistance(GeoLocation[] geoLocations)
        {
            double distance = 0;

            for (int i = 0; i < geoLocations.Length - 2; i++)
            {
                Coordinate origin = new Coordinate();
                origin.Latitude = (double)geoLocations[i].coords.latitude;
                origin.Longitude = (double)geoLocations[i].coords.longitude;

                Coordinate destination = new Coordinate();
                destination.Latitude = (double)geoLocations[i + 1].coords.latitude;
                destination.Longitude = (double)geoLocations[i + 1].coords.longitude;

                distance += GeoCalculator.GetDistance(origin, destination, 10);
            }
            return distance;
        }

        public static double GetPace(GeoLocation[] geoLocations, double distance)
        {
            double time1 = (double)geoLocations[0].timestamp;
            double time2 = (double)(geoLocations[geoLocations.Length - 1].timestamp);
            double time3 = time1 - time2;
            double time = time3 / 3600000;
            return distance / time;
                
        }

        public static string GetFirebaseLocation(byte[] responseByteArray)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(FirebaseResponse));
            MemoryStream stream1 = new MemoryStream(responseByteArray);
            stream1.Position = 0;
            FirebaseResponse firebaseResponse = (FirebaseResponse)ser.ReadObject(stream1);
            return firebaseResponse.name;
        }

        public static List<FirebaseGeoCordObject> ConvertFirebaseResponse(string responseString)
        {
            dynamic data = JsonConvert.DeserializeObject<dynamic>(responseString);
            var list = new List<FirebaseGeoCordObject>();
            foreach (var itemDynamic in data)
            {
                string key = ((JProperty)itemDynamic).Name;
                GeoLocation[] value = JsonConvert.DeserializeObject<GeoLocation[]>(((JProperty)itemDynamic).Value.ToString());
                list.Add(new FirebaseGeoCordObject(key, value));
            }

            return list;
        }
    }
}
