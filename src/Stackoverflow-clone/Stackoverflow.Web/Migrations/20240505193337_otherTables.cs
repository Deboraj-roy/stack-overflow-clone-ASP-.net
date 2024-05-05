using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stackoverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class otherTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBadges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BadgeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BadgeDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateEarned = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBadges", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("4d64c300-950a-4f2b-867f-d3ed6f4f0e2c"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 6, 1, 33, 37, 172, DateTimeKind.Local).AddTicks(4014), new DateTime(2024, 5, 6, 1, 33, 37, 172, DateTimeKind.Local).AddTicks(4014) });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("613641bf-ea84-491a-902f-982db227cb1a"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 6, 1, 33, 37, 172, DateTimeKind.Local).AddTicks(4007), new DateTime(2024, 5, 6, 1, 33, 37, 172, DateTimeKind.Local).AddTicks(4007) });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("a924e31a-0d24-46e5-a3ca-80d31a1a445b"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 6, 1, 33, 37, 172, DateTimeKind.Local).AddTicks(4017), new DateTime(2024, 5, 6, 1, 33, 37, 172, DateTimeKind.Local).AddTicks(4018) });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("c40c756c-f117-4fa4-b7c6-a28f01eb6995"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 6, 1, 33, 37, 172, DateTimeKind.Local).AddTicks(4010), new DateTime(2024, 5, 6, 1, 33, 37, 172, DateTimeKind.Local).AddTicks(4011) });

            migrationBuilder.UpdateData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("cad5f124-3a9f-45fc-8553-669146dd08d3"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 6, 1, 33, 37, 172, DateTimeKind.Local).AddTicks(3978), new DateTime(2024, 5, 6, 1, 33, 37, 172, DateTimeKind.Local).AddTicks(3992) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "UserBadges");

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
    }
}
