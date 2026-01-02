using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfferService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingColumnsForced : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add missing columns directly
            migrationBuilder.Sql(@"ALTER TABLE ""Offers"" ADD COLUMN IF NOT EXISTS ""BuyerId"" INTEGER;");
            migrationBuilder.Sql(@"ALTER TABLE ""Offers"" ADD COLUMN IF NOT EXISTS ""CarrierId"" INTEGER;");
            migrationBuilder.Sql(@"ALTER TABLE ""Offers"" ADD COLUMN IF NOT EXISTS ""BuyerZipCode"" VARCHAR(20);");
            migrationBuilder.Sql(@"ALTER TABLE ""Offers"" ALTER COLUMN ""Vin"" DROP NOT NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""Offers"" DROP COLUMN IF EXISTS ""BuyerId"";");
            migrationBuilder.Sql(@"ALTER TABLE ""Offers"" DROP COLUMN IF EXISTS ""CarrierId"";");
            migrationBuilder.Sql(@"ALTER TABLE ""Offers"" DROP COLUMN IF EXISTS ""BuyerZipCode"";");
        }
    }
}