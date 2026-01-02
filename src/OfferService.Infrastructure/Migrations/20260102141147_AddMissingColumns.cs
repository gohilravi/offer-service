using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OfferService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    SellerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.SellerId);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    OfferId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SellerId = table.Column<int>(type: "integer", nullable: false),
                    SellerNetworkId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SellerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Vin = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: true),
                    VehicleYear = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VehicleMake = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VehicleModel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VehicleTrim = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VehicleBodyType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VehicleCabType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VehicleDoorCount = table.Column<int>(type: "integer", nullable: false),
                    VehicleFuelType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VehicleBodyStyle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VehicleUsage = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VehicleZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BuyerZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    OwnershipType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OwnershipTitleType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Mileage = table.Column<int>(type: "integer", nullable: false),
                    IsMileageUnverifiable = table.Column<bool>(type: "boolean", nullable: false),
                    DrivetrainCondition = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    KeyOrFobAvailable = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    WorkingBatteryInstalled = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AllTiresInflated = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    WheelsRemoved = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    WheelsRemovedDriverFront = table.Column<bool>(type: "boolean", nullable: false),
                    WheelsRemovedDriverRear = table.Column<bool>(type: "boolean", nullable: false),
                    WheelsRemovedPassengerFront = table.Column<bool>(type: "boolean", nullable: false),
                    WheelsRemovedPassengerRear = table.Column<bool>(type: "boolean", nullable: false),
                    BodyPanelsIntact = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BodyDamageFree = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MirrorsLightsGlassIntact = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    InteriorIntact = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FloodFireDamageFree = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EngineTransmissionCondition = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AirbagsDeployed = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "offered"),
                    PurchaseId = table.Column<Guid>(type: "uuid", nullable: true),
                    TransportId = table.Column<Guid>(type: "uuid", nullable: true),
                    BuyerId = table.Column<int>(type: "integer", nullable: true),
                    CarrierId = table.Column<int>(type: "integer", nullable: true),
                    NoSQLIndexId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.OfferId);
                    table.ForeignKey(
                        name: "FK_Offers_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "SellerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Sellers",
                columns: new[] { "SellerId", "CreatedAt", "Email", "LastModifiedAt", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 2, 14, 11, 46, 53, DateTimeKind.Utc).AddTicks(8412), "john.doe@example.com", new DateTime(2026, 1, 2, 14, 11, 46, 53, DateTimeKind.Utc).AddTicks(8417), "John Doe", "$2a$11$HNCCQYeZ1ATeQyyyhdTOk.2itJrBUc.BPJGRSjw3sVcihaqzVCbK." },
                    { 2, new DateTime(2026, 1, 2, 14, 11, 46, 208, DateTimeKind.Utc).AddTicks(4282), "jane.smith@example.com", new DateTime(2026, 1, 2, 14, 11, 46, 208, DateTimeKind.Utc).AddTicks(4289), "Jane Smith", "$2a$11$//p.CBierACcyVGxkjss4ehmbmkpixS5S9uFUBQBIqNsHcfRWVVz6" },
                    { 3, new DateTime(2026, 1, 2, 14, 11, 46, 365, DateTimeKind.Utc).AddTicks(2791), "bob.johnson@example.com", new DateTime(2026, 1, 2, 14, 11, 46, 365, DateTimeKind.Utc).AddTicks(2798), "Bob Johnson", "$2a$11$tg.DiJVlXNCiRUWTfX/9/.5UI0G9PK/kiWUAvKVe4ImiSl1hkaEjq" },
                    { 4, new DateTime(2026, 1, 2, 14, 11, 46, 519, DateTimeKind.Utc).AddTicks(8701), "alice.williams@example.com", new DateTime(2026, 1, 2, 14, 11, 46, 519, DateTimeKind.Utc).AddTicks(8709), "Alice Williams", "$2a$11$9OWhbbduUTnnlyv7a6FsZuszXKDBvwIqE21taB1E0.au.gimA0niK" },
                    { 5, new DateTime(2026, 1, 2, 14, 11, 46, 673, DateTimeKind.Utc).AddTicks(1396), "charlie.brown@example.com", new DateTime(2026, 1, 2, 14, 11, 46, 673, DateTimeKind.Utc).AddTicks(1404), "Charlie Brown", "$2a$11$ocgUeG/dKfbSIb4n9dTex.EqbiiHSKdPGT4gtb3DL94eLYpI.8iB2" },
                    { 6, new DateTime(2026, 1, 2, 14, 11, 46, 826, DateTimeKind.Utc).AddTicks(3892), "diana.davis@example.com", new DateTime(2026, 1, 2, 14, 11, 46, 826, DateTimeKind.Utc).AddTicks(3902), "Diana Davis", "$2a$11$dD4iY2hahVAaPy2LEYSsQumw4OJlfkAeS7/YodKqf/SFDZDxgitIO" },
                    { 7, new DateTime(2026, 1, 2, 14, 11, 46, 979, DateTimeKind.Utc).AddTicks(5800), "edward.miller@example.com", new DateTime(2026, 1, 2, 14, 11, 46, 979, DateTimeKind.Utc).AddTicks(5806), "Edward Miller", "$2a$11$Z4Dre4brHqTBnTkMsVabk.fpsZqM1ml7ANScBuuJD3IPZqiWYvG62" },
                    { 8, new DateTime(2026, 1, 2, 14, 11, 47, 132, DateTimeKind.Utc).AddTicks(8005), "fiona.wilson@example.com", new DateTime(2026, 1, 2, 14, 11, 47, 132, DateTimeKind.Utc).AddTicks(8011), "Fiona Wilson", "$2a$11$IpBx353qIFa0/j1Aisv7g.kGjK7M9ND1Hjswu5.RT5eEq5lRypsQa" },
                    { 9, new DateTime(2026, 1, 2, 14, 11, 47, 285, DateTimeKind.Utc).AddTicks(9351), "george.moore@example.com", new DateTime(2026, 1, 2, 14, 11, 47, 285, DateTimeKind.Utc).AddTicks(9356), "George Moore", "$2a$11$z7HJJ.1KyI8dzmz.wCggh.hhr5zyRIx4ofa/9I6VZxaBY2ibnSiuu" },
                    { 10, new DateTime(2026, 1, 2, 14, 11, 47, 441, DateTimeKind.Utc).AddTicks(9506), "helen.taylor@example.com", new DateTime(2026, 1, 2, 14, 11, 47, 441, DateTimeKind.Utc).AddTicks(9520), "Helen Taylor", "$2a$11$cdDxXwmOwluiLgPLWpg1se.mJ9dugl8Mdu3x26W6X26vJ9Fidmxby" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_CreatedAt",
                table: "Offers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_SellerId_Status",
                table: "Offers",
                columns: new[] { "SellerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_Status",
                table: "Offers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_Email",
                table: "Sellers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Sellers");
        }
    }
}
