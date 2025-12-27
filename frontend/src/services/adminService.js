import axios from "axios";
import authService from "./authService";

const API_URL = "http://localhost:5199/api/Admin/";

const getAuthHeader = () => {
  const user = authService.getCurrentUser();
  if (user && user.accessToken) {
    return { Authorization: 'Bearer ' + user.accessToken }; 
  } else {
    return {};
  }
};

const getDashboardStats = async () => {
  const response = await axios.get(API_URL + "GetDashboardStats", {
    headers: getAuthHeader()
  });
  // Backend: SuccessDataResult { data: {...}, success: true, message: "..." }
  // Axios: response.data = SuccessDataResult
  // Bizim istediğimiz: response.data.data
  return response.data.data; 
};

const getAllUsers = async () => {
  const response = await axios.get(API_URL + "GetAllUsers", {
    headers: getAuthHeader()
  });
  // List<UserDto> verisi response.data.data içindedir
  return response.data.data;
};

const adminService = {
  getDashboardStats,
  getAllUsers,
};

export default adminService;