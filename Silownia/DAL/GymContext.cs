using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Silownia.DAL
{
    public class GymContext : DbContext
    {
        public GymContext() : base("DefaultConnection")
        {
        }
        public DbSet<Activites> Activites { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Order_Pass> Order_Pass { get; set; }
        public DbSet<Pass> Pass { get; set; }
        public DbSet<Photos> Photos { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<TrainingRoom> TrainingRoom { get; set; }
        
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Activites_Profile> Activites_Profile { get; set; }
        public DbSet<Dish> Dish { get; set; }
        public DbSet<Dish_Profile> Dish_Profile { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //    modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}