import React from "react";
import { Navigate } from "react-router-dom";
import authService from "../services/authService";

const ProtectedRoute = ({ children }) => {
  const user = authService.getCurrentUser();

  // Eğer kullanıcı yoksa (Giriş yapmamışsa)
  if (!user) {
    // Login sayfasına (/) şutla
    // replace: Geri tuşuna basınca tekrar bu sayfaya gelmesini engeller
    return <Navigate to="/" replace />;
  }

  // Kullanıcı varsa, istediği sayfayı (children) göster
  return children;
};

export default ProtectedRoute;