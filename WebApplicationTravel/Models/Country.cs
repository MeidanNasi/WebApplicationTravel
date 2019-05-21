using System.Collections;
using System.Collections.Generic;

namespace WebApplicationTravel.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public ICollection<City> Cities { get; set; }

    }
}