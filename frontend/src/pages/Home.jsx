import React, { useEffect, useState } from "react";
import DashboardLayout from "../layouts/DashboardLayout";
import authService from "../services/authService";

const Home = () => {
  const [user, setUser] = useState(null);

  useEffect(() => {
    const currentUser = authService.getCurrentUser();
    setUser(currentUser);
  }, []);

  return (
    <DashboardLayout>
      <div className="container-fluid text-center mt-5">
        <div className="p-5 mb-4 bg-light rounded-3 shadow-sm">
          <div className="container-fluid py-5">
            <h1 className="display-5 fw-bold text-primary">DevOps Wizard'a Hoş Geldin! 🧙‍♂️</h1>
            {user && (
                <p className="col-md-8 fs-4 mx-auto mt-3">
                  Merhaba <b>{user.username}</b>, sistem şu an aktif.
                </p>
            )}
            <p className="text-muted">
                Sol menüyü kullanarak işlemlerine başlayabilirsin. (Şu an kullanıcı modundasın).
            </p>
            <button className="btn btn-primary btn-lg" type="button">Sohbet Başlat</button>
          </div>
        </div>
      </div>
    </DashboardLayout>
  );
};

export default Home;