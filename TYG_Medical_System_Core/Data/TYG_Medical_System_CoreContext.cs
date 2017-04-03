using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TYG_Medical_System_Core.Models
{
    public class TYG_Medical_System_CoreContext : DbContext
    {
        public TYG_Medical_System_CoreContext (DbContextOptions<TYG_Medical_System_CoreContext> options)
            : base(options)
        {
        }

        public DbSet<TYG_Medical_System_Core.Models.MedicineItem> MedicineItem { get; set; }
    }
}
