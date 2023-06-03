using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TravellersChoice.Models
{
    public class TravellerContext : ApplicationDbContext
    {

        

        public TravellerContext()
        {

            //Database.SetInitializer<TravellerContext>(new DropCreateDatabaseIfModelChanges<TravellerContext>());
        }
         

        public DbSet<Place> Place { get; set; }


        public DbSet<User> Profile { get; set; }

        public DbSet<Image> Image { get; set; }

        public DbSet<Review> Review { get; set; }


    }
}