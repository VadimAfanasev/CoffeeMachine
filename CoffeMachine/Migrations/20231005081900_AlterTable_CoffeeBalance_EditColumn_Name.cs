using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeMachine.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_CoffeeBalance_EditColumn_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoffeeName",
                table: "CoffeeBalances",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CoffeeBalances",
                newName: "CoffeeName");
        }
    }
}
