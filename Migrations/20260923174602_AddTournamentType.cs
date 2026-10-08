using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChessPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddTournamentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TournamentPlayers_TournamentId",
                table: "TournamentPlayers");

            migrationBuilder.RenameColumn(
                name: "isFinished",
                table: "Matches",
                newName: "IsFinished");

            migrationBuilder.AddColumn<string>(
                name: "TournamentType",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Player2Id",
                table: "Matches",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Player1Id",
                table: "Matches",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPlayers_TournamentId_PlayerId",
                table: "TournamentPlayers",
                columns: new[] { "TournamentId", "PlayerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player1Id",
                table: "Matches",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player2Id",
                table: "Matches",
                column: "Player2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_Player1Id",
                table: "Matches",
                column: "Player1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_Player2Id",
                table: "Matches",
                column: "Player2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_Player1Id",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_Player2Id",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_TournamentPlayers_TournamentId_PlayerId",
                table: "TournamentPlayers");

            migrationBuilder.DropIndex(
                name: "IX_Matches_Player1Id",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_Player2Id",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TournamentType",
                table: "Tournaments");

            migrationBuilder.RenameColumn(
                name: "IsFinished",
                table: "Matches",
                newName: "isFinished");

            migrationBuilder.AlterColumn<string>(
                name: "Player2Id",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Player1Id",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPlayers_TournamentId",
                table: "TournamentPlayers",
                column: "TournamentId");
        }
    }
}
