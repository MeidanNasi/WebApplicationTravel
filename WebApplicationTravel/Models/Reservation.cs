using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationTravel.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public string TheReservation { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string Departure { get; set; }
           
    }
}