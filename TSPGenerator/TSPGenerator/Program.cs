using System;
using System.Collections.Generic;
using System.IO;

namespace TSPGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<City> cities = ReadFromCSV("wspol.csv");
            CreateMatrix(cities, "matrix.txt");
        }

        private static List<City> ReadFromCSV(string path)
        {
            StreamReader sr = new StreamReader(path);
            List<City> cities = new List<City>();
            String line;
            Boolean first = true;
            while ((line = sr.ReadLine()) != null)
            {
                if (!first)
                {
                    String[] lineArray = line.Split(';');
                    String std = lineArray[0];
                    String nm = lineArray[1];
                    Decimal cap = decimal.Parse(lineArray[2]);
                    String[] coords = lineArray[3].Split(',');
                    double coord1 = double.Parse(coords[0].Trim().Replace('.', ','));
                    double coord2 = double.Parse(coords[1].Trim().Replace('.', ','));
                    cities.Add(new City(std, nm, cap, coord2, coord1));
                }
                else
                {
                    first = false;
                }
            }

            return cities;
        }

        private static void CreateMatrix(List<City> cities,string OutputPatch)
        {
            StreamWriter sw = new StreamWriter(OutputPatch);
            sw.WriteLine(cities.Count);
            for (int i = 0; i < cities.Count; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    sw.Write(Distance(cities[i].Latitude, cities[i].Longitude, cities[j].Latitude, cities[j].Longitude) + " ");
                }
                sw.WriteLine();
            }
            sw.Close();
        }

        private static double Distance(double lat1, double lon1, double lat2, double lon2) // Haversine formula
        {
            if(lat1 == lat2 && lon1 == lon2)
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(Deg2rad(lat1)) * Math.Sin(Deg2rad(lat2)) + Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) * Math.Cos(Deg2rad(theta));
                dist = Math.Acos(dist);
                dist = Rad2deg(dist);
                dist = dist * 60 * 1.1515;
                dist = dist * 1.609344;
                dist = Math.Round(dist,0);
                return dist;
            }
        }

        private static double Deg2rad(double deg)
        {
            return deg * Math.PI / 180.0;
        }

        private static double Rad2deg(double rad)
        {
            return rad / Math.PI * 180.0;
        }
    }
}