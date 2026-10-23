import { useState } from "react";
import Navbar from "../components/Navbar";
import styles from "./css/AdminPage.module.css";
import UserList from "../components/UserList";
import AdminBookings from "../components/AdminBookings";
import ResourceStatus from "../components/ResourceStatus";
import RegisterUser from "../components/RegisterUser";
import DeviceOverview from "../components/DeviceOverview";

export default function AdminPage() {
  const [showRegister, setShowRegister] = useState(false);
  const [activeView, setActiveView] = useState<"administration" | "sensors">(
    "administration",
  );

  return (
    <>
      <Navbar />
      <main className={styles.adminPage}>
        <nav className={styles.viewTabs} aria-label="Adminvyer">
          <button
            type="button"
            className={styles.viewTab}
            aria-pressed={activeView === "administration"}
            onClick={() => {
              setActiveView("administration");
              setShowRegister(false);
            }}
          >
            Administration
          </button>
          <button
            type="button"
            className={styles.viewTab}
            aria-pressed={activeView === "sensors"}
            onClick={() => {
              setActiveView("sensors");
              setShowRegister(false);
            }}
          >
            Sensoröversikt
          </button>
        </nav>

        {activeView === "sensors" ? (
          <div className={styles.sensorView}>
            <DeviceOverview />
          </div>
        ) : showRegister ? (
          <RegisterUser onBack={() => setShowRegister(false)} />
        ) : (
          <>
            <button
              className={styles.registerButton}
              type="button"
              onClick={() => setShowRegister(true)}
            >
              Registrera användare
            </button>

            <div className={styles.adminBookingViewWrapper}>
              <AdminBookings />
            </div>

            <div className={styles.resourceStatusAndUserListWrapper}>
              <ResourceStatus />
              <UserList />
            </div>
          </>
        )}
      </main>
    </>
  );
}
