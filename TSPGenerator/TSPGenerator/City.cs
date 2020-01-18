using System;
using System.Collections.Generic;
using System.Text;

namespace TSPGenerator
{
    class City
    {
        public String Stadium { get; set; }

        public String CityName { get; set; }

        public decimal Capacity { get; set; }

        public Double Longitude { get; set; }

        public Double Latitude { get; set; }

        public City(string stadium, string cityName, decimal capacity, double longitude, double latitude)
        {
            Stadium = stadium;
            CityName = cityName;
            Capacity = capacity;
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}
