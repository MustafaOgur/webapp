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

      // Eğer hata alınırsa, URL Login değilse ve Hata Kodu 401 ise
      if (originalConfig.url !== "/api/User/Login" && error.response) {
        
        // _retry bayrağı sonsuz döngüyü engeller
        if (error.response.status === 401 && !originalConfig._retry) {
          originalConfig._retry = true;

          try {
            // 1. Token yenilemeyi dene
            const result = await authService.refreshToken();
            
            // Eğer authService { success: true, accessToken: '...' } dönerse
            if (result.success && result.accessToken) {
              
              // 2. Axios headerlarını güncelle (Gelecek istekler için)
              axios.defaults.headers.common['Authorization'] = 'Bearer ' + result.accessToken;

              // 3. Başarısız olan isteğin header'ını güncelle
              originalConfig.headers['Authorization'] = 'Bearer ' + result.accessToken;
              
              // 4. İsteği tekrarla
              return axios(originalConfig);
            }
          } catch (_error) {
            // Token yenileme sırasında kritik hata oluştu
            authService.logout();
            navigate("/"); 
            return Promise.reject(_error);
          }
        }
      }

      // Eğer 401 değilse veya yenileme başarısızsa hatayı fırlat
      return Promise.reject(error);
    }
  );
};

export default setupInterceptors;