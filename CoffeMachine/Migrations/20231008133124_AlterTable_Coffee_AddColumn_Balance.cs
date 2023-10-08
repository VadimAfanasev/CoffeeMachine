using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeMachine.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Coffee_AddColumn_Balance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeBalances");

            migrationBuilder.AddColumn<long>(
                name: "Balance",
                table: "Coffees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Coffees");

            migrationBuilder.CreateTable(
                name: "CoffeeBalances",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeBalances", x => x.Name);
                });
        }
    }
}
