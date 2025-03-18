using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Intaker.TaskManagementSystem.infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AssignedTo", "CreatedAt", "CreatedBy", "Description", "Name", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "User 1", new DateTimeOffset(new DateTime(2025, 3, 18, 18, 7, 49, 770, DateTimeKind.Unspecified).AddTicks(2695), new TimeSpan(0, 0, 0, 0, 0)), "System", "Task 1 Description", "Task 1", "NotStarted", new DateTimeOffset(new DateTime(2025, 3, 18, 18, 7, 49, 770, DateTimeKind.Unspecified).AddTicks(2695), new TimeSpan(0, 0, 0, 0, 0)), "System" },
                    { 2, "User 2", new DateTimeOffset(new DateTime(2025, 3, 18, 18, 7, 49, 770, DateTimeKind.Unspecified).AddTicks(2695), new TimeSpan(0, 0, 0, 0, 0)), "System", "Task 2 Description", "Task 2", "NotStarted", new DateTimeOffset(new DateTime(2025, 3, 18, 18, 7, 49, 770, DateTimeKind.Unspecified).AddTicks(2695), new TimeSpan(0, 0, 0, 0, 0)), "System" },
                    { 3, "User 3", new DateTimeOffset(new DateTime(2025, 3, 18, 18, 7, 49, 770, DateTimeKind.Unspecified).AddTicks(2695), new TimeSpan(0, 0, 0, 0, 0)), "System", "Task 3 Description", "Task 3", "NotStarted", new DateTimeOffset(new DateTime(2025, 3, 18, 18, 7, 49, 770, DateTimeKind.Unspecified).AddTicks(2695), new TimeSpan(0, 0, 0, 0, 0)), "System" },
                    { 4, "User 4", new DateTimeOffset(new DateTime(2025, 3, 18, 18, 7, 49, 770, DateTimeKind.Unspecified).AddTicks(2695), new TimeSpan(0, 0, 0, 0, 0)), "System", "Task 4 Description", "Task 4", "NotStarted", new DateTimeOffset(new DateTime(2025, 3, 18, 18, 7, 49, 770, DateTimeKind.Unspecified).AddTicks(2695), new TimeSpan(0, 0, 0, 0, 0)), "System" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Name",
                table: "Tasks",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
