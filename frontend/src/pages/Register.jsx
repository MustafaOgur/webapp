import React, { useState } from "react";
import authService from "../services/authService";
import { toast } from "react-toastify";
import { useNavigate, Link } from "react-router-dom";

const Register = () => {
  // Sadece DTO'da olanlar
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();

    try {
      // Name parametresi yok, sadece 3 veri gidiyor
      await authService.register(username, email, password).then(
        (response) => {
          toast.success("Kayıt Başarılı! Giriş yapabilirsiniz. 🎉");
          setTimeout(() => {
            navigate("/"); 
          }, 2000);
        },
        (error) => {
          const resMessage =
            (error.response &&
              error.response.data &&
              error.response.data.message) ||
            error.message ||
            error.toString();

          toast.error("Kayıt Başarısız: " + resMessage);
        }
      );
    } catch (err) {
      console.log(err);
    }
  };

  return (
    <div className="container d-flex justify-content-center align-items-center vh-100 bg-light">
      <div className="card shadow-lg p-4" style={{ width: "450px" }}>
        <div className="text-center mb-4">
          <h2 className="text-primary fw-bold">Kayıt Ol</h2>
          <p className="text-muted">DevOps Wizard ailesine katıl</p>
        </div>
        
        <form onSubmit={handleRegister}>
          {/* Ad Soyad SİLİNDİ */}

          {/* Kullanıcı Adı */}
          <div className="mb-3">
            <label className="form-label">Kullanıcı Adı</label>
            <input
              type="text"
              className="form-control"
              placeholder="Kullanıcı adı seçin"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>

          {/* Email */}
          <div className="mb-3">
            <label className="form-label">Email Adresi</label>
            <input
              type="email"
              className="form-control"
              placeholder="ornek@email.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          {/* Şifre */}
          <div className="mb-3">
            <label className="form-label">Şifre</label>
            <input
              type="password"
              className="form-control"
              placeholder="********"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <button type="submit" className="btn btn-success w-100 py-2">
            Kayıt Ol
          </button>
        </form>

        <div className="text-center mt-3">
            <small>Zaten hesabın var mı? <Link to="/">Giriş Yap</Link></small>
        </div>
      </div>
    </div>
  );
};

export default Register;