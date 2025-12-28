import axios from "axios";
import authService from "./authService";

const API_URL = "http://localhost:5199/api/";

// Helper: Token Header Ekle
const getAuthHeader = () => {
  const user = authService.getCurrentUser();
  if (user && user.accessToken) {
    return { Authorization: 'Bearer ' + user.accessToken }; 
  }
  return {};
};

// --- CHAT İŞLEMLERİ ---

const createChat = async (name) => {
  const response = await axios.post(API_URL + "Chat/CreateChat", 
    { name: name }, 
    { headers: getAuthHeader() }
  );
  return response.data;
};

const getAllChats = async () => {
  try {
    const response = await axios.get(API_URL + "Chat/GetAllChats", {
      headers: getAuthHeader()
    });
    return response.data.data || [];
  } catch (error) {
    return [];
  }
};

const updateChat = async (id, name) => {
    // Backend UpdateChatDto bekliyor: { id, name }
    const response = await axios.post(API_URL + "Chat/UpdateChat", 
        { id: id, name: name }, 
        { headers: getAuthHeader() }
    );
    return response.data;
};

const deleteChat = async (id) => {
    // Backend Query String bekliyor: DeleteChat?id=...
    const response = await axios.delete(API_URL + `Chat/DeleteChat?id=${id}`, {
        headers: getAuthHeader()
    });
    return response.data;
};

// --- GEÇMİŞ VE MESAJLAŞMA ---

const getChatHistory = async (chatId) => {
    try {
        const response = await axios.get(API_URL + `Chat/GetChatHistory?chatId=${chatId}`, {
            headers: getAuthHeader()
        });
        return response.data.data || [];
    } catch (error) {
        // Hata olursa boş dizi dön ki .map() patlamasın
        return [];
    }
};

const sendMessage = async (chatId, content) => {
    // DTO: { chatId, content }
    const payload = {
        chatId: chatId,
        content: content
    };

    const response = await axios.post(API_URL + "Message/AddMessage", payload, {
        headers: getAuthHeader()
    });
    return response.data; 
};

const generateResponse = async (messageId) => {
    const response = await axios.post(API_URL + `Response/AddResponse?messageId=${messageId}`, {}, {
        headers: getAuthHeader()
    });
    return response.data;
};

const chatService = {
  createChat,
  getAllChats,
  updateChat,
  deleteChat,
  getChatHistory,
  sendMessage,
  generateResponse
};

export default chatService;