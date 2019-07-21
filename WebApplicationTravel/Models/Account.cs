using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationTravel.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password{ get; set; }
        public Boolean Admin { get; set; }
        public LinkedList<LinkedList<string>> Reservations = new LinkedList<LinkedList<string>>();

    }
}