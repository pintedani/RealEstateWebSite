using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imobiliare.Repositories.Migrations
{
    public partial class Sqlite_Switch_Back : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agenties_Orase_OrasSelect",
                table: "Agenties");

            migrationBuilder.DropForeignKey(
                name: "FK_Ansambluri_AspNetUsers_UserId",
                table: "Ansambluri");

            migrationBuilder.DropForeignKey(
                name: "FK_Ansambluri_Orase_OrasSelect",
                table: "Ansambluri");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Agenties_AgentieId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Constructori_ConstructorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Cartiere_Orase_OrasId",
                table: "Cartiere");

            migrationBuilder.DropForeignKey(
                name: "FK_Constructori_Orase_OrasId",
                table: "Constructori");

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
                name: "FK_Mesaje_AspNetUsers_FromUserId",
                table: "Mesaje");

            migrationBuilder.DropForeignKey(
                name: "FK_Mesaje_AspNetUsers_ToUserId",
                table: "Mesaje");

            migrationBuilder.DropForeignKey(
                name: "FK_Mesaje_AspNetUsers_UserContactFormId",
                table: "Mesaje");

            migrationBuilder.DropForeignKey(
                name: "FK_Mesaje_Imobil_ImobilId",
                table: "Mesaje");

            migrationBuilder.DropForeignKey(
                name: "FK_Orase_Judet_JudetId",
                table: "Orase");

            migrationBuilder.DropForeignKey(
                name: "FK_Stires_AspNetUsers_UserId",
                table: "Stires");

            migrationBuilder.DropTable(
                name: "UserRating");

            migrationBuilder.DropIndex(
                name: "IX_Imobil_CartierId",
                table: "Imobil");

            migrationBuilder.DropIndex(
                name: "IX_Imobil_JudetId",
                table: "Imobil");

            migrationBuilder.DropIndex(
                name: "IX_Imobil_OrasId",
                table: "Imobil");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stires",
                table: "Stires");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RaportActivitates",
                table: "RaportActivitates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orase",
                table: "Orase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mesaje",
                table: "Mesaje");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailTemplates",
                table: "EmailTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Constructori",
                table: "Constructori");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cartiere",
                table: "Cartiere");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockedIps",
                table: "BlockedIps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ansambluri",
                table: "Ansambluri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agenties",
                table: "Agenties");

            migrationBuilder.DropColumn(
                name: "CartierId",
                table: "Imobil");

            migrationBuilder.DropColumn(
                name: "JudetId",
                table: "Imobil");

            migrationBuilder.DropColumn(
                name: "OrasId",
                table: "Imobil");

            migrationBuilder.RenameTable(
                name: "Stires",
                newName: "Stire");

            migrationBuilder.RenameTable(
                name: "RaportActivitates",
                newName: "RaportActivitate");

            migrationBuilder.RenameTable(
                name: "Orase",
                newName: "Oras");

            migrationBuilder.RenameTable(
                name: "Mesaje",
                newName: "Mesaj");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "Log");

            migrationBuilder.RenameTable(
                name: "EmailTemplates",
                newName: "EmailTemplate");

            migrationBuilder.RenameTable(
                name: "Constructori",
                newName: "Constructor");

            migrationBuilder.RenameTable(
                name: "Cartiere",
                newName: "Cartier");

            migrationBuilder.RenameTable(
                name: "BlockedIps",
                newName: "BlockedIp");

            migrationBuilder.RenameTable(
                name: "Ansambluri",
                newName: "Ansamblu");

            migrationBuilder.RenameTable(
                name: "Agenties",
                newName: "Agentie");

            migrationBuilder.RenameIndex(
                name: "IX_Stires_UserId",
                table: "Stire",
                newName: "IX_Stire_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orase_JudetId",
                table: "Oras",
                newName: "IX_Oras_JudetId");

            migrationBuilder.RenameIndex(
                name: "IX_Mesaje_UserContactFormId",
                table: "Mesaj",
                newName: "IX_Mesaj_UserContactFormId");

            migrationBuilder.RenameIndex(
                name: "IX_Mesaje_ToUserId",
                table: "Mesaj",
                newName: "IX_Mesaj_ToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Mesaje_ImobilId",
                table: "Mesaj",
                newName: "IX_Mesaj_ImobilId");

            migrationBuilder.RenameIndex(
                name: "IX_Mesaje_FromUserId",
                table: "Mesaj",
                newName: "IX_Mesaj_FromUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Constructori_OrasId",
                table: "Constructor",
                newName: "IX_Constructor_OrasId");

            migrationBuilder.RenameIndex(
                name: "IX_Cartiere_OrasId",
                table: "Cartier",
                newName: "IX_Cartier_OrasId");

            migrationBuilder.RenameIndex(
                name: "IX_Ansambluri_UserId",
                table: "Ansamblu",
                newName: "IX_Ansamblu_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ansambluri_OrasSelect",
                table: "Ansamblu",
                newName: "IX_Ansamblu_OrasSelect");

            migrationBuilder.RenameIndex(
                name: "IX_Agenties_OrasSelect",
                table: "Agentie",
                newName: "IX_Agentie_OrasSelect");

            migrationBuilder.AlterColumn<string>(
                name: "PrescurtareAuto",
                table: "Judet",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nume",
                table: "Judet",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Descriere",
                table: "Judet",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CoordinateGps",
                table: "Judet",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Imobil",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "RowVersion",
                table: "Imobil",
                type: "rowversion",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "rowversion",
                oldRowVersion: true);

            migrationBuilder.AddColumn<int>(
                name: "Cartier_Id",
                table: "Imobil",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Judet_Id",
                table: "Imobil",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Oras_Id",
                table: "Imobil",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AuditTrail",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Picture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NumeComplet",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NumeAgentieImobiliara",
                table: "AspNetUsers",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<string>(
                name: "Flags",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriereAgent",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmationToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Stire",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "TitluUrl",
                table: "Stire",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Poze",
                table: "Stire",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MetaDescription",
                table: "Stire",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LinkExtern",
                table: "Stire",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Keywords",
                table: "Stire",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nume",
                table: "Oras",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriereLocalitate",
                table: "Oras",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CoordinateGps",
                table: "Oras",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CodPostal",
                table: "Oras",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Thread",
                table: "Log",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Constructor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Telefon",
                table: "Constructor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Poza",
                table: "Constructor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "OrasId",
                table: "Constructor",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Nume",
                table: "Constructor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Descriere",
                table: "Constructor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nume",
                table: "Cartier",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ansamblu",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Poze",
                table: "Ansamblu",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Keywords",
                table: "Ansamblu",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Agentie",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TelefonAgentie",
                table: "Agentie",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PozaAgentie",
                table: "Agentie",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nume",
                table: "Agentie",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriereAgentie",
                table: "Agentie",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stire",
                table: "Stire",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RaportActivitate",
                table: "RaportActivitate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Oras",
                table: "Oras",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mesaj",
                table: "Mesaj",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Log",
                table: "Log",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailTemplate",
                table: "EmailTemplate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Constructor",
                table: "Constructor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cartier",
                table: "Cartier",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockedIp",
                table: "BlockedIp",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ansamblu",
                table: "Ansamblu",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agentie",
                table: "Agentie",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Imobil_Cartier_Id",
                table: "Imobil",
                column: "Cartier_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Imobil_Judet_Id",
                table: "Imobil",
                column: "Judet_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Imobil_Oras_Id",
                table: "Imobil",
                column: "Oras_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Agentie_Oras_OrasSelect",
                table: "Agentie",
                column: "OrasSelect",
                principalTable: "Oras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ansamblu_AspNetUsers_UserId",
                table: "Ansamblu",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ansamblu_Oras_OrasSelect",
                table: "Ansamblu",
                column: "OrasSelect",
                principalTable: "Oras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Agentie_AgentieId",
                table: "AspNetUsers",
                column: "AgentieId",
                principalTable: "Agentie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Constructor_ConstructorId",
                table: "AspNetUsers",
                column: "ConstructorId",
                principalTable: "Constructor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cartier_Oras_OrasId",
                table: "Cartier",
                column: "OrasId",
                principalTable: "Oras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Constructor_Oras_OrasId",
                table: "Constructor",
                column: "OrasId",
                principalTable: "Oras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imobil_Cartier_Cartier_Id",
                table: "Imobil",
                column: "Cartier_Id",
                principalTable: "Cartier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imobil_Judet_Judet_Id",
                table: "Imobil",
                column: "Judet_Id",
                principalTable: "Judet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imobil_Oras_Oras_Id",
                table: "Imobil",
                column: "Oras_Id",
                principalTable: "Oras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mesaj_AspNetUsers_FromUserId",
                table: "Mesaj",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mesaj_AspNetUsers_ToUserId",
                table: "Mesaj",
                column: "ToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mesaj_AspNetUsers_UserContactFormId",
                table: "Mesaj",
                column: "UserContactFormId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mesaj_Imobil_ImobilId",
                table: "Mesaj",
                column: "ImobilId",
                principalTable: "Imobil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Oras_Judet_JudetId",
                table: "Oras",
                column: "JudetId",
                principalTable: "Judet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stire_AspNetUsers_UserId",
                table: "Stire",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agentie_Oras_OrasSelect",
                table: "Agentie");

            migrationBuilder.DropForeignKey(
                name: "FK_Ansamblu_AspNetUsers_UserId",
                table: "Ansamblu");

            migrationBuilder.DropForeignKey(
                name: "FK_Ansamblu_Oras_OrasSelect",
                table: "Ansamblu");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Agentie_AgentieId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Constructor_ConstructorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Cartier_Oras_OrasId",
                table: "Cartier");

            migrationBuilder.DropForeignKey(
                name: "FK_Constructor_Oras_OrasId",
                table: "Constructor");

            migrationBuilder.DropForeignKey(
                name: "FK_Imobil_Cartier_Cartier_Id",
                table: "Imobil");

            migrationBuilder.DropForeignKey(
                name: "FK_Imobil_Judet_Judet_Id",
                table: "Imobil");

            migrationBuilder.DropForeignKey(
                name: "FK_Imobil_Oras_Oras_Id",
                table: "Imobil");

            migrationBuilder.DropForeignKey(
                name: "FK_Mesaj_AspNetUsers_FromUserId",
                table: "Mesaj");

            migrationBuilder.DropForeignKey(
                name: "FK_Mesaj_AspNetUsers_ToUserId",
                table: "Mesaj");

            migrationBuilder.DropForeignKey(
                name: "FK_Mesaj_AspNetUsers_UserContactFormId",
                table: "Mesaj");

            migrationBuilder.DropForeignKey(
                name: "FK_Mesaj_Imobil_ImobilId",
                table: "Mesaj");

            migrationBuilder.DropForeignKey(
                name: "FK_Oras_Judet_JudetId",
                table: "Oras");

            migrationBuilder.DropForeignKey(
                name: "FK_Stire_AspNetUsers_UserId",
                table: "Stire");

            migrationBuilder.DropIndex(
                name: "IX_Imobil_Cartier_Id",
                table: "Imobil");

            migrationBuilder.DropIndex(
                name: "IX_Imobil_Judet_Id",
                table: "Imobil");

            migrationBuilder.DropIndex(
                name: "IX_Imobil_Oras_Id",
                table: "Imobil");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stire",
                table: "Stire");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RaportActivitate",
                table: "RaportActivitate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Oras",
                table: "Oras");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mesaj",
                table: "Mesaj");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Log",
                table: "Log");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailTemplate",
                table: "EmailTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Constructor",
                table: "Constructor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cartier",
                table: "Cartier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockedIp",
                table: "BlockedIp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ansamblu",
                table: "Ansamblu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agentie",
                table: "Agentie");

            migrationBuilder.DropColumn(
                name: "Cartier_Id",
                table: "Imobil");

            migrationBuilder.DropColumn(
                name: "Judet_Id",
                table: "Imobil");

            migrationBuilder.DropColumn(
                name: "Oras_Id",
                table: "Imobil");

            migrationBuilder.RenameTable(
                name: "Stire",
                newName: "Stires");

            migrationBuilder.RenameTable(
                name: "RaportActivitate",
                newName: "RaportActivitates");

            migrationBuilder.RenameTable(
                name: "Oras",
                newName: "Orase");

            migrationBuilder.RenameTable(
                name: "Mesaj",
                newName: "Mesaje");

            migrationBuilder.RenameTable(
                name: "Log",
                newName: "Logs");

            migrationBuilder.RenameTable(
                name: "EmailTemplate",
                newName: "EmailTemplates");

            migrationBuilder.RenameTable(
                name: "Constructor",
                newName: "Constructori");

            migrationBuilder.RenameTable(
                name: "Cartier",
                newName: "Cartiere");

            migrationBuilder.RenameTable(
                name: "BlockedIp",
                newName: "BlockedIps");

            migrationBuilder.RenameTable(
                name: "Ansamblu",
                newName: "Ansambluri");

            migrationBuilder.RenameTable(
                name: "Agentie",
                newName: "Agenties");

            migrationBuilder.RenameIndex(
                name: "IX_Stire_UserId",
                table: "Stires",
                newName: "IX_Stires_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Oras_JudetId",
                table: "Orase",
                newName: "IX_Orase_JudetId");

            migrationBuilder.RenameIndex(
                name: "IX_Mesaj_UserContactFormId",
                table: "Mesaje",
                newName: "IX_Mesaje_UserContactFormId");

            migrationBuilder.RenameIndex(
                name: "IX_Mesaj_ToUserId",
                table: "Mesaje",
                newName: "IX_Mesaje_ToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Mesaj_ImobilId",
                table: "Mesaje",
                newName: "IX_Mesaje_ImobilId");

            migrationBuilder.RenameIndex(
                name: "IX_Mesaj_FromUserId",
                table: "Mesaje",
                newName: "IX_Mesaje_FromUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Constructor_OrasId",
                table: "Constructori",
                newName: "IX_Constructori_OrasId");

            migrationBuilder.RenameIndex(
                name: "IX_Cartier_OrasId",
                table: "Cartiere",
                newName: "IX_Cartiere_OrasId");

            migrationBuilder.RenameIndex(
                name: "IX_Ansamblu_UserId",
                table: "Ansambluri",
                newName: "IX_Ansambluri_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ansamblu_OrasSelect",
                table: "Ansambluri",
                newName: "IX_Ansambluri_OrasSelect");

            migrationBuilder.RenameIndex(
                name: "IX_Agentie_OrasSelect",
                table: "Agenties",
                newName: "IX_Agenties_OrasSelect");

            migrationBuilder.AlterColumn<string>(
                name: "PrescurtareAuto",
                table: "Judet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nume",
                table: "Judet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descriere",
                table: "Judet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoordinateGps",
                table: "Judet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Imobil",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "RowVersion",
                table: "Imobil",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "rowversion",
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CartierId",
                table: "Imobil",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JudetId",
                table: "Imobil",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrasId",
                table: "Imobil",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AuditTrail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Picture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeComplet",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeAgentieImobiliara",
                table: "AspNetUsers",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Flags",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriereAgent",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmationToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Stires",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TitluUrl",
                table: "Stires",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Poze",
                table: "Stires",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetaDescription",
                table: "Stires",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinkExtern",
                table: "Stires",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Keywords",
                table: "Stires",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nume",
                table: "Orase",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriereLocalitate",
                table: "Orase",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoordinateGps",
                table: "Orase",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CodPostal",
                table: "Orase",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Thread",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Constructori",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telefon",
                table: "Constructori",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Poza",
                table: "Constructori",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrasId",
                table: "Constructori",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nume",
                table: "Constructori",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descriere",
                table: "Constructori",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nume",
                table: "Cartiere",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ansambluri",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Poze",
                table: "Ansambluri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Keywords",
                table: "Ansambluri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Agenties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TelefonAgentie",
                table: "Agenties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PozaAgentie",
                table: "Agenties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nume",
                table: "Agenties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriereAgentie",
                table: "Agenties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stires",
                table: "Stires",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RaportActivitates",
                table: "RaportActivitates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orase",
                table: "Orase",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mesaje",
                table: "Mesaje",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs",
                table: "Logs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailTemplates",
                table: "EmailTemplates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Constructori",
                table: "Constructori",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cartiere",
                table: "Cartiere",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockedIps",
                table: "BlockedIps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ansambluri",
                table: "Ansambluri",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agenties",
                table: "Agenties",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    NumeEvaluator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RatingCategory = table.Column<int>(type: "int", nullable: false),
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRating_AspNetUsers_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Imobil_CartierId",
                table: "Imobil",
                column: "CartierId");

            migrationBuilder.CreateIndex(
                name: "IX_Imobil_JudetId",
                table: "Imobil",
                column: "JudetId");

            migrationBuilder.CreateIndex(
                name: "IX_Imobil_OrasId",
                table: "Imobil",
                column: "OrasId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRating_UserProfileId",
                table: "UserRating",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agenties_Orase_OrasSelect",
                table: "Agenties",
                column: "OrasSelect",
                principalTable: "Orase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ansambluri_AspNetUsers_UserId",
                table: "Ansambluri",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ansambluri_Orase_OrasSelect",
                table: "Ansambluri",
                column: "OrasSelect",
                principalTable: "Orase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Agenties_AgentieId",
                table: "AspNetUsers",
                column: "AgentieId",
                principalTable: "Agenties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Constructori_ConstructorId",
                table: "AspNetUsers",
                column: "ConstructorId",
                principalTable: "Constructori",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cartiere_Orase_OrasId",
                table: "Cartiere",
                column: "OrasId",
                principalTable: "Orase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Constructori_Orase_OrasId",
                table: "Constructori",
                column: "OrasId",
                principalTable: "Orase",
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
                name: "FK_Mesaje_AspNetUsers_FromUserId",
                table: "Mesaje",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mesaje_AspNetUsers_ToUserId",
                table: "Mesaje",
                column: "ToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mesaje_AspNetUsers_UserContactFormId",
                table: "Mesaje",
                column: "UserContactFormId",
                principalTable: "AspNetUsers",
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
                name: "FK_Stires_AspNetUsers_UserId",
                table: "Stires",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
