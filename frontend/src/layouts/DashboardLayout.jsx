import React from "react";
import Sidebar from "../components/Sidebar";

const DashboardLayout = ({ children }) => {
  return (
    <div className="d-flex">
      {/* Sol taraf: Sabit Sidebar */}
      <Sidebar />
      
      {/* Sağ taraf: Değişen İçerik */}
      <div className="flex-grow-1 p-4 bg-light" style={{ minHeight: "100vh" }}>
        {children}
      </div>
    </div>
  );
};

export default DashboardLayout;