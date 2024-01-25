using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imobiliare.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NotifyAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlockedIp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    DataAdaugare = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BlockCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Descriere = table.Column<string>(type: "TEXT", nullable: false),
                    TraceBlocking = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdateBlockCount = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRegex = table.Column<bool>(type: "INTEGER", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedIp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titlu = table.Column<string>(type: "TEXT", nullable: false),
                    Mesaj = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateModified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EmailTemplateCategory = table.Column<int>(type: "INTEGER", nullable: false),
                    HumanReadableEmailIdentifier = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Judet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nume = table.Column<string>(type: "TEXT", nullable: true),
                    PrescurtareAuto = table.Column<string>(type: "TEXT", nullable: true),
                    CoordinateGps = table.Column<string>(type: "TEXT", nullable: true),
                    Descriere = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Judet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Thread = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<string>(type: "TEXT", nullable: false),
                    Logger = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Exception = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    AddressLine1 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AddressLine2 = table.Column<string>(type: "TEXT", nullable: true),
                    ZipCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    OrderTotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    OrderPlaced = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RaportActivitate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReceiversCount = table.Column<int>(type: "INTEGER", nullable: false),
                    StiriAdaugateCount = table.Column<int>(type: "INTEGER", nullable: false),
                    AnsambluriAdaugateCount = table.Column<int>(type: "INTEGER", nullable: false),
                    UseriCreatiCount = table.Column<int>(type: "INTEGER", nullable: false),
                    AnunturiByUsersCount = table.Column<int>(type: "INTEGER", nullable: false),
                    AnunturiByAdminCount = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionStartedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionStartedDistinctCount = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditTrailsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LastConsideredHoursCount = table.Column<int>(type: "INTEGER", nullable: false),
                    FinalEmail = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GenerareRaportTime = table.Column<string>(type: "TEXT", nullable: false),
                    IsFakeEmailManager = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaportActivitate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DefaultPageSizeAdminPageAnunturi = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultPageSizeAdminPageUsers = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultPageSizeMainPages = table.Column<int>(type: "INTEGER", nullable: false),
                    LogsRetrieveNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    AutoApproveAnunturi = table.Column<bool>(type: "INTEGER", nullable: false),
                    CapchaEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    UseFakeEmailManager = table.Column<bool>(type: "INTEGER", nullable: false),
                    NotifyAdminByEmailWhenUserContactsAnotherUser = table.Column<bool>(type: "INTEGER", nullable: false),
                    NotifyAdminByEmailWhenUserChangedAnuntByEmail = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoSendAnuntExpiratEmails = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaxDbSize = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Oras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nume = table.Column<string>(type: "TEXT", nullable: true),
                    CoordinateGps = table.Column<string>(type: "TEXT", nullable: true),
                    CodPostal = table.Column<string>(type: "TEXT", nullable: true),
                    ResedintaJudet = table.Column<bool>(type: "INTEGER", nullable: false),
                    LocalitateMica = table.Column<bool>(type: "INTEGER", nullable: false),
                    JudetId = table.Column<int>(type: "INTEGER", nullable: false),
                    DescriereLocalitate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Oras_Judet_JudetId",
                        column: x => x.JudetId,
                        principalTable: "Judet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Agentie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nume = table.Column<string>(type: "TEXT", nullable: true),
                    OrasSelect = table.Column<int>(type: "INTEGER", nullable: true),
                    DescriereAgentie = table.Column<string>(type: "TEXT", nullable: true),
                    Website = table.Column<string>(type: "TEXT", nullable: true),
                    PozaAgentie = table.Column<string>(type: "TEXT", nullable: true),
                    TelefonAgentie = table.Column<string>(type: "TEXT", nullable: true),
                    AfisarePrimaPagina = table.Column<bool>(type: "INTEGER", nullable: false),
                    PromotedLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agentie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agentie_Oras_OrasSelect",
                        column: x => x.OrasSelect,
                        principalTable: "Oras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cartier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nume = table.Column<string>(type: "TEXT", nullable: true),
                    OrasId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cartier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cartier_Oras_OrasId",
                        column: x => x.OrasId,
                        principalTable: "Oras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Constructor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nume = table.Column<string>(type: "TEXT", nullable: true),
                    OrasId = table.Column<int>(type: "INTEGER", nullable: false),
                    Descriere = table.Column<string>(type: "TEXT", nullable: true),
                    Website = table.Column<string>(type: "TEXT", nullable: true),
                    Poza = table.Column<string>(type: "TEXT", nullable: true),
                    Telefon = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Constructor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Constructor_Oras_OrasId",
                        column: x => x.OrasId,
                        principalTable: "Oras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    TipVanzator = table.Column<int>(type: "INTEGER", nullable: false),
                    Picture = table.Column<string>(type: "TEXT", nullable: true),
                    ConfirmationToken = table.Column<string>(type: "TEXT", nullable: true),
                    StareUser = table.Column<int>(type: "INTEGER", nullable: false),
                    TrustedUser = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserCreateDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastLoginTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Flags = table.Column<string>(type: "TEXT", nullable: true),
                    AbonatLaNewsLetter = table.Column<bool>(type: "INTEGER", nullable: false),
                    RestrictionatPrimireEmail = table.Column<bool>(type: "INTEGER", nullable: false),
                    NumeComplet = table.Column<string>(type: "TEXT", nullable: true),
                    ReceiveAdminReports = table.Column<bool>(type: "INTEGER", nullable: false),
                    NumeAgentieImobiliara = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    AgentieId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsAgentieAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    DescriereAgent = table.Column<string>(type: "TEXT", nullable: true),
                    ConstructorId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Agentie_AgentieId",
                        column: x => x.AgentieId,
                        principalTable: "Agentie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Constructor_ConstructorId",
                        column: x => x.ConstructorId,
                        principalTable: "Constructor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ansamblu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titlu = table.Column<string>(type: "TEXT", nullable: false),
                    Continut = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateModified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    NumarVizualizari = table.Column<int>(type: "INTEGER", nullable: false),
                    Poze = table.Column<string>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    Keywords = table.Column<string>(type: "TEXT", nullable: true),
                    OrasSelect = table.Column<int>(type: "INTEGER", nullable: false),
                    GoogleMarkerCoordinateLat = table.Column<double>(type: "REAL", nullable: false),
                    GoogleMarkerCoordinateLong = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ansamblu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ansamblu_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ansamblu_Oras_OrasSelect",
                        column: x => x.OrasSelect,
                        principalTable: "Oras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Imobil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descriere = table.Column<string>(type: "TEXT", maxLength: 5000, nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    Suprafata = table.Column<int>(type: "INTEGER", nullable: false),
                    Poze = table.Column<string>(type: "TEXT", nullable: true),
                    Judet_Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Oras_Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Cartier_Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Strada = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    TipOfertaGen = table.Column<int>(type: "INTEGER", nullable: false),
                    TipProprietate = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    DataAdaugare = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAdaugareInitiala = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAprobare = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Valabilitate = table.Column<int>(type: "INTEGER", nullable: false),
                    Etaj = table.Column<int>(type: "INTEGER", nullable: false),
                    NumarTotalEtaje = table.Column<int>(type: "INTEGER", nullable: false),
                    VechimeImobil = table.Column<int>(type: "INTEGER", nullable: true),
                    NrBalcoane = table.Column<int>(type: "INTEGER", nullable: true),
                    NrBai = table.Column<int>(type: "INTEGER", nullable: true),
                    NumarCamere = table.Column<int>(type: "INTEGER", nullable: true),
                    ContactTelefon = table.Column<string>(type: "TEXT", nullable: false),
                    ContactEmail = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    LinkExtern = table.Column<string>(type: "TEXT", nullable: true),
                    NumarVizualizari = table.Column<int>(type: "INTEGER", nullable: false),
                    StareAprobare = table.Column<int>(type: "INTEGER", nullable: false),
                    GoogleMarkerCoordinateLat = table.Column<double>(type: "REAL", nullable: false),
                    GoogleMarkerCoordinateLong = table.Column<double>(type: "REAL", nullable: false),
                    TipVanzator = table.Column<int>(type: "INTEGER", nullable: false),
                    Garaj = table.Column<bool>(type: "INTEGER", nullable: false),
                    CT = table.Column<bool>(type: "INTEGER", nullable: false),
                    AerConditionat = table.Column<bool>(type: "INTEGER", nullable: false),
                    LocInPivnita = table.Column<bool>(type: "INTEGER", nullable: false),
                    Negociabil = table.Column<bool>(type: "INTEGER", nullable: false),
                    LocParcare = table.Column<bool>(type: "INTEGER", nullable: false),
                    Decomandat = table.Column<bool>(type: "INTEGER", nullable: false),
                    Finisat = table.Column<bool>(type: "INTEGER", nullable: false),
                    PromotedLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    ObservatiiAdmin = table.Column<string>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imobil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imobil_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Imobil_Cartier_Cartier_Id",
                        column: x => x.Cartier_Id,
                        principalTable: "Cartier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Imobil_Judet_Judet_Id",
                        column: x => x.Judet_Id,
                        principalTable: "Judet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Imobil_Oras_Oras_Id",
                        column: x => x.Oras_Id,
                        principalTable: "Oras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategorieStire = table.Column<int>(type: "INTEGER", nullable: false),
                    Titlu = table.Column<string>(type: "TEXT", nullable: false),
                    Continut = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateModified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    NumarVizualizari = table.Column<int>(type: "INTEGER", nullable: false),
                    Poze = table.Column<string>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    AfiseazaPrimaPagina = table.Column<bool>(type: "INTEGER", nullable: false),
                    Keywords = table.Column<string>(type: "TEXT", nullable: true),
                    OrasSelect = table.Column<int>(type: "INTEGER", nullable: false),
                    LinkExtern = table.Column<string>(type: "TEXT", nullable: true),
                    TitluUrl = table.Column<string>(type: "TEXT", nullable: true),
                    MetaDescription = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stire_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Comment = table.Column<string>(type: "TEXT", nullable: false),
                    Nota = table.Column<int>(type: "INTEGER", nullable: false),
                    NumeEvaluator = table.Column<string>(type: "TEXT", nullable: false),
                    RatingCategory = table.Column<int>(type: "INTEGER", nullable: false),
                    UserProfileId = table.Column<string>(type: "TEXT", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "FavoriteAnuntItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImobilId = table.Column<int>(type: "INTEGER", nullable: false),
                    FavoritesID = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteAnuntItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteAnuntItems_Imobil_ImobilId",
                        column: x => x.ImobilId,
                        principalTable: "Imobil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mesaj",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FromUserId = table.Column<string>(type: "TEXT", nullable: false),
                    ToUserId = table.Column<string>(type: "TEXT", nullable: false),
                    Continut = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CitireDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ImobilId = table.Column<int>(type: "INTEGER", nullable: false),
                    Categorie = table.Column<int>(type: "INTEGER", nullable: false),
                    UserContactFormId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesaj", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mesaj_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mesaj_AspNetUsers_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mesaj_AspNetUsers_UserContactFormId",
                        column: x => x.UserContactFormId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mesaj_Imobil_ImobilId",
                        column: x => x.ImobilId,
                        principalTable: "Imobil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ImobilId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Imobil_ImobilId",
                        column: x => x.ImobilId,
                        principalTable: "Imobil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItems",
                columns: table => new
                {
                    ShoppingCartItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImobilId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    ShoppingCartId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItems", x => x.ShoppingCartItemId);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_Imobil_ImobilId",
                        column: x => x.ImobilId,
                        principalTable: "Imobil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agentie_OrasSelect",
                table: "Agentie",
                column: "OrasSelect");

            migrationBuilder.CreateIndex(
                name: "IX_Ansamblu_OrasSelect",
                table: "Ansamblu",
                column: "OrasSelect");

            migrationBuilder.CreateIndex(
                name: "IX_Ansamblu_UserId",
                table: "Ansamblu",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AgentieId",
                table: "AspNetUsers",
                column: "AgentieId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ConstructorId",
                table: "AspNetUsers",
                column: "ConstructorId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cartier_OrasId",
                table: "Cartier",
                column: "OrasId");

            migrationBuilder.CreateIndex(
                name: "IX_Constructor_OrasId",
                table: "Constructor",
                column: "OrasId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteAnuntItems_ImobilId",
                table: "FavoriteAnuntItems",
                column: "ImobilId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Imobil_UserId",
                table: "Imobil",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesaj_FromUserId",
                table: "Mesaj",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesaj_ImobilId",
                table: "Mesaj",
                column: "ImobilId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesaj_ToUserId",
                table: "Mesaj",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesaj_UserContactFormId",
                table: "Mesaj",
                column: "UserContactFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Oras_JudetId",
                table: "Oras",
                column: "JudetId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ImobilId",
                table: "OrderDetails",
                column: "ImobilId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ImobilId",
                table: "ShoppingCartItems",
                column: "ImobilId");

            migrationBuilder.CreateIndex(
                name: "IX_Stire_UserId",
                table: "Stire",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRating_UserProfileId",
                table: "UserRating",
                column: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ansamblu");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditTrail");

            migrationBuilder.DropTable(
                name: "BlockedIp");

            migrationBuilder.DropTable(
                name: "EmailTemplate");

            migrationBuilder.DropTable(
                name: "FavoriteAnuntItems");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Mesaj");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "RaportActivitate");

            migrationBuilder.DropTable(
                name: "ShoppingCartItems");

            migrationBuilder.DropTable(
                name: "Stire");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "UserRating");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Imobil");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Cartier");

            migrationBuilder.DropTable(
                name: "Agentie");

            migrationBuilder.DropTable(
                name: "Constructor");

            migrationBuilder.DropTable(
                name: "Oras");

            migrationBuilder.DropTable(
                name: "Judet");
        }
    }
}
