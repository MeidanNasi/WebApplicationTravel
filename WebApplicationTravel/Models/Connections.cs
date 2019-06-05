using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplicationTravel.Models
{
    public class Connections
    {
        public int ConnectionsId { get; set; }

        [ForeignKey("SourceCity")]
        public int? SourceCityId { get; set; }
        public virtual City SourceCity { get; set; }

        [ForeignKey("DestCity")]
        public int? DestCityId { get; set; }
        public virtual City DestCity { get; set; }
        public double? FlightDuration { get; set; }
        public double? CarDuration { get; set; }
        public double? FlightPrice { get; set; }
        public double? CarPrice { get; set; }
        public Boolean CarAvailabilty { get; set; }
        public Boolean FlightAvailabilty { get; set; }
    }
}