using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYG_Medical_System_Core.Models;
using Microsoft.EntityFrameworkCore;


namespace TYG_Medical_System_Core.DAL
{
    public class DBContext : DbContext
    {

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}
