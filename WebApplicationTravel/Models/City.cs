using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [InverseProperty("SourceCity")]
        public ICollection<Connections> SourceCityConnections { get; set; }
        [InverseProperty("DestCity")]
        public ICollection<Connections> DestCityConnections { get; set; }

    }
}