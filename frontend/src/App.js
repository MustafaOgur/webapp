import React, { useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, useNavigate, Navigate } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import setupInterceptors from './services/setupInterceptors';
import authService from './services/authService';

// Sayfalar
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import UserList from './pages/UserList';
import Home from './pages/Home';

// Layout
import DashboardLayout from './layouts/DashboardLayout'; 

// Güvenlik
import ProtectedRoute from './components/ProtectedRoute'; 

const AppContent = () => {
  const navigate = useNavigate();

  useEffect(() => {
    setupInterceptors(navigate);
  }, [navigate]);

  // Kullanıcıyı Oku
  const user = authService.getCurrentUser();

  // --- ROL KONTROLÜ FONKSİYONU ---
  const getHomeRoute = () => {
    // Eğer rolü 'Admin' ise Dashboard'a gönder
    if (user?.role === 'Admin') {
      return "/dashboard";
    }
    // Değilse (User ise) Home'a gönder
    return "/home";
  };

  return (
    <Routes>
      {/* --- GİRİŞ / KAYIT (Doluysa Yönlendir) --- */}
      <Route 
        path="/" 
        element={user ? <Navigate to={getHomeRoute()} replace /> : <Login />} 
      />
      <Route 
        path="/register" 
        element={user ? <Navigate to={getHomeRoute()} replace /> : <Register />} 
      />
      
      {/* --- SIDEBAR'LI SAYFALAR --- */}
      <Route element={<DashboardLayout />}>
          
          <Route 
            path="/dashboard" 
            element={
              <ProtectedRoute>
                <Dashboard />
              </ProtectedRoute>
            } 
          />
          
          <Route 
            path="/users" 
            element={
              <ProtectedRoute>
                <UserList />
              </ProtectedRoute>
            } 
          />

          <Route 
            path="/home" 
            element={
              <ProtectedRoute>
                <Home />
              </ProtectedRoute>
            } 
          />

      </Route>

    </Routes>
  );
};

function App() {
  return (
    <Router>
      <ToastContainer position="top-right" autoClose={3000} />
      <AppContent /> 
    </Router>
  );
}

export default App;