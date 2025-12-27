import React, { useEffect, useState } from "react";
import DashboardLayout from "../layouts/DashboardLayout";
import adminService from "../services/adminService";
import authService from "../services/authService"; // <-- Auth servisini eklemeyi unutma
import { toast } from "react-toastify";

const UserList = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false); // Varsayılan false yaptık, yetki varsa true yapacağız
  const [user, setUser] = useState(null); // Giriş yapan kullanıcı

  useEffect(() => {
    // 1. Önce kullanıcının kim olduğuna bak
    const currentUser = authService.getCurrentUser();
    setUser(currentUser);

    // 2. Sadece ve Sadece ADMIN ise Backend'e git
    if (currentUser && currentUser.role === 'Admin') {
        setLoading(true); // Yüklemeyi başlat
        adminService.getAllUsers()
          .then((data) => {
            setUsers(data || []); 
            setLoading(false);
          })
          .catch((error) => {
            console.error(error);
            toast.error("Liste yüklenemedi.");
            setLoading(false);
          });
    }
  }, []);

  return (
    <DashboardLayout>
      <div className="container-fluid">
        
        {/* GÜVENLİK KONTROLÜ: Sadece Admin ise tabloyu göster */}
        {user && user.role === 'Admin' ? (
            <>
                <h2 className="mb-4 fw-bold text-secondary">👥 Kullanıcı Listesi</h2>

                <div className="card shadow-sm">
                <div className="card-body">
                    {loading ? (
                    <div className="text-center py-4">
                        <div className="spinner-border text-primary"></div>
                        <p className="mt-2">Yükleniyor...</p>
                    </div>
                    ) : (
                    <div className="table-responsive">
                        <table className="table table-hover table-bordered align-middle">
                        <thead className="table-dark">
                            <tr>
                            <th>ID</th>
                            <th>Kullanıcı Adı</th>
                            <th>Email</th>
                            <th>Rol</th>
                            </tr>
                        </thead>
                        <tbody>
                            {users.length > 0 ? (
                            users.map((u) => (
                                <tr key={u.id || u.Id}>
                                <td><small>{u.id || u.Id}</small></td>
                                <td className="fw-bold">{u.username || u.Username}</td>
                                <td>{u.email || u.Email}</td>
                                <td>
                                    <span className={`badge ${ (u.role || u.Role) === 'Admin' ? 'bg-danger' : 'bg-info text-dark'}`}>
                                        {u.role || u.Role}
                                    </span>
                                </td>
                                </tr>
                            ))
                            ) : (
                                <tr><td colSpan="4" className="text-center">Kullanıcı bulunamadı.</td></tr>
                            )}
                        </tbody>
                        </table>
                    </div>
                    )}
                </div>
                </div>
            </>
        ) : (
            // --- YETKİSİZ GİRİŞ UYARISI ---
            // Admin olmayan biri URL'yi elle yazıp girerse burayı görür
            <div className="alert alert-danger text-center mt-5 shadow-sm">
                <h4>⛔ Erişim Engellendi</h4>
                <p>Bu sayfayı görüntülemek için Admin yetkisine sahip olmanız gerekmektedir.</p>
                
            </div>
        )}

      </div>
    </DashboardLayout>
  );
};

export default UserList;