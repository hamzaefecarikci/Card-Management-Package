using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cardholder.API.Migrations
{
    /// <inheritdoc />
    public partial class FirstDBInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CardHolders",
                columns: table => new
                {
                    CardholderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaswordHash = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardHolders", x => x.CardholderId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardholderId = table.Column<int>(type: "int", nullable: false),
                    CardNumber = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiryDate = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cvv = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardHolders");

            migrationBuilder.DropTable(
                name: "Cards");
        }
    }
}
