using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stackoverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class PostAndComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostAndComments",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostAndComments", x => new { x.PostId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_PostAndComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PostAndComments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("4d64c300-950a-4f2b-867f-d3ed6f4f0e2c"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 7, 19, 22, 4, 345, DateTimeKind.Local).AddTicks(3381), new DateTime(2024, 5, 7, 19, 22, 4, 345, DateTimeKind.Local).AddTicks(3381) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("613641bf-ea84-491a-902f-982db227cb1a"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 7, 19, 22, 4, 345, DateTimeKind.Local).AddTicks(3364), new DateTime(2024, 5, 7, 19, 22, 4, 345, DateTimeKind.Local).AddTicks(3365) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("a924e31a-0d24-46e5-a3ca-80d31a1a445b"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 7, 19, 22, 4, 345, DateTimeKind.Local).AddTicks(3384), new DateTime(2024, 5, 7, 19, 22, 4, 345, DateTimeKind.Local).AddTicks(3384) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("c40c756c-f117-4fa4-b7c6-a28f01eb6995"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 7, 19, 22, 4, 345, DateTimeKind.Local).AddTicks(3377), new DateTime(2024, 5, 7, 19, 22, 4, 345, DateTimeKind.Local).AddTicks(3378) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("cad5f124-3a9f-45fc-8553-669146dd08d3"),
                columns: new[] { "CreationDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 5, 7, 19, 22, 4, 345, DateTimeKind.Local).AddTicks(3343), new DateTime(2024, 5, 7, 19, 22, 4, 345, DateTimeKind.Local).AddTicks(3360) });

            migrationBuilder.CreateIndex(
                name: "IX_PostAndComments_CommentId",
                table: "PostAndComments",
                column: "CommentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostAndComments");

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
        }
    }
}
