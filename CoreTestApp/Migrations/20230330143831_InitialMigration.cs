using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreTestApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Imobils",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descriere = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Suprafata = table.Column<int>(type: "int", nullable: false),
                    Poze = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Strada = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    DataAdaugare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAdaugareInitiala = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAprobare = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Valabilitate = table.Column<int>(type: "int", nullable: false),
                    Etaj = table.Column<int>(type: "int", nullable: false),
                    NumarTotalEtaje = table.Column<int>(type: "int", nullable: false),
                    VechimeImobil = table.Column<int>(type: "int", nullable: true),
                    NrBalcoane = table.Column<int>(type: "int", nullable: true),
                    NrBai = table.Column<int>(type: "int", nullable: true),
                    NumarCamere = table.Column<int>(type: "int", nullable: true),
                    ContactTelefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    LinkExtern = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumarVizualizari = table.Column<int>(type: "int", nullable: false),
                    GoogleMarkerCoordinateLat = table.Column<double>(type: "float", nullable: false),
                    GoogleMarkerCoordinateLong = table.Column<double>(type: "float", nullable: false),
                    Garaj = table.Column<bool>(type: "bit", nullable: false),
                    CT = table.Column<bool>(type: "bit", nullable: false),
                    AerConditionat = table.Column<bool>(type: "bit", nullable: false),
                    LocInPivnita = table.Column<bool>(type: "bit", nullable: false),
                    Negociabil = table.Column<bool>(type: "bit", nullable: false),
                    LocParcare = table.Column<bool>(type: "bit", nullable: false),
                    Decomandat = table.Column<bool>(type: "bit", nullable: false),
                    Finisat = table.Column<bool>(type: "bit", nullable: false),
                    ObservatiiAdmin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imobils", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Imobils");
        }
    }
}
