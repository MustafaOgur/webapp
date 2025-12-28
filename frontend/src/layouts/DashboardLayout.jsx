import React, { useState } from "react";
import Sidebar from "../components/Sidebar";
import { Outlet } from "react-router-dom";

const DashboardLayout = ({ children }) => {
  const [selectedChatId, setSelectedChatId] = useState(null);
  
  // --- YENİ: Sidebar Yenileme Tetikleyicisi ---
  const [refreshSidebar, setRefreshSidebar] = useState(0); 

  const handleChatSelect = (id) => {
    setSelectedChatId(id);
  };

  // Home bileşeni bu fonksiyonu çağırınca Sidebar yenilenecek
  const triggerSidebarRefresh = () => {
      setRefreshSidebar(prev => prev + 1);
  };

  return (
    <div className="d-flex" style={{ height: "100vh", overflow: "hidden" }}>
      {/* Sidebar'a refresh tetikleyicisini gönderiyoruz */}
      <Sidebar 
        onSelectChat={handleChatSelect} 
        selectedChatId={selectedChatId} 
        refreshTrigger={refreshSidebar} // <--- EKLENDİ
      />
      
      <div className="flex-grow-1 d-flex flex-column" style={{ height: "100vh", overflow: "hidden" }}>
        {/* Home'a "triggerSidebarRefresh" fonksiyonunu gönderiyoruz */}
        <Outlet context={{ selectedChatId, setSelectedChatId, triggerSidebarRefresh }} />
        
        {!children && <div className="d-none">Outlet Used</div>}
        {children}
      </div>
    </div>
  );
};

export default DashboardLayout;