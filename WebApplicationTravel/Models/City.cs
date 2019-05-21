﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebApplicationTravel.Models
{
    public class City
    {
        public int CityId { get; set; } 
        public string CityName { get; set; }
        public Country Country { get; set; }
        public int CountryId { get; set; }
        public double FlightPriceKey { get; set; }
        public double CarRentalPriceKey { get; set; }
        public Point Coordinate { get; set; }

    }
}