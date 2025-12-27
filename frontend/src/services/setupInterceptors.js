import axios from "axios";
import authService from "./authService";

const setupInterceptors = (navigate) => {
  
  axios.interceptors.response.use(
    (response) => {
      // Cevap başarılıysa aynen devam et
      return response;
    },
    async (error) => {
      const originalConfig = error.config;

      // Hata alırsak ve bu hata 401 ise (Yetkisiz) 
      // VE daha önce retry yapmadıysak (sonsuz döngü olmasın diye)
      if (originalConfig.url !== "/api/User/Login" && error.response) {
        if (error.response.status === 401 && !originalConfig._retry) {
          originalConfig._retry = true;

          try {
            // 1. Token yenilemeyi dene
            const rs = await authService.refreshToken();
            
            if (rs && rs.accessToken) {
              // 2. Yeni token'ı header'a koy
              // Backend 'AccessToken' dönüyor ama biz bearer olarak eklerken değişkene bakıyoruz
              // authService zaten localStorage'ı güncelledi.
              
              // 3. Başarısız olan isteğin header'ını güncelle
              originalConfig.headers['Authorization'] = 'Bearer ' + rs.accessToken;
              
              // 4. İsteği tekrarla (Sanki hiç hata olmamış gibi)
              return axios(originalConfig);
            }
          } catch (_error) {
            // Token yenilenemedi (Refresh token süresi de bitmiş)
            // Çıkış yap ve Login'e at
            authService.logout();
            navigate("/"); 
            return Promise.reject(_error);
          }
        }
      }

      return Promise.reject(error);
    }
  );
};

export default setupInterceptors;