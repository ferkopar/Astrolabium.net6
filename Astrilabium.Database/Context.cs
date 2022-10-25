using Astrolabium.Database.Modell;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Astrolabium.Database
{
    public class Context : DbContext
    {
        private static bool _created = false;
        public Context()
        {
            if (!_created)
            {
                _created = true;
               // Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }
       protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(@"Data Source=Data\Astrolabum.db");
        }
        public DbSet<Place> Places { get; set; }
        public DbSet<Person> Persons { get; set; }

    }
}
