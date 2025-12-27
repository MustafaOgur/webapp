import React, { useEffect, useState } from "react";
import DashboardLayout from "../layouts/DashboardLayout";
import authService from "../services/authService";
import adminService from "../services/adminService";
import { toast } from "react-toastify";

const Dashboard = () => {
  const [user, setUser] = useState(null);
  
  // Backend DTO yapısı
  const [stats, setStats] = useState({
    totalUsers: 0,
    totalChats: 0,
    totalMessages: 0,
    totalAiResponses: 0
  });
  
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const currentUser = authService.getCurrentUser();
    setUser(currentUser);

    // Sadece ADMIN ise verileri çek
    if (currentUser && currentUser.role === 'Admin') {
        setLoading(true);
        adminService.getDashboardStats()
            .then(data => {
                setStats(data); 
                setLoading(false);
            })
            .catch(err => {
                console.error("Stats error:", err);
                toast.error("İstatistikler yüklenirken hata oluştu.");
                setLoading(false);
            });
    }
  }, []);

  return (
    <DashboardLayout>
      <div className="container-fluid">
        
        {/* SADECE ADMIN İÇİN İÇERİK */}
        {user && user.role === 'Admin' ? (
             <div className="row">
              <h2 className="mb-4 fw-bold text-secondary">📊 Sistem İstatistikleri</h2>
                
                {/* 1. KART: KULLANICILAR */}
                <div className="col-md-3 mb-4">
                  <div className="card text-white bg-primary shadow h-100">
                    <div className="card-body">
                       <h6 className="card-title text-uppercase mb-0 opacity-75">Kullanıcılar</h6>
                       <h2 className="display-4 fw-bold my-2">{loading ? "-" : stats.totalUsers}</h2>
                       <p className="mb-0 small opacity-75">Toplam Kayıtlı</p>
                    </div>
                  </div>
                </div>
             
                {/* 2. KART: SOHBETLER */}
                <div className="col-md-3 mb-4">
                  <div className="card text-white bg-success shadow h-100">
                    <div className="card-body">
                       <h6 className="card-title text-uppercase mb-0 opacity-75">Sohbetler</h6>
                       <h2 className="display-4 fw-bold my-2">{loading ? "-" : stats.totalChats}</h2>
                       <p className="mb-0 small opacity-75">Oluşturulan Chat</p>
                    </div>
                  </div>
                </div>

                {/* 3. KART: MESAJLAR */}
                <div className="col-md-3 mb-4">
                  <div className="card text-dark bg-warning shadow h-100">
                    <div className="card-body">
                       <h6 className="card-title text-uppercase mb-0 opacity-75">Mesajlar</h6>
                       <h2 className="display-4 fw-bold my-2">{loading ? "-" : stats.totalMessages}</h2>
                       <p className="mb-0 small opacity-75">Toplam Mesaj</p>
                    </div>
                  </div>
                </div>

                {/* 4. KART: AI CEVAPLARI */}
                <div className="col-md-3 mb-4">
                  <div className="card text-white bg-info shadow h-100">
                    <div className="card-body">
                       <h6 className="card-title text-uppercase mb-0 opacity-75">AI Cevapları</h6>
                       <h2 className="display-4 fw-bold my-2">{loading ? "-" : stats.totalAiResponses}</h2>
                       <p className="mb-0 small opacity-75">Bot Yanıtları</p>
                    </div>
                  </div>
                </div>

           </div>
        ) : (
            // --- YETKİSİZ GİRİŞ UYARISI ---
            // Normal kullanıcı URL'yi elle yazıp girerse burayı görür
            <div className="alert alert-danger text-center mt-5 shadow-sm">
                <h4>⛔ Erişim Engellendi</h4>
                <p>Bu sayfayı görüntülemek için Admin yetkisine sahip olmanız gerekmektedir.</p>
            </div>
        )}
      </div>
    </DashboardLayout>
  );
};

export default Dashboard;