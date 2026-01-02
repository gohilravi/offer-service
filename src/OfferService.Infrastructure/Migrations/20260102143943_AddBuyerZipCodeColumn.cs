using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfferService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBuyerZipCodeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 39, 42, 106, DateTimeKind.Utc).AddTicks(2848), new DateTime(2026, 1, 2, 14, 39, 42, 106, DateTimeKind.Utc).AddTicks(2854), "$2a$11$hiwZ8KF9BDfMqUxmqQI/x.OLkVX.o47Z24GcFFBS44V68eAf6I58y" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 39, 42, 259, DateTimeKind.Utc).AddTicks(9330), new DateTime(2026, 1, 2, 14, 39, 42, 259, DateTimeKind.Utc).AddTicks(9336), "$2a$11$/SUUBUyBWlWDqizOYZ4x9uxsmWRLvT7DBfTV8Moj2tiQuG6U0R1si" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 39, 42, 413, DateTimeKind.Utc).AddTicks(1537), new DateTime(2026, 1, 2, 14, 39, 42, 413, DateTimeKind.Utc).AddTicks(1541), "$2a$11$sXY2OMNRndNtzVHoOaVGjOL3QlpwJ7N4DJPmtFpwez.AQi94IO/zu" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 39, 42, 574, DateTimeKind.Utc).AddTicks(4944), new DateTime(2026, 1, 2, 14, 39, 42, 574, DateTimeKind.Utc).AddTicks(4954), "$2a$11$WXsT6H4weMd963sEgCS0vehNk/Iwb3gS3ybfRhWocmq/lYguewEWa" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 39, 42, 727, DateTimeKind.Utc).AddTicks(3037), new DateTime(2026, 1, 2, 14, 39, 42, 727, DateTimeKind.Utc).AddTicks(3041), "$2a$11$M6QPrHMfQL3PhLj36okGlOpFVAiMYoMiKS6kWH9Dk8EiH2EVju9V6" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 39, 42, 853, DateTimeKind.Utc).AddTicks(3307), new DateTime(2026, 1, 2, 14, 39, 42, 853, DateTimeKind.Utc).AddTicks(3311), "$2a$11$CsO5CGIv1OZ2UYkfCap1xe2YQ6UkVuDtmSLkjBKJZ2gp7GeYN3Es6" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 39, 42, 978, DateTimeKind.Utc).AddTicks(9458), new DateTime(2026, 1, 2, 14, 39, 42, 978, DateTimeKind.Utc).AddTicks(9459), "$2a$11$rb/YLOcz/1iXODkNi0unteDKce8E1CgNgJH7BWyq8iVVd5WjciFwy" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 8,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 39, 43, 104, DateTimeKind.Utc).AddTicks(5593), new DateTime(2026, 1, 2, 14, 39, 43, 104, DateTimeKind.Utc).AddTicks(5598), "$2a$11$UzLSj3oKM.y/MpPAb3n.OuCqc0rrLZsGTSKGCd61vPihT3AvcQWmy" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 9,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 39, 43, 230, DateTimeKind.Utc).AddTicks(1657), new DateTime(2026, 1, 2, 14, 39, 43, 230, DateTimeKind.Utc).AddTicks(1659), "$2a$11$1RuKdk.ZJK59T818F5Bseeu5K7Im1sMSpNa4Z3ed/QqKxe.zw2SNe" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 10,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 39, 43, 356, DateTimeKind.Utc).AddTicks(1359), new DateTime(2026, 1, 2, 14, 39, 43, 356, DateTimeKind.Utc).AddTicks(1362), "$2a$11$YmKJLz.FyCdJ4cZWxUowC.wH8TNxvv2.4KfrEU2vT4f4Rz4KGjyjy" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 26, 35, 771, DateTimeKind.Utc).AddTicks(5210), new DateTime(2026, 1, 2, 14, 26, 35, 771, DateTimeKind.Utc).AddTicks(5217), "$2a$11$7faO2XQwCbC74b/E7wr4YetSbTbDTMMbAlZYQe..ZzdRMA0O4isj2" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 26, 35, 911, DateTimeKind.Utc).AddTicks(1522), new DateTime(2026, 1, 2, 14, 26, 35, 911, DateTimeKind.Utc).AddTicks(1529), "$2a$11$lSa0MCVPAclsTU2rwvy3n.n4MV.nAVTyJ4qGdGPD0e/2vUFRfagby" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 26, 36, 65, DateTimeKind.Utc).AddTicks(5626), new DateTime(2026, 1, 2, 14, 26, 36, 65, DateTimeKind.Utc).AddTicks(5631), "$2a$11$nqM6msPzje7ys234vJLWK.5GNh.JMj7GQrBn.we.ZpEfnhoin.kcC" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 26, 36, 237, DateTimeKind.Utc).AddTicks(6581), new DateTime(2026, 1, 2, 14, 26, 36, 237, DateTimeKind.Utc).AddTicks(6589), "$2a$11$h2avwvAUWu1EAxK3cco3Kuvop3oMI0o7ssYZGw2.J54Q0.ZpwENkO" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 26, 36, 379, DateTimeKind.Utc).AddTicks(5905), new DateTime(2026, 1, 2, 14, 26, 36, 379, DateTimeKind.Utc).AddTicks(5911), "$2a$11$VDPIEJty7BAzl/7c8AUL9O.MCdCiUBesGjc8Bv8W2V/YfxxlX6Lqm" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 26, 36, 508, DateTimeKind.Utc).AddTicks(8517), new DateTime(2026, 1, 2, 14, 26, 36, 508, DateTimeKind.Utc).AddTicks(8523), "$2a$11$di3coA.C43ZM2wGiYYul7uWa8yynIuT7MIQIujJat7hCHCqWIoI/G" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 26, 36, 658, DateTimeKind.Utc).AddTicks(1297), new DateTime(2026, 1, 2, 14, 26, 36, 658, DateTimeKind.Utc).AddTicks(1303), "$2a$11$uotvRcIpGzaUMi/1T3FOYeshQWf.MQ5RfRuUwlILG1bDUJohx0fCC" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 8,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 26, 36, 784, DateTimeKind.Utc).AddTicks(5862), new DateTime(2026, 1, 2, 14, 26, 36, 784, DateTimeKind.Utc).AddTicks(5869), "$2a$11$1oegcXpIF92/yX/WXSdM7ecoLknQZmXP/z2L1r3XSht8x0XW07n5K" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 9,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 26, 36, 921, DateTimeKind.Utc).AddTicks(2015), new DateTime(2026, 1, 2, 14, 26, 36, 921, DateTimeKind.Utc).AddTicks(2026), "$2a$11$2AqGoKjJm3Ing8AYoTmFSON45O4pj.RxoRcQJ7y7paA59fhhV.Emy" });

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "SellerId",
                keyValue: 10,
                columns: new[] { "CreatedAt", "LastModifiedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 14, 26, 37, 74, DateTimeKind.Utc).AddTicks(7090), new DateTime(2026, 1, 2, 14, 26, 37, 74, DateTimeKind.Utc).AddTicks(7096), "$2a$11$j1PhptgiwJrTpXKpTQp.PO1dZiqjIzGJ52XSCvkn9/yK7znn74AWG" });
        }
    }
}
