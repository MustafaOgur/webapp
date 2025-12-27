import React, { useState } from "react";
import authService from "../services/authService";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";

const Login = () => {
  // Backend LoginDto ne bekliyorsa onu kullanmalısın. 
  // Genelde Email/Password olur.
  const [email, setEmail] = useState(""); 
  const [password, setPassword] = useState("");
  
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();

    try {
      // Servise verileri gönder
      await authService.login(email, password).then(
        () => {
          // 1. Giriş başarılı olunca hemen kullanıcının kim olduğuna bakıyoruz
          const user = authService.getCurrentUser();
          
          toast.success("Giriş Başarılı! Yönlendiriliyorsunuz... 🚀");
          
          // 1 saniye sonra Dashboard'a at
          setTimeout(() => {
           // 2. KONTROL NOKTASI 🚦
             if (user.role === 'Admin') {
                 navigate("/dashboard"); // Admin ise İstatistiklere git
             } else {
                 navigate("/home");      // Normal kullanıcı ise Ana Sayfaya git
             }
          }, 1000);
        },
        (error) => {
           // Backend'den gelen hata mesajını yakala
           const resMessage =
            (error.response &&
              error.response.data &&
              error.response.data.message) ||
            error.message ||
            error.toString();

          toast.error("Giriş Başarısız: " + resMessage);
          console.log(error);
        }
      );
    } catch (err) {
      console.log(err);
    }
  };

  return (
    <div className="container d-flex justify-content-center align-items-center vh-100 bg-light">
      <div className="card shadow-lg p-4" style={{ width: "400px" }}>
        <div className="text-center mb-4">
          <h2 className="text-primary fw-bold">Giriş Yap</h2>
          <p className="text-muted">Hesabınıza erişin</p>
        </div>
        
        <form onSubmit={handleLogin}>
          <div className="mb-3">
            <label className="form-label">Email</label>
            <input
              type="text" // LoginDto'da Username veya Email olabilir
              className="form-control"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          <div className="mb-3">
            <label className="form-label">Şifre</label>
            <input
              type="password"
              className="form-control"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <button type="submit" className="btn btn-primary w-100 py-2">
            Giriş Yap
          </button>
        </form>

        <div className="text-center mt-3">
            <small>Hesabın yok mu? <Link to="/register">Kayıt Ol</Link></small>
        </div>

      </div>
    </div>
  );
};

export default Login;