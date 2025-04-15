using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechPulse.Migrations
{
    /// <inheritdoc />
    public partial class CompleteOrderOverhaul : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CartData",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartData",
                table: "Orders");
        }
    }
}
