using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class WebApplication2DbContext: DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Laptop>()
            .HasKey(l => l.Number); // Replace 'Id' with the actual property that should be the primary key

            modelBuilder.Entity<Laptop>()
            .Property(l => l.Price)
            .HasColumnType("decimal(18, 2)"); // Adjust the precision and scale as needed

            modelBuilder.Entity<Store>()
            .HasKey(s => s.StoreNumber); // Replace 'Id' with the actual property that should be the primary key

            modelBuilder.Entity<StoreLaptop>().HasKey(sl => new { sl.StoreId, sl.LaptopId }); // define 2 foreign keys as primary key

        }


        public WebApplication2DbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Laptop> Laptops { get; set; } = null!;
        public DbSet<Store> Stores { get; set; } = null!;
        public DbSet<StoreLaptop> StoresLaptops { get; set; } = null!;

    }
}
