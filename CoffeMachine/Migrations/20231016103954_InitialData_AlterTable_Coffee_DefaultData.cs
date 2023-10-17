using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoffeeMachine.Migrations
{
    /// <inheritdoc />
    public partial class InitialData_AlterTable_Coffee_DefaultData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoffeesDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Balance = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeesDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoneyInMachinesDb",
                columns: table => new
                {
                    Nominal = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Count = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyInMachinesDb", x => x.Nominal);
                });

            migrationBuilder.InsertData(
                table: "CoffeesDb",
                columns: new[] { "Balance", "Name", "Price" },
                values: new object[,]
                {
                    { 0u, "Cappuccino", 600u },
                    { 0u, "Latte", 850u },
                    { 0u, "Americano", 900u }
                });

            migrationBuilder.InsertData(
                table: "MoneyInMachinesDb",
                columns: new[] { "Nominal", "Count" },
                values: new object[,]
                {
                    { 50u, 10u },
                    { 100u, 10u },
                    { 200u, 10u },
                    { 500u, 10u },
                    { 1000u, 10u },
                    { 2000u, 10u },
                    { 5000u, 10u }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeesDb");

            migrationBuilder.DropTable(
                name: "MoneyInMachinesDb");
        }
    }
}
