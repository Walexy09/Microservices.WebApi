using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.Microservice.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Name", "Price", "StockQuantity" },
                values: new object[] { new Guid("25d1d4b0-1d0e-459e-9ae7-9b196a0a4e57"), "Category 1", "This is a solid soled shoe", "Solid Shoes", 10.99m, 100 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Name", "Price", "StockQuantity" },
                values: new object[] { new Guid("afc4468e-2a9d-4e0a-b208-5c71dc21d7ba"), "Category 2", "Black Gucci bag for ladies", "Black Bag", 19.99m, 200 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
