import React, { useEffect, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom"; // <-- Link yerine NavLink import ettik
import authService from "../services/authService";
import { toast } from "react-toastify";

const Sidebar = () => {
  const navigate = useNavigate();
  const [user, setUser] = useState(null);

  useEffect(() => {
    const currentUser = authService.getCurrentUser();
    setUser(currentUser);
  }, []);

  const handleLogout = () => {
    authService.logout();
    toast.info("Görüşmek üzere 👋");
    navigate("/");
  };

  return (
    <div className="d-flex flex-column flex-shrink-0 p-3 text-white bg-dark" style={{ width: "250px", minHeight: "100vh" }}>
      <span className="fs-4 fw-bold text-center mb-4">
        🧙‍♂️ DevOps Wizard
      </span>
      <hr />
      <ul className="nav nav-pills flex-column mb-auto">
        
        {/* SADECE ADMIN GÖRÜR */}
        {user && user.role === 'Admin' && (
          <>
            <li className="nav-item">
              {/* NavLink: Otomatik olarak aktif sayfayı algılar */}
              <NavLink 
                to="/dashboard" 
                className={({ isActive }) => 
                  `nav-link text-white ${isActive ? "active" : ""}`
                }
              >
                📊 Sistem İstatistikleri
              </NavLink>
            </li>

            <li className="nav-item mt-2">
              <NavLink 
                to="/users" 
                className={({ isActive }) => 
                  `nav-link text-white ${isActive ? "active" : ""}`
                }
              >
                👥 Kullanıcı Listesi
              </NavLink>
            </li>
          </>
        )}

      </ul>
      <div className="mt-auto">
        <button onClick={handleLogout} className="btn btn-outline-danger w-100">
          🚪 Çıkış Yap
        </button>
      </div>
    </div>
  );
};

export default Sidebar;