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

// Layout
import DashboardLayout from './layouts/DashboardLayout'; 

// Güvenlik
import ProtectedRoute from './components/ProtectedRoute'; 

const AppContent = () => {
  const navigate = useNavigate();

  // Interceptor'ı burada kuruyoruz, navigate fonksiyonunu içine atıyoruz
  useEffect(() => {
    setupInterceptors(navigate);
  }, [navigate]);

  return (
    <Routes>
      {/* --- HALKA AÇIK SAYFALAR --- */}
      <Route path="/" element={<Login />} />
      <Route path="/register" element={<Register />} />
      
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