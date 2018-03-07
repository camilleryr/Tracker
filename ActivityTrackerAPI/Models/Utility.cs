using Geolocation;
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
            return distance /
                (double)(geoLocations[0].timestamp - (geoLocations[geoLocations.Length - 1].timestamp) / 3600000);
        }

        public static string GetFirebaseLocation(byte[] responseByteArray)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(FirebaseResponse));
            MemoryStream stream1 = new MemoryStream(responseByteArray);
            stream1.Position = 0;
            FirebaseResponse firebaseResponse = (FirebaseResponse)ser.ReadObject(stream1);
            return firebaseResponse.name;
        }
    }
}
