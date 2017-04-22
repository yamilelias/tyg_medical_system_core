using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using system_core_with_authentication.Models;

namespace system_core_with_authentication.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<TyGUser> TyGUser { get; set; }
        public DbSet<IdentityRole> identityRole { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<RepositionStock> RepositionStocks { get; set; }
        public DbSet<RepositionStockDetailed> RepositionStockDetailed { get; set; }
        public DbSet<Request> Requests { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationSchedule> LocationSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            modelBuilder.Entity<Medicament>().ToTable("Medicament");
            modelBuilder.Entity<Stock>().ToTable("Stock");
            modelBuilder.Entity<RepositionStock>().ToTable("RepositionStock");
            modelBuilder.Entity<RepositionStockDetailed>().ToTable("RepositionStockDetailed");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<LocationSchedule>().ToTable("LocationSchedule");
        }

        public DbSet<system_core_with_authentication.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}
