﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationTravel.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public Account Account { get; set; }
        public int AccountId { get; set; }
        public IList<Connections> Connections { get; set; }
    }
}