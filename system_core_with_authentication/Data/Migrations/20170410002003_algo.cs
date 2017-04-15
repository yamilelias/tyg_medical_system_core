using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace system_core_with_authentication.Data.Migrations
{
    public partial class algo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicament",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<int>(nullable: false),
                    Counter = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicament", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    SecondLastName = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Expiration = table.Column<DateTime>(nullable: false),
                    MedicamentId = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stock_Medicament_MedicamentId",
                        column: x => x.MedicamentId,
                        principalTable: "Medicament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Friday = table.Column<string>(nullable: true),
                    IdLocation = table.Column<int>(nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    Monday = table.Column<string>(nullable: true),
                    Saturday = table.Column<string>(nullable: true),
                    Sunday = table.Column<string>(nullable: true),
                    Thursday = table.Column<string>(nullable: true),
                    Tuesday = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    Wednesday = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationSchedule_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationSchedule_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    IdUser = table.Column<int>(nullable: false),
                    IdUserApproved = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    UserApprovedId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_User_UserApprovedId",
                        column: x => x.UserApprovedId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RepositionStock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdLocation = table.Column<int>(nullable: false),
                    IdRequest = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    RequestId = table.Column<int>(nullable: true),
                    Solved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepositionStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepositionStock_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RepositionStock_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RepositionStockDetailed",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentStock = table.Column<int>(nullable: false),
                    IdMedicament = table.Column<int>(nullable: false),
                    IdRepositionStock = table.Column<int>(nullable: false),
                    MedicamentId = table.Column<int>(nullable: true),
                    RepositionStockId = table.Column<int>(nullable: true),
                    RequestStock = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepositionStockDetailed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepositionStockDetailed_Medicament_MedicamentId",
                        column: x => x.MedicamentId,
                        principalTable: "Medicament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RepositionStockDetailed_RepositionStock_RepositionStockId",
                        column: x => x.RepositionStockId,
                        principalTable: "RepositionStock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationSchedule_LocationId",
                table: "LocationSchedule",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationSchedule_UserId",
                table: "LocationSchedule",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RepositionStock_LocationId",
                table: "RepositionStock",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RepositionStock_RequestId",
                table: "RepositionStock",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RepositionStockDetailed_MedicamentId",
                table: "RepositionStockDetailed",
                column: "MedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_RepositionStockDetailed_RepositionStockId",
                table: "RepositionStockDetailed",
                column: "RepositionStockId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserApprovedId",
                table: "Requests",
                column: "UserApprovedId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserId",
                table: "Requests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_MedicamentId",
                table: "Stock",
                column: "MedicamentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationSchedule");

            migrationBuilder.DropTable(
                name: "RepositionStockDetailed");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "RepositionStock");

            migrationBuilder.DropTable(
                name: "Medicament");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
