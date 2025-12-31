import api from "./api"; // api.js kullan

// URL'leri /Admin/ ile başlatıyoruz, baseURL zaten tanımlı.

const getDashboardStats = async () => {
  const response = await api.get("/Admin/GetDashboardStats");
  // Interceptor header'ı eklediği için manuel eklemeye gerek yok.
  return response.data.data;
};

const getAllUsers = async () => {
  const response = await api.get("/Admin/GetAllUsers");
  return response.data.data;
};

const adminService = {
  getDashboardStats,
  getAllUsers,
};

export default adminService;