import axios from "axios";
import { jwtDecode } from "jwt-decode"; 

// Backend Portun: 5199
const API_URL = "http://localhost:5199/api/User/"; 

const register = async (username, email, password) => {
  const response = await axios.post(API_URL + "Register", {
    username,
    email,
    password,
  });
  return response.data;
};

const login = async (email, password) => {
  const response = await axios.post(API_URL + "Login", {
    email: email,      
    password: password,
  });

  // DÜZELTME BURADA: Hem küçük 'a' hem büyük 'A' kontrol ediyoruz.
  // .NET genelde 'accessToken' gönderir, biz işimizi sağlama alalım.
  const token = response.data.accessToken || response.data.AccessToken;
  const refreshToken = response.data.refreshToken || response.data.RefreshToken;

  if (token) {
    const decodedToken = jwtDecode(token);

    // 1. Rolü bul (Uzun URL veya kısa 'role' key)
    let rawRole = 
        decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || 
        decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role"] ||    
        decodedToken["role"] || 
        "User";

    // 2. Rolü Standartlaştır (admin -> Admin)
    const normalizedRole = rawRole.toString().charAt(0).toUpperCase() + rawRole.toString().slice(1).toLowerCase();

    // 3. İsim bul
    const name = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || decodedToken.unique_name || email;

    const userObj = {
      accessToken: token,
      refreshToken: refreshToken,
      username: name,
      role: normalizedRole, 
      id: decodedToken.sub || decodedToken.nameid
    };

    // 4. Kaydet
    localStorage.setItem("user", JSON.stringify(userObj));
    console.log("Giriş Başarılı! Kaydedilen Kullanıcı:", userObj); // Kontrol için konsola yazdırıyoruz
  } else {
    console.error("Token bulunamadı! Gelen veri:", response.data);
  }
  
  return response.data;
};

const logout = () => {
  localStorage.removeItem("user");
};

const getCurrentUser = () => {
  return JSON.parse(localStorage.getItem("user"));
};

const refreshToken = async () => {
  const user = JSON.parse(localStorage.getItem("user"));
  
  if (user && user.refreshToken) {
    try {
      // Backend'deki endpoint'e istek atıyoruz
      const response = await axios.post("http://localhost:5199/api/RefreshToken/Refresh", {
        token: user.refreshToken // DTO'ndaki isme göre (Token)
      });

      if (response.data.AccessToken) {
        // Yeni tokenları kullanıcı objesine güncelle
        user.accessToken = response.data.AccessToken;
        user.refreshToken = response.data.RefreshToken;
        
        // LocalStorage'ı güncelle
        localStorage.setItem("user", JSON.stringify(user));
        
        return user;
      }
    } catch (error) {
      console.error("Token yenileme hatası:", error);
      // Refresh token da geçersizse kullanıcıyı tamamen çıkarmalıyız
      logout(); 
    }
  }
  return null;
};


const authService = {
  register,
  login,
  logout,
  getCurrentUser,
  refreshToken,
};

export default authService;

