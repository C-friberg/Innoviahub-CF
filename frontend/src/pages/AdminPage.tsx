import { useState } from "react";
import Navbar from "../components/Navbar";
import styles from "./css/AdminPage.module.css";
import UserList from "../components/UserList";
import AdminBookings from "../components/AdminBookings";
import ResourceStatus from "../components/ResourceStatus";
import RegisterUser from "../components/RegisterUser";

export default function LandingPage() {
  const [showRegister, setShowRegister] = useState(false);

  return (
    <>
      <Navbar />
      <main className={styles.adminPage}>
        {showRegister ? (
          <RegisterUser onBack={() => setShowRegister(false)} />
        ) : (
          <>
            <button className={styles.registerButton} type="button" onClick={() => setShowRegister(true)}>
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
