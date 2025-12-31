import { jwtDecode } from "jwt-decode";
import api from "./api"; // 1. ADIMDA OLUŞTURDUĞUMUZ AYARLI AXIOS'U ÇAĞIRIYORUZ

// Backend Endpointleri
const REGISTER_URL = "/User/Register";
const LOGIN_URL = "/User/Login";
const REFRESH_URL = "/RefreshToken/Refresh"; // Endpoint adresin (Başına http koymana gerek yok, api.js hallediyor)

const register = async (username, email, password) => {
  const response = await api.post(REGISTER_URL, {
    username,
    email,
    password,
  });
  return response.data;
};

const login = async (email, password) => {
  // api.post kullanıyoruz (withCredentials otomatik çalışıyor)
  const response = await api.post(LOGIN_URL, {
    email: email,
    password: password,
  });

  // Backend'den artık sadece AccessToken dönüyor (RefreshToken Cookie'de)
  const token = response.data.accessToken || response.data.AccessToken;

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
      // refreshToken: refreshToken, // <--- ARTIK BU SATIR YOK! GÜVENLİK İÇİN SİLDİK.
      username: name,
      role: normalizedRole,
      id: decodedToken.sub || decodedToken.nameid
    };

    // 4. Kaydet (Refresh token olmadan sadece user bilgisini saklıyoruz)
    localStorage.setItem("user", JSON.stringify(userObj));
  }

  return response.data;
};

const logout = () => {
  // İstersen backend'e logout isteği atıp cookie'yi sildirebilirsin (opsiyonel ama iyi olur)
  // api.post("/User/Logout"); 
  localStorage.removeItem("user");
};

const getCurrentUser = () => {
  return JSON.parse(localStorage.getItem("user"));
};

// --- GÜNCELLENEN REFRESHTOKEN FONKSİYONU ---
const refreshToken = async () => {
  // Artık LocalStorage'dan refresh token okumuyoruz.
  // Cookie'de olduğu için tarayıcı otomatik gönderecek.
  
  try {
    // Backend'e boş istek atıyoruz, o cookie'yi okuyacak.
    const response = await api.post(REFRESH_URL, {}); 

    const newAccessToken = response.data.AccessToken || response.data.accessToken;

    if (newAccessToken) {
      const user = getCurrentUser();
      
      if (user) {
        user.accessToken = newAccessToken;
        // user.refreshToken = ... // ARTIK YOK
        localStorage.setItem("user", JSON.stringify(user));
      }
      
      return { success: true, accessToken: newAccessToken };
    }
  } catch (error) {
    console.error("Token yenileme hatası (Cookie süresi dolmuş olabilir):", error);
    logout();
  }

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