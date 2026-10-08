using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChessPlatform.Migrations
{
    /// <inheritdoc />
    public partial class MinutesTimer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeControlMinutes",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeControlMinutes",
                table: "Tournaments");
        }
    }
}
