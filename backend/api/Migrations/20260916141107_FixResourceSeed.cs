using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class FixResourceSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "huiohoi76786GY!");

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 33);

            migrationBuilder.DropColumn(
                name: "TimeSlotTimeInHours",
                table: "Resources");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeSlotTimeInHours",
                table: "Resources",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "huiohoi76786GY!", 0, "6c4efa7a-7b5c-4213-817b-ce4626e0a4a0", null, false, false, null, null, null, null, null, false, "446434ad-ae47-4e37-9d1d-d1bbc9fbbc58", false, null });

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 1,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 2,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 3,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 4,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 5,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 6,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 7,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 8,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 9,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 10,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 11,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 12,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 13,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 14,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 15,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 20,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 21,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 22,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 23,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 30,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 40,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 41,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 42,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 43,
                column: "TimeSlotTimeInHours",
                value: 0);

            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "ResourceId", "Capacity", "ResourceType", "TimeSlotTimeInHours" },
                values: new object[,]
                {
                    { 31, null, 4, 0 },
                    { 32, null, 4, 0 },
                    { 33, null, 4, 0 }
                });
        }
    }
}
