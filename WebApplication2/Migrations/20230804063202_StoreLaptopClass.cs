using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    public partial class StoreLaptopClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    StoreNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreetNameAndNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreNumber);
                });

            migrationBuilder.CreateTable(
                name: "StoresLaptops",
                columns: table => new
                {
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LaptopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoresLaptops", x => new { x.StoreId, x.LaptopId });
                    table.ForeignKey(
                        name: "FK_StoresLaptops_Laptops_LaptopId",
                        column: x => x.LaptopId,
                        principalTable: "Laptops",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoresLaptops_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoresLaptops_LaptopId",
                table: "StoresLaptops",
                column: "LaptopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoresLaptops");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
