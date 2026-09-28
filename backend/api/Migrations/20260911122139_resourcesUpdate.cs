using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class resourcesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 16);

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
                columns: new[] { "ResourceType", "TimeSlotTimeInHours" },
                values: new object[] { 1, 0 });

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 6,
                columns: new[] { "ResourceType", "TimeSlotTimeInHours" },
                values: new object[] { 1, 0 });

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 7,
                columns: new[] { "ResourceType", "TimeSlotTimeInHours" },
                values: new object[] { 1, 0 });

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 8,
                columns: new[] { "ResourceType", "TimeSlotTimeInHours" },
                values: new object[] { 1, 0 });

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 9,
                columns: new[] { "ResourceType", "TimeSlotTimeInHours" },
                values: new object[] { 1, 0 });

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 10,
                columns: new[] { "ResourceType", "TimeSlotTimeInHours" },
                values: new object[] { 1, 0 });

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 11,
                columns: new[] { "ResourceType", "TimeSlotTimeInHours" },
                values: new object[] { 1, 0 });

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 12,
                columns: new[] { "ResourceType", "TimeSlotTimeInHours" },
                values: new object[] { 1, 0 });

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 13,
                columns: new[] { "ResourceType", "TimeSlotTimeInHours" },
                values: new object[] { 1, 0 });

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 14,
                columns: new[] { "ResourceType", "TimeSlotTimeInHours" },
                values: new object[] { 1, 0 });

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 15,
                columns: new[] { "ResourceType", "TimeSlotTimeInHours" },
                values: new object[] { 1, 0 });

            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "ResourceId", "Capacity", "ResourceType", "TimeSlotTimeInHours" },
                values: new object[,]
                {
                    { 20, null, 2, 0 },
                    { 21, null, 2, 0 },
                    { 22, null, 2, 0 },
                    { 23, null, 2, 0 },
                    { 30, null, 4, 0 },
                    { 31, null, 4, 0 },
                    { 32, null, 4, 0 },
                    { 33, null, 4, 0 },
                    { 40, null, 3, 0 },
                    { 41, null, 3, 0 },
                    { 42, null, 3, 0 },
                    { 43, null, 3, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "huiohoi76786GY!");

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 30);

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

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 43);

            migrationBuilder.DropColumn(
                name: "TimeSlotTimeInHours",
                table: "Resources");

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 5,
                column: "ResourceType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 6,
                column: "ResourceType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 7,
                column: "ResourceType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 8,
                column: "ResourceType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 9,
                column: "ResourceType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 10,
                column: "ResourceType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 11,
                column: "ResourceType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 12,
                column: "ResourceType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 13,
                column: "ResourceType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 14,
                column: "ResourceType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: 15,
                column: "ResourceType",
                value: 3);

            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "ResourceId", "Capacity", "ResourceType" },
                values: new object[] { 16, null, 3 });
        }
    }
}
