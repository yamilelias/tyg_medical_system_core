using TYG_Medical_System_Core.Models;
using Microsoft.EntityFrameworkCore;

namespace TYG_Medical_System_Core.DAL
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicament>().ToTable("Medicament");
            modelBuilder.Entity<Stock>().ToTable("Stock");
        }
    }
}
