using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Stackoverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedPostData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "posts",
                columns: new[] { "Id", "Body", "CreationDate", "Downvotes", "IsDeleted", "LastModifiedDate", "PostType", "Title", "Upvotes", "UserId" },
                values: new object[,]
                {
                    { new Guid("4d64c300-950a-4f2b-867f-d3ed6f4f0e2c"), "Python is often recommended for beginners due to its simple syntax and versatility. It's used in various domains such as web development, data science, and automation. Additionally, there are plenty of resources available online for learning Python.", new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2412), 0, false, new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2412), 1, "Re: Best programming language for beginners?", 12, new Guid("e7a46768-075e-41ff-8732-79bc75bf0711") },
                    { new Guid("613641bf-ea84-491a-902f-982db227cb1a"), "One way to improve your programming skills is by practicing regularly. Try to solve coding problems daily and participate in coding competitions. Also, consider contributing to open-source projects on platforms like GitHub.", new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2405), 1, false, new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2406), 1, "Re: How to improve programming skills?", 15, new Guid("940a3086-054e-4dbb-8ba3-1b83df4ee62e") },
                    { new Guid("a924e31a-0d24-46e5-a3ca-80d31a1a445b"), "I'm preparing for technical interviews and would like some advice on the best way to study and practice. What topics should I focus on, and are there any resources you recommend?", new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2415), 1, false, new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2416), 0, "How to prepare for technical interviews?", 6, new Guid("b8c38056-0974-4493-920f-87ff018274cd") },
                    { new Guid("c40c756c-f117-4fa4-b7c6-a28f01eb6995"), "I'm new to programming and wondering which language I should start learning first. Any recommendations based on ease of learning and job opportunities?", new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2409), 3, false, new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2409), 0, "Best programming language for beginners?", 8, new Guid("ac7bf694-8d79-4b67-bc6c-117e661d75c9") },
                    { new Guid("cad5f124-3a9f-45fc-8553-669146dd08d3"), "I'm looking for tips and advice on how to become a better programmer. Any recommendations on books, online courses, or coding challenges would be greatly appreciated!", new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2391), 2, false, new DateTime(2024, 4, 13, 18, 26, 49, 97, DateTimeKind.Local).AddTicks(2402), 0, "How to improve programming skills?", 10, new Guid("fe6d58b6-c61e-45d1-a797-fc97b00ef640") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("4d64c300-950a-4f2b-867f-d3ed6f4f0e2c"));

            migrationBuilder.DeleteData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("613641bf-ea84-491a-902f-982db227cb1a"));

            migrationBuilder.DeleteData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("a924e31a-0d24-46e5-a3ca-80d31a1a445b"));

            migrationBuilder.DeleteData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("c40c756c-f117-4fa4-b7c6-a28f01eb6995"));

            migrationBuilder.DeleteData(
                table: "posts",
                keyColumn: "Id",
                keyValue: new Guid("cad5f124-3a9f-45fc-8553-669146dd08d3"));
        }
    }
}
