import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5199/api",
  withCredentials: true, // Bu sayede Cookie (Refresh Token) sunucuya gider/gelir
});

export default api;