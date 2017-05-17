using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using system_core_with_authentication.Data;

namespace system_core_with_authentication.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecondLastName");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Telephone");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserImage");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.LocationSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Friday");

                    b.Property<int>("IdLocation");

                    b.Property<int>("IdUser");

                    b.Property<int?>("LocationId");

                    b.Property<string>("Monday");

                    b.Property<string>("Saturday");

                    b.Property<string>("Sunday");

                    b.Property<string>("Thursday");

                    b.Property<string>("Tuesday");

                    b.Property<int?>("UserId");

                    b.Property<string>("Wednesday");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("LocationSchedule");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.Medicament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Content");

                    b.Property<int>("Counter");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("MedicamentImage");

                    b.Property<double>("Price");

                    b.Property<int>("Priority");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Medicament");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.RepositionStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LocationId");

                    b.Property<int?>("RequestId");

                    b.Property<bool>("Solved");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("RequestId");

                    b.ToTable("RepositionStock");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.RepositionStockDetailed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CurrentStock");

                    b.Property<int?>("MedicamentId");

                    b.Property<int?>("RepositionStockId");

                    b.Property<int>("RequestStock");

                    b.HasKey("Id");

                    b.HasIndex("MedicamentId");

                    b.HasIndex("RepositionStockId");

                    b.ToTable("RepositionStockDetailed");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Note");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Expiration");

                    b.Property<int>("MedicamentId");

                    b.Property<int>("Total");

                    b.HasKey("Id");

                    b.HasIndex("MedicamentId");

                    b.ToTable("Stock");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.TyGUser", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("LastName");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("SecondLastName");

                    b.Property<string>("Telephone");

                    b.HasKey("ID");

                    b.ToTable("TyGUser");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Address");

                    b.Property<string>("Email");

                    b.Property<DateTime>("EntryDate");

                    b.Property<byte[]>("Image");

                    b.Property<string>("LastName");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("SecondLastName");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("system_core_with_authentication.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("system_core_with_authentication.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("system_core_with_authentication.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("system_core_with_authentication.Models.LocationSchedule", b =>
                {
                    b.HasOne("system_core_with_authentication.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("system_core_with_authentication.Models.User", "User")
                        .WithMany("ls")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.RepositionStock", b =>
                {
                    b.HasOne("system_core_with_authentication.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("system_core_with_authentication.Models.Request", "Request")
                        .WithMany()
                        .HasForeignKey("RequestId");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.RepositionStockDetailed", b =>
                {
                    b.HasOne("system_core_with_authentication.Models.Medicament", "Medicament")
                        .WithMany()
                        .HasForeignKey("MedicamentId");

                    b.HasOne("system_core_with_authentication.Models.RepositionStock", "RepositionStock")
                        .WithMany("rpd")
                        .HasForeignKey("RepositionStockId");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.Request", b =>
                {
                    b.HasOne("system_core_with_authentication.Models.User", "User")
                        .WithMany("Request")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("system_core_with_authentication.Models.Stock", b =>
                {
                    b.HasOne("system_core_with_authentication.Models.Medicament", "Medicament")
                        .WithMany("Stocks")
                        .HasForeignKey("MedicamentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
