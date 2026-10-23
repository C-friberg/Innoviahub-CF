using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class LinkSensorsToResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeviceRegistryDeviceId",
                table: "Sensor",
                type: "uuid",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Sensor",
                columns: new[] { "SensorId", "DeviceRegistryDeviceId", "Name", "ResourceId", "SensorType" },
                values: new object[,]
                {
                    { -113, new Guid("9e1f582c-91ef-4512-b859-d07387e07df1"), "dev-113", 23, 5 },
                    { -112, new Guid("f02aeac7-4b78-4a13-8154-f6c007cd514a"), "dev-112", 22, 5 },
                    { -111, new Guid("30b49b57-50ab-4c76-9e64-15b9cb5ed532"), "dev-111", 21, 5 },
                    { -110, new Guid("f5debc98-4bbf-4cdd-9473-689a6172ce8e"), "dev-110", 20, 5 },
                    { -109, new Guid("faad6f65-2036-45c0-a6a8-05c016575297"), "dev-109", 30, 4 },
                    { -108, new Guid("f115f39f-6f6a-427c-acbf-2fba8ec1282a"), "dev-108", 22, 0 },
                    { -107, new Guid("67a224c8-9835-4aa0-8cd4-1d9260479fc6"), "dev-107", 21, 0 },
                    { -106, new Guid("ddfd3e26-aeed-40f6-a3c4-511736b8a4b1"), "dev-106", 20, 0 },
                    { -105, new Guid("b2be7090-1b32-41b7-bd6f-5dfda293bb3b"), "dev-105", 23, 3 },
                    { -104, new Guid("002d3586-0e2c-4a3b-ac9a-9412cfd69840"), "dev-104", 22, 3 },
                    { -103, new Guid("2c7050b0-d442-4eae-baae-f6554a85e4ce"), "dev-103", 21, 3 },
                    { -102, new Guid("059de92e-8bdb-4586-99c3-a55a45a330cb"), "dev-102", 20, 3 },
                    { -101, new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "dev-101", 30, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sensor_DeviceRegistryDeviceId",
                table: "Sensor",
                column: "DeviceRegistryDeviceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sensor_DeviceRegistryDeviceId",
                table: "Sensor");

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -113);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -112);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -111);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -110);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -109);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -108);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -107);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -106);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -105);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -104);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -103);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -102);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "SensorId",
                keyValue: -101);

            migrationBuilder.DropColumn(
                name: "DeviceRegistryDeviceId",
                table: "Sensor");
        }
    }
}
