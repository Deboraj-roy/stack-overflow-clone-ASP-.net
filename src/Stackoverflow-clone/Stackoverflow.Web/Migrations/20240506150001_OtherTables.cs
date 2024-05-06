using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stackoverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class OtherTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_posts",
                table: "posts");

            migrationBuilder.RenameTable(
                name: "posts",
                newName: "Posts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

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
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("4d64c300-950a-4f2b-867f-d3ed6f4f0e2c"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 6, 21, 0, 1, 814, DateTimeKind.Local).AddTicks(2732), new DateTime(2024, 5, 6, 21, 0, 1, 814, DateTimeKind.Local).AddTicks(2732) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("613641bf-ea84-491a-902f-982db227cb1a"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 6, 21, 0, 1, 814, DateTimeKind.Local).AddTicks(2714), new DateTime(2024, 5, 6, 21, 0, 1, 814, DateTimeKind.Local).AddTicks(2715) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("a924e31a-0d24-46e5-a3ca-80d31a1a445b"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 6, 21, 0, 1, 814, DateTimeKind.Local).AddTicks(2735), new DateTime(2024, 5, 6, 21, 0, 1, 814, DateTimeKind.Local).AddTicks(2736) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("c40c756c-f117-4fa4-b7c6-a28f01eb6995"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 6, 21, 0, 1, 814, DateTimeKind.Local).AddTicks(2729), new DateTime(2024, 5, 6, 21, 0, 1, 814, DateTimeKind.Local).AddTicks(2729) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("cad5f124-3a9f-45fc-8553-669146dd08d3"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 6, 21, 0, 1, 814, DateTimeKind.Local).AddTicks(2699), new DateTime(2024, 5, 6, 21, 0, 1, 814, DateTimeKind.Local).AddTicks(2711) });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "posts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_posts",
                table: "posts",
                column: "Id");

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
