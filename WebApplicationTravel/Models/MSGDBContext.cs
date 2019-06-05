using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplicationTravel.Models
{
    public class MSGDBContext: DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Connections> Connections { get; set; }
        public DbSet<Account> Accounts { get; set; }

    }
}