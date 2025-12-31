import api from "./api"; // Sadece bunu import etmen yeterli

// --- CHAT İŞLEMLERİ ---

const createChat = async (name) => {
  // URL'in başını yazmıyoruz, api.js hallediyor. Header otomatik ekleniyor.
  const response = await api.post("/Chat/CreateChat", { name: name });
  return response.data;
};

const getAllChats = async () => {
  try {
    const response = await api.get("/Chat/GetAllChats");
    return response.data.data || [];
  } catch (error) {
    return [];
  }
};

const updateChat = async (id, name) => {
  const response = await api.post("/Chat/UpdateChat", { id: id, name: name });
  return response.data;
};

const deleteChat = async (id) => {
  const response = await api.delete(`/Chat/DeleteChat?id=${id}`);
  return response.data;
};

// --- GEÇMİŞ VE MESAJLAŞMA ---

const getChatHistory = async (chatId) => {
  try {
    const response = await api.get(`/Chat/GetChatHistory?chatId=${chatId}`);
    return response.data.data || [];
  } catch (error) {
    return [];
  }
};

const sendMessage = async (chatId, content) => {
  const payload = {
    chatId: chatId,
    content: content,
  };
  const response = await api.post("/Message/AddMessage", payload);
  return response.data;
};

const generateResponse = async (messageId) => {
  const response = await api.post(`/Response/AddResponse?messageId=${messageId}`, {});
  return response.data;
};

const chatService = {
  createChat,
  getAllChats,
  updateChat,
  deleteChat,
  getChatHistory,
  sendMessage,
  generateResponse,
};

export default chatService;