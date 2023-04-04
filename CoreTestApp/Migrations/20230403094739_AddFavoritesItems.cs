using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreTestApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoritesItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteAnuntItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImobilId = table.Column<int>(type: "int", nullable: false),
                    FavoritesID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteAnuntItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteAnuntItems_Imobils_ImobilId",
                        column: x => x.ImobilId,
                        principalTable: "Imobils",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteAnuntItems_ImobilId",
                table: "FavoriteAnuntItems",
                column: "ImobilId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteAnuntItems");
        }
    }
}
