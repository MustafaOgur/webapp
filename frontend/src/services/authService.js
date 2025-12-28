import axios from "axios";
import { jwtDecode } from "jwt-decode"; 

// Backend Portun: 5199
const API_URL = "http://localhost:5199/api/User/"; 
const REFRESH_URL = "http://localhost:5199/api/RefreshToken/Refresh"; // Endpoint adresin

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

  const token = response.data.accessToken || response.data.AccessToken;
  const refreshToken = response.data.refreshToken || response.data.RefreshToken;

  if (token) {
    const decodedToken = jwtDecode(token);

    // 1. Rolü bul
    let rawRole = 
        decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || 
        decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role"] ||    
        decodedToken["role"] || 
        "User";

    // 2. Rolü Standartlaştır
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
  }
  
  return response.data;
};

const logout = () => {
  localStorage.removeItem("user");
};

const getCurrentUser = () => {
  return JSON.parse(localStorage.getItem("user"));
};

// --- GÜNCELLENEN REFRESHTOKEN FONKSİYONU ---
const refreshToken = async () => {
  const user = getCurrentUser();
  
  // Eğer kullanıcı veya refresh token yoksa başarısız dön
  if (!user || !user.refreshToken) return { success: false };

  try {
    // Backend'e istek at
    const response = await axios.post(REFRESH_URL, {
      token: user.refreshToken 
    });

    // Backend 'AccessToken' veya 'data.AccessToken' dönebilir, kontrol et
    const newAccessToken = response.data.AccessToken || response.data.accessToken;
    const newRefreshToken = response.data.RefreshToken || response.data.refreshToken;

    if (newAccessToken) {
      // 1. User objesini güncelle
      user.accessToken = newAccessToken;
      // Eğer backend yeni refresh token da dönüyorsa onu da güncelle (Best Practice)
      if (newRefreshToken) {
          user.refreshToken = newRefreshToken;
      }
      
      // 2. LocalStorage'ı güncelle
      localStorage.setItem("user", JSON.stringify(user));
      
      // 3. Başarılı olduğunu ve yeni token'ı dön
      return { success: true, accessToken: newAccessToken };
    }
  } catch (error) {
    console.error("Token yenileme hatası:", error);
    // Hata varsa (Refresh token süresi dolmuşsa) çıkış yap
    logout(); 
  }
  
  // Herhangi bir sorun varsa başarısız dön
  return { success: false };
};

const authService = {
  register,
  login,
  logout,
  getCurrentUser,
  refreshToken,
};

export default authService;