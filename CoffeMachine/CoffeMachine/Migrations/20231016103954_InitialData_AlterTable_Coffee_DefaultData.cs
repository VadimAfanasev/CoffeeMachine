using CoffeeMachine.Common;
using CoffeeMachine.Common.Constants;
using CoffeeMachine.Common.Enums;
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
                    { 0u, CoffeeNames.CAPPUCCINO, 600u },
                    { 0u, CoffeeNames.LATTE, 850u },
                    { 0u, CoffeeNames.AMERICANO, 900u }
                });

            migrationBuilder.InsertData(
                table: "MoneyInMachinesDb",
                columns: new[] { "Nominal", "Count" },
                values: new object[,]
                {
                    { (uint)InputAdminBanknotesEnums.Fifty, 10u },
                    { (uint)InputAdminBanknotesEnums.OneHundred, 10u },
                    { (uint)InputAdminBanknotesEnums.TwoHundred, 10u },
                    { (uint)InputAdminBanknotesEnums.FiveHundred, 10u },
                    { (uint)InputAdminBanknotesEnums.OneThousand, 10u },
                    { (uint)InputAdminBanknotesEnums.TwoThousand, 10u },
                    { (uint)InputAdminBanknotesEnums.FiveThousand, 10u }
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
