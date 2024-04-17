using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Stackoverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class seedUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("16f53ba8-f8fd-4cf6-6f5b-08dc5edbc664"), null, "User", "USER" },
                    { new Guid("244902a4-a09b-410d-6f5c-08dc5edbc664"), null, "Newbie", "NEWBIE" },
                    { new Guid("2f0576b4-9665-47bc-6f5e-08dc5edbc664"), null, "Power User", "POWER USER" },
                    { new Guid("4b78faf4-752b-4806-6f5d-08dc5edbc664"), null, "Elite", "ELITE" },
                    { new Guid("a1f7777a-6530-4fee-6f5a-08dc5edbc664"), null, "Admin", "ADMIN" },
                    { new Guid("d35e0e83-9cdf-4819-6f5f-08dc5edbc664"), null, "VIP", "VIP" }
                });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("4d64c300-950a-4f2b-867f-d3ed6f4f0e2c"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 4, 17, 18, 59, 27, 360, DateTimeKind.Local).AddTicks(2483), new DateTime(2024, 4, 17, 18, 59, 27, 360, DateTimeKind.Local).AddTicks(2483) });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("613641bf-ea84-491a-902f-982db227cb1a"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 4, 17, 18, 59, 27, 360, DateTimeKind.Local).AddTicks(2466), new DateTime(2024, 4, 17, 18, 59, 27, 360, DateTimeKind.Local).AddTicks(2466) });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("a924e31a-0d24-46e5-a3ca-80d31a1a445b"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 4, 17, 18, 59, 27, 360, DateTimeKind.Local).AddTicks(2487), new DateTime(2024, 4, 17, 18, 59, 27, 360, DateTimeKind.Local).AddTicks(2487) });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("c40c756c-f117-4fa4-b7c6-a28f01eb6995"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 4, 17, 18, 59, 27, 360, DateTimeKind.Local).AddTicks(2479), new DateTime(2024, 4, 17, 18, 59, 27, 360, DateTimeKind.Local).AddTicks(2479) });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("cad5f124-3a9f-45fc-8553-669146dd08d3"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 4, 17, 18, 59, 27, 360, DateTimeKind.Local).AddTicks(2449), new DateTime(2024, 4, 17, 18, 59, 27, 360, DateTimeKind.Local).AddTicks(2462) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("16f53ba8-f8fd-4cf6-6f5b-08dc5edbc664"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("244902a4-a09b-410d-6f5c-08dc5edbc664"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2f0576b4-9665-47bc-6f5e-08dc5edbc664"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4b78faf4-752b-4806-6f5d-08dc5edbc664"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a1f7777a-6530-4fee-6f5a-08dc5edbc664"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d35e0e83-9cdf-4819-6f5f-08dc5edbc664"));

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("4d64c300-950a-4f2b-867f-d3ed6f4f0e2c"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2412), new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2412) });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("613641bf-ea84-491a-902f-982db227cb1a"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2405), new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2406) });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("a924e31a-0d24-46e5-a3ca-80d31a1a445b"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2415), new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2416) });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("c40c756c-f117-4fa4-b7c6-a28f01eb6995"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2409), new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2409) });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("cad5f124-3a9f-45fc-8553-669146dd08d3"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2391), new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2402) });
        }
    }
}
