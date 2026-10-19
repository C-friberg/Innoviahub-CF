import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import styles from "./css/DeviceOverview.module.css";

const API_URL = import.meta.env.VITE_API_URL;
const HUB_URL = "http://localhost:5103/hub/telemetry";
const TENANT = "innovia";

type Device = {
  id: string;
  serial: string;
  model: string;
  status: string;
};

type Measurement = {
  tenantSlug: string;
  deviceId: string;
  type: string;
  value: number;
  time: string;
};

const metricLabels: Record<string, { label: string; unit: string }> = {
  temperature: { label: "Temperatur", unit: "°C" },
  co2: { label: "CO₂", unit: "ppm" },
  humidity: { label: "Luftfuktighet", unit: "%" },
  powerconsumption: { label: "Effekt", unit: "W" },
};

export default function DeviceOverview() {
  const [devices, setDevices] = useState<Device[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [connectionStatus, setConnectionStatus] = useState("Ansluter...");
  const [measurements, setMeasurements] = useState<
    Record<string, Record<string, Measurement>>
  >({});

  useEffect(() => {
    const controller = new AbortController();

    async function fetchDevices() {
      try {
        const response = await fetch(`${API_URL}/api/devices`, {
          signal: controller.signal,
        });
        if (!response.ok) throw new Error("Kunde inte hämta enheterna.");
        const data: Device[] = await response.json();
        if (!controller.signal.aborted) setDevices(data);
      } catch (error) {
        if (!controller.signal.aborted) {
          setError(error instanceof Error ? error.message : "Ett fel uppstod.");
        }
      } finally {
        if (!controller.signal.aborted) setLoading(false);
      }
    }

    void fetchDevices();
    return () => controller.abort();
  }, []);

  useEffect(() => {
    let disposed = false;
    let retryTimer: ReturnType<typeof setTimeout> | undefined;
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(HUB_URL)
      .withAutomaticReconnect()
      .build();

    connection.on("measurementReceived", (measurement: Measurement) => {
      if (disposed || measurement.tenantSlug !== TENANT) return;
      setMeasurements(current => {
        const previous = current[measurement.deviceId]?.[measurement.type];
        if (previous && Date.parse(previous.time) > Date.parse(measurement.time)) {
          return current;
        }
        return {
          ...current,
          [measurement.deviceId]: {
            ...current[measurement.deviceId],
            [measurement.type]: measurement,
          },
        };
      });
    });

    async function joinTenant() {
      await connection.invoke("JoinTenant", TENANT);
      if (!disposed) setConnectionStatus("Ansluten till realtid");
    }

    async function startConnection() {
      try {
        await connection.start();
        if (!disposed) await joinTenant();
      } catch {
        if (!disposed) {
          setConnectionStatus("Ingen realtidsanslutning. Försöker igen...");
          await connection.stop();
          if (!disposed) retryTimer = setTimeout(() => void startConnection(), 5000);
        }
      }
    }

    connection.onreconnecting(() => {
      if (!disposed) setConnectionStatus("Återansluter — värden kan vara gamla");
    });
    connection.onreconnected(async () => {
      try {
        await joinTenant();
      } catch {
        if (!disposed) await connection.stop();
      }
    });
    connection.onclose(() => {
      if (!disposed) {
        setConnectionStatus("Frånkopplad — värden kan vara gamla");
        retryTimer = setTimeout(() => void startConnection(), 5000);
      }
    });

    void startConnection();
    return () => {
      disposed = true;
      clearTimeout(retryTimer);
      void connection.stop();
    };
  }, []);

  return (
    <section className={styles.overview} aria-labelledby="device-heading">
      <h2 id="device-heading">Sensoröversikt</h2>
      <p>{devices.length} registrerade enheter · {connectionStatus}</p>
      {loading && <p>Hämtar enheter...</p>}
      {error && <p role="alert">{error}</p>}
      {!loading && !error && devices.length === 0 && <p>Inga enheter registrerade.</p>}
      <div className={styles.cards}>
        {devices.map(device => (
          <article key={device.id} className={styles.card}>
            <h3>{device.serial}</h3>
            <p>{device.model} · {device.status}</p>
            {!measurements[device.id] && <p>Väntar på mätdata...</p>}
            {Object.values(measurements[device.id] ?? {}).map(measurement => {
              const metric = metricLabels[measurement.type];
              return (
                <div key={measurement.type} className={styles.measurement}>
                  <span>{metric?.label ?? measurement.type}</span>
                  <strong>
                    {measurement.value.toLocaleString("sv-SE", { maximumFractionDigits: 1 })}
                    {" "}{metric?.unit ?? ""}
                  </strong>
                  <small>
                    Mätt {new Date(measurement.time).toLocaleString("sv-SE")}
                  </small>
                </div>
              );
            })}
          </article>
        ))}
      </div>
    </section>
  );
}
