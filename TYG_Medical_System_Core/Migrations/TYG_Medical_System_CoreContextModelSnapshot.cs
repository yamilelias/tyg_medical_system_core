using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TYG_Medical_System_Core.Models;

namespace TYG_Medical_System_Core.Migrations
{
    [DbContext(typeof(TYG_Medical_System_CoreContext))]
    partial class TYG_Medical_System_CoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TYG_Medical_System_Core.Models.MedicineItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<byte[]>("Image");

                    b.Property<string>("MaintenanceDate");

                    b.Property<string>("Note");

                    b.Property<string>("Serial");

                    b.HasKey("Id");

                    b.ToTable("MedicineItem");
                });
        }
    }
}
