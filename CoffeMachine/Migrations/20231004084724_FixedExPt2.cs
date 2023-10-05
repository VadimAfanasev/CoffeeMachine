using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeMachine.Migrations
{
    /// <inheritdoc />
    public partial class FixedExPt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CoffeeBalances",
                newName: "CoffeeName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoffeeName",
                table: "CoffeeBalances",
                newName: "Name");
        }
    }
}
