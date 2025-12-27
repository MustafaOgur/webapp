import React, { useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, useNavigate } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import setupInterceptors from './services/setupInterceptors';

// Sayfalar
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import UserList from './pages/UserList';
import Home from './pages/Home';

// YENİ: Güvenlik Görevlisi
import ProtectedRoute from './components/ProtectedRoute'; 

const AppContent = () => {
  const navigate = useNavigate();

  useEffect(() => {
    setupInterceptors(navigate);
  }, [navigate]);

  return (
    <Routes>
      {/* --- HALKA AÇIK SAYFALAR --- */}
      <Route path="/" element={<Login />} />
      <Route path="/register" element={<Register />} />
      
      {/* --- KORUMALI SAYFALAR (Sadece Giriş Yapanlar) --- */}
      {/* ProtectedRoute içine aldığımız her şey, Sidebar dahil korunur */}
      
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