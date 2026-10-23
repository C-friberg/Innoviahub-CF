using api.Enums;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public static class IoTSensorSeed
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            // Negative keys keep demo seed rows separate from generated positive IDs.
            // GUIDs were resolved from our existing DeviceRegistry by serial.
            modelBuilder.Entity<Sensor>().HasData(
                new Sensor { SensorId = -101, Name = "dev-101", DeviceRegistryDeviceId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), ResourceId = 30, SensorType = SensorType.Temperature },
                new Sensor { SensorId = -102, Name = "dev-102", DeviceRegistryDeviceId = Guid.Parse("059de92e-8bdb-4586-99c3-a55a45a330cb"), ResourceId = 20, SensorType = SensorType.AirQuality },
                new Sensor { SensorId = -103, Name = "dev-103", DeviceRegistryDeviceId = Guid.Parse("2c7050b0-d442-4eae-baae-f6554a85e4ce"), ResourceId = 21, SensorType = SensorType.AirQuality },
                new Sensor { SensorId = -104, Name = "dev-104", DeviceRegistryDeviceId = Guid.Parse("002d3586-0e2c-4a3b-ac9a-9412cfd69840"), ResourceId = 22, SensorType = SensorType.AirQuality },
                new Sensor { SensorId = -105, Name = "dev-105", DeviceRegistryDeviceId = Guid.Parse("b2be7090-1b32-41b7-bd6f-5dfda293bb3b"), ResourceId = 23, SensorType = SensorType.AirQuality },
                new Sensor { SensorId = -106, Name = "dev-106", DeviceRegistryDeviceId = Guid.Parse("ddfd3e26-aeed-40f6-a3c4-511736b8a4b1"), ResourceId = 20, SensorType = SensorType.Temperature },
                new Sensor { SensorId = -107, Name = "dev-107", DeviceRegistryDeviceId = Guid.Parse("67a224c8-9835-4aa0-8cd4-1d9260479fc6"), ResourceId = 21, SensorType = SensorType.Temperature },
                new Sensor { SensorId = -108, Name = "dev-108", DeviceRegistryDeviceId = Guid.Parse("f115f39f-6f6a-427c-acbf-2fba8ec1282a"), ResourceId = 22, SensorType = SensorType.Temperature },
                new Sensor { SensorId = -109, Name = "dev-109", DeviceRegistryDeviceId = Guid.Parse("faad6f65-2036-45c0-a6a8-05c016575297"), ResourceId = 30, SensorType = SensorType.PowerConsumption },
                new Sensor { SensorId = -110, Name = "dev-110", DeviceRegistryDeviceId = Guid.Parse("f5debc98-4bbf-4cdd-9473-689a6172ce8e"), ResourceId = 20, SensorType = SensorType.Humidity },
                new Sensor { SensorId = -111, Name = "dev-111", DeviceRegistryDeviceId = Guid.Parse("30b49b57-50ab-4c76-9e64-15b9cb5ed532"), ResourceId = 21, SensorType = SensorType.Humidity },
                new Sensor { SensorId = -112, Name = "dev-112", DeviceRegistryDeviceId = Guid.Parse("f02aeac7-4b78-4a13-8154-f6c007cd514a"), ResourceId = 22, SensorType = SensorType.Humidity },
                new Sensor { SensorId = -113, Name = "dev-113", DeviceRegistryDeviceId = Guid.Parse("9e1f582c-91ef-4512-b859-d07387e07df1"), ResourceId = 23, SensorType = SensorType.Humidity }
            );
        }
    }
}