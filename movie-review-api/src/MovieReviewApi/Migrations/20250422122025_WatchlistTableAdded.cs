using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApi.Migrations
{
    /// <inheritdoc />
    public partial class WatchlistTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WatchlistId",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Watchlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchlists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_WatchlistId",
                table: "Movies",
                column: "WatchlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Watchlists_WatchlistId",
                table: "Movies",
                column: "WatchlistId",
                principalTable: "Watchlists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Watchlists_WatchlistId",
                table: "Movies");

            migrationBuilder.DropTable(
                name: "Watchlists");

            migrationBuilder.DropIndex(
                name: "IX_Movies_WatchlistId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "WatchlistId",
                table: "Movies");
        }
    }
}
