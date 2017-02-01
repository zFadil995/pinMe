using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms.Maps;

namespace PinMe
{
    public static class Location
    {
        public static Xamarin.Forms.Maps.Position MoveToPosition = new Xamarin.Forms.Maps.Position(0, 0);
        public static int range = 5;
        public static int distance;
        public static double longitude;
        public static double latitude;
        public static  List<Diner> diners;
        public static Xamarin.Forms.Maps.Position CurrentPosition = new Xamarin.Forms.Maps.Position(0, 0);
        public static void Init()
        {
            var assembly = typeof(MapPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("PinMe.diners.txt");
            string dinerData = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                dinerData = reader.ReadToEnd();
            }
            diners = JsonConvert.DeserializeObject<List<Diner>>(dinerData);
            longitude = 0;
            latitude = 0;
            foreach (Diner diner in diners)
            {
                longitude += diner.gpsx;
                latitude += diner.gpsy;
            }
            longitude /= diners.Count;
            latitude /= diners.Count;

            InitDistance(latitude, longitude);
        }
        public static void InitDistance(double Latitude, double Longitude) {
            distance = 0;
            int temp;
            foreach (Diner diner in diners)
            {
                temp = Between(diner.gpsy, diner.gpsx, Latitude, Longitude);
                if (temp > distance)
                    distance = temp;
            }
            range = distance;
        }
        public static int Between(double FromLatitude, double FromLongtitude, double ToLatidude, double ToLongtitude)
        {
            double theta = FromLongtitude - ToLongtitude;
            double dist = Math.Sin(deg2rad(FromLatitude)) * Math.Sin(deg2rad(ToLatidude)) + Math.Cos(deg2rad(FromLatitude)) * Math.Cos(deg2rad(ToLatidude)) * Math.Cos(deg2rad(theta));
            dist = rad2deg(Math.Acos(dist)) * 60 * 1.1515 * 1609.344;
            return (int)dist / 1000;
        }
        public static string BetweenString(double FromLatitude, double FromLongtitude, double ToLatidude, double ToLongtitude)
        {
            double theta = FromLongtitude - ToLongtitude;
            double dist = Math.Sin(deg2rad(FromLatitude)) * Math.Sin(deg2rad(ToLatidude)) + Math.Cos(deg2rad(FromLatitude)) * Math.Cos(deg2rad(ToLatidude)) * Math.Cos(deg2rad(theta));
            dist = rad2deg(Math.Acos(dist)) * 60 * 1.1515 * 1609.344;
            if (dist < 1000)
                return (int)dist + " m";
            else
            {
                return (int) (dist/1000) + " km";
            }
        }
        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }
        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
    public class Diner
    {
        public string name { get; set; }
        public string address { get; set; }
        public double gpsx { get; set; }
        public double gpsy { get; set; }
    }
}
