using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplicationTravel.Models
{
    public class MSGDContext: DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
    }
}