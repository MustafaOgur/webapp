import api from "./api"; // Kendi oluşturduğumuz api.js
import authService from "./authService";

const setupInterceptors = (navigate) => {
  
  // 1. İSTEK (REQUEST) INTERCEPTOR: Her isteğe Access Token'ı otomatik ekle
  api.interceptors.request.use(
    (config) => {
      const user = authService.getCurrentUser();
      if (user && user.accessToken) {
        config.headers["Authorization"] = "Bearer " + user.accessToken;
      }
      return config;
    },
    (error) => Promise.reject(error)
  );

  // 2. CEVAP (RESPONSE) INTERCEPTOR: 401 Hatalarını Yakala
  api.interceptors.response.use(
    (response) => {
      return response;
    },
    async (error) => {
      const originalConfig = error.config;

      // Hata 401 ise ve daha önce denememişsek
      if (originalConfig.url !== "/User/Login" && error.response) {
        if (error.response.status === 401 && !originalConfig._retry) {
          originalConfig._retry = true;

          try {
            // Token yenile (Cookie otomatik gidecek)
            const result = await authService.refreshToken();

            if (result.success && result.accessToken) {
              // Yeni token'ı header'a ekle ve isteği tekrarla
              originalConfig.headers["Authorization"] = "Bearer " + result.accessToken;
              return api(originalConfig);
            }
          } catch (_error) {
            // Yenileme başarısızsa (Cookie süresi dolmuşsa) çıkış yap
            authService.logout();
            if (navigate) navigate("/");
            return Promise.reject(_error);
          }
        }
      }

      return Promise.reject(error);
    }
  );
};

export default setupInterceptors;