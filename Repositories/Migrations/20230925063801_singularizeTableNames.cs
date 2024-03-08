using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imobiliare.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class singularizeTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteAnuntItems_Imobile_ImobilId",
                table: "FavoriteAnuntItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Imobile_AspNetUsers_UserId",
                table: "Imobile");

            migrationBuilder.DropForeignKey(
                name: "FK_Imobile_Cartiere_CartierId",
                table: "Imobile");

            migrationBuilder.DropForeignKey(
                name: "FK_Imobile_Judete_JudetId",
                table: "Imobile");

            migrationBuilder.DropForeignKey(
                name: "FK_Imobile_Orase_OrasId",
                table: "Imobile");

            migrationBuilder.DropForeignKey(
                name: "FK_Mesaje_Imobile_ImobilId",
                table: "Mesaje");

            migrationBuilder.DropForeignKey(
                name: "FK_Orase_Judete_JudetId",
                table: "Orase");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Imobile_ImobilId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Imobile_ImobilId",
                table: "ShoppingCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Judete",
                table: "Judete");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Imobile",
                table: "Imobile");

            migrationBuilder.RenameTable(
                name: "Judete",
                newName: "Judet");

            migrationBuilder.RenameTable(
                name: "Imobile",
                newName: "Imobil");

            migrationBuilder.RenameIndex(
                name: "IX_Imobile_UserId",
                table: "Imobil",
                newName: "IX_Imobil_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Imobile_OrasId",
                table: "Imobil",
                newName: "IX_Imobil_OrasId");

            migrationBuilder.RenameIndex(
                name: "IX_Imobile_JudetId",
                table: "Imobil",
                newName: "IX_Imobil_JudetId");

            migrationBuilder.RenameIndex(
                name: "IX_Imobile_CartierId",
                table: "Imobil",
                newName: "IX_Imobil_CartierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Judet",
                table: "Judet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Imobil",
                table: "Imobil",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteAnuntItems_Imobil_ImobilId",
                table: "FavoriteAnuntItems",
                column: "ImobilId",
                principalTable: "Imobil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imobil_AspNetUsers_UserId",
                table: "Imobil",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imobil_Cartiere_CartierId",
                table: "Imobil",
                column: "CartierId",
                principalTable: "Cartiere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imobil_Judet_JudetId",
                table: "Imobil",
                column: "JudetId",
                principalTable: "Judet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imobil_Orase_OrasId",
                table: "Imobil",
                column: "OrasId",
                principalTable: "Orase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mesaje_Imobil_ImobilId",
                table: "Mesaje",
                column: "ImobilId",
                principalTable: "Imobil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orase_Judet_JudetId",
                table: "Orase",
                column: "JudetId",
                principalTable: "Judet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Imobil_ImobilId",
                table: "OrderDetails",
                column: "ImobilId",
                principalTable: "Imobil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Imobil_ImobilId",
                table: "ShoppingCartItems",
                column: "ImobilId",
                principalTable: "Imobil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteAnuntItems_Imobil_ImobilId",
                table: "FavoriteAnuntItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Imobil_AspNetUsers_UserId",
                table: "Imobil");

            migrationBuilder.DropForeignKey(
                name: "FK_Imobil_Cartiere_CartierId",
                table: "Imobil");

            migrationBuilder.DropForeignKey(
                name: "FK_Imobil_Judet_JudetId",
                table: "Imobil");

            migrationBuilder.DropForeignKey(
                name: "FK_Imobil_Orase_OrasId",
                table: "Imobil");

            migrationBuilder.DropForeignKey(
                name: "FK_Mesaje_Imobil_ImobilId",
                table: "Mesaje");

            migrationBuilder.DropForeignKey(
                name: "FK_Orase_Judet_JudetId",
                table: "Orase");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Imobil_ImobilId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Imobil_ImobilId",
                table: "ShoppingCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Judet",
                table: "Judet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Imobil",
                table: "Imobil");

            migrationBuilder.RenameTable(
                name: "Judet",
                newName: "Judete");

            migrationBuilder.RenameTable(
                name: "Imobil",
                newName: "Imobile");

            migrationBuilder.RenameIndex(
                name: "IX_Imobil_UserId",
                table: "Imobile",
                newName: "IX_Imobile_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Imobil_OrasId",
                table: "Imobile",
                newName: "IX_Imobile_OrasId");

            migrationBuilder.RenameIndex(
                name: "IX_Imobil_JudetId",
                table: "Imobile",
                newName: "IX_Imobile_JudetId");

            migrationBuilder.RenameIndex(
                name: "IX_Imobil_CartierId",
                table: "Imobile",
                newName: "IX_Imobile_CartierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Judete",
                table: "Judete",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Imobile",
                table: "Imobile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteAnuntItems_Imobile_ImobilId",
                table: "FavoriteAnuntItems",
                column: "ImobilId",
                principalTable: "Imobile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imobile_AspNetUsers_UserId",
                table: "Imobile",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imobile_Cartiere_CartierId",
                table: "Imobile",
                column: "CartierId",
                principalTable: "Cartiere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imobile_Judete_JudetId",
                table: "Imobile",
                column: "JudetId",
                principalTable: "Judete",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imobile_Orase_OrasId",
                table: "Imobile",
                column: "OrasId",
                principalTable: "Orase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mesaje_Imobile_ImobilId",
                table: "Mesaje",
                column: "ImobilId",
                principalTable: "Imobile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orase_Judete_JudetId",
                table: "Orase",
                column: "JudetId",
                principalTable: "Judete",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Imobile_ImobilId",
                table: "OrderDetails",
                column: "ImobilId",
                principalTable: "Imobile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Imobile_ImobilId",
                table: "ShoppingCartItems",
                column: "ImobilId",
                principalTable: "Imobile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
