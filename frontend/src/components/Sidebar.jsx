import React, { useEffect, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import authService from "../services/authService";
import chatService from "../services/chatService";
import { toast } from "react-toastify";

// Modal Bileşeni (Aynı kalıyor)
const SimpleModal = ({ show, onClose, onConfirm, title, confirmText = "Oluştur", confirmColor = "btn-primary", children }) => {
    if (!show) return null;
    return (
        <div className="modal show d-block" style={{backgroundColor: 'rgba(0,0,0,0.5)', zIndex: 1050}}>
            <div className="modal-dialog modal-dialog-centered">
                <div className="modal-content text-dark">
                    <div className="modal-header">
                        <h5 className="modal-title">{title}</h5>
                        <button type="button" className="btn-close" onClick={onClose}></button>
                    </div>
                    <div className="modal-body">{children}</div>
                    <div className="modal-footer">
                        <button type="button" className="btn btn-secondary" onClick={onClose}>İptal</button>
                        <button type="button" className={`btn ${confirmColor}`} onClick={onConfirm}>{confirmText}</button>
                    </div>
                </div>
            </div>
        </div>
    );
};

const Sidebar = ({ onSelectChat, selectedChatId, refreshTrigger }) => {
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [chats, setChats] = useState([]); 

  // --- STATE YÖNETİMİ ---
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [newChatName, setNewChatName] = useState("");

  const [showEditModal, setShowEditModal] = useState(false);
  const [editingChat, setEditingChat] = useState(null);
  const [editChatName, setEditChatName] = useState("");

  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [deletingChat, setDeletingChat] = useState(null);

  // --- DÜZELTME 1: KULLANICIYI BAŞLANGIÇTA YÜKLE ---
  useEffect(() => {
    const currentUser = authService.getCurrentUser();
    setUser(currentUser);
  }, []); // Sadece sayfa ilk açıldığında çalışır

  // --- DÜZELTME 2: KULLANICI GELDİĞİNDE VEYA TRIGGER DEĞİŞTİĞİNDE CHATLERİ ÇEK ---
  useEffect(() => {
    // Kullanıcı varsa ve Admin değilse listeyi çek
    if (user && user.role !== 'Admin') {
        loadChats();
    }
  }, [user, refreshTrigger]); // user veya refreshTrigger değişirse çalışır

  const loadChats = () => {
      chatService.getAllChats()
        .then(data => setChats(data || []))
        .catch(err => console.error("Chatler yüklenemedi", err));
  };

  const handleLogout = () => {
    authService.logout();
    navigate("/");
  };

  // --- CREATE ---
  const handleCreateChat = async () => {
      if (!newChatName.trim()) return;
      try {
          const result = await chatService.createChat(newChatName);
          if (result.success) {
              toast.success("Sohbet oluşturuldu!");
              setNewChatName("");
              setShowCreateModal(false);
              loadChats(); 
              if (onSelectChat) onSelectChat(result.data.id);
          }
      } catch (error) {
          toast.error("Hata oluştu.");
      }
  };

  // --- UPDATE (RENAME) ---
  const openEditModal = (chat, e) => {
      e.stopPropagation(); 
      setEditingChat(chat);
      setEditChatName(chat.name);
      setShowEditModal(true);
  };

  const handleUpdateChat = async () => {
      if (!editChatName.trim() || !editingChat) return;
      try {
          const result = await chatService.updateChat(editingChat.id, editChatName);
          if (result.success) {
              toast.success("Sohbet güncellendi");
              setShowEditModal(false);
              loadChats();
          }
      } catch (error) {
          toast.error("Güncelleme başarısız");
      }
  };

  // --- DELETE ---
  const openDeleteModal = (chat, e) => {
      e.stopPropagation(); 
      setDeletingChat(chat);
      setShowDeleteModal(true);
  };

  const handleDeleteChat = async () => {
      if (!deletingChat) return;
      try {
          const result = await chatService.deleteChat(deletingChat.id);
          if (result.success) {
              toast.info("Sohbet silindi");
              setShowDeleteModal(false);
              
              if (selectedChatId === deletingChat.id) {
                  if (onSelectChat) onSelectChat(null);
                  navigate("/home"); 
              }
              loadChats();
          }
      } catch (error) {
          toast.error("Silme başarısız");
      }
  };

  return (
    <div className="d-flex flex-column flex-shrink-0 p-3 text-white bg-dark" style={{ width: "280px", height: "100vh" }}>
      
      {/* LOGO */}
      <div 
        className="d-flex align-items-center mb-3 mb-md-0 me-md-auto text-white text-decoration-none"
        style={{ cursor: user?.role !== 'Admin' ? 'pointer' : 'default' }}
        onClick={() => {
            if (user?.role !== 'Admin') {
                if (onSelectChat) onSelectChat(null); 
                navigate("/home"); 
            }
        }}
      >
        <span className="fs-4 fw-bold">🧙‍♂️ DevOps Wizard</span>
      </div>
      
      <hr />
      
      {/* NEW CHAT BUTTON */}
      {user && user.role !== 'Admin' && (
          <button 
            className="btn btn-primary w-100 mb-3 fw-bold"
            onClick={() => setShowCreateModal(true)}
          >
              + New Chat
          </button>
      )}

      {/* CHAT LIST */}
      <div className="flex-grow-1 overflow-auto custom-scrollbar mb-3">
        <ul className="nav nav-pills flex-column mb-auto">
            {/* Admin Linkleri */}
            {user && user.role === 'Admin' && (
             <>
                <li className="nav-item"><NavLink to="/dashboard" className="nav-link text-white">📊 İstatistikler</NavLink></li>
                <li className="nav-item"><NavLink to="/users" className="nav-link text-white">👥 Kullanıcılar</NavLink></li>
             </>
            )}

            {/* Kullanıcı Chatleri */}
            {user && user.role !== 'Admin' && chats.map(chat => (
                <li key={chat.id} className="nav-item mb-1">
                    <div 
                        className={`nav-link text-white d-flex justify-content-between align-items-center ${selectedChatId === chat.id ? "active bg-secondary" : ""}`}
                        style={{cursor: "pointer"}}
                        onClick={() => onSelectChat(chat.id)}
                    >
                        <div className="text-truncate me-2">
                             💬 {chat.name}
                        </div>

                        <div className="d-flex gap-1">
                            <button 
                                className="btn btn-sm btn-link text-white p-0 text-decoration-none opacity-50 hover-opacity-100" 
                                title="Düzenle"
                                onClick={(e) => openEditModal(chat, e)}
                            >
                                ✏️
                            </button>
                            <button 
                                className="btn btn-sm btn-link text-danger p-0 text-decoration-none opacity-75 hover-opacity-100 ms-1" 
                                title="Sil"
                                onClick={(e) => openDeleteModal(chat, e)}
                            >
                                🗑️
                            </button>
                        </div>
                    </div>
                </li>
            ))}
        </ul>
      </div>

      <div className="mt-auto pt-3 border-top">
        <div className="d-flex align-items-center justify-content-between">
            <small>{user?.username}</small>
            <button onClick={handleLogout} className="btn btn-sm btn-outline-danger">Çıkış</button>
        </div>
      </div>

      {/* --- MODALLAR --- */}
      <SimpleModal 
        show={showCreateModal} onClose={() => setShowCreateModal(false)} onConfirm={handleCreateChat} title="Yeni Sohbet Başlat"
      >
         <input type="text" className="form-control" placeholder="Sohbet Adı" value={newChatName} onChange={(e) => setNewChatName(e.target.value)} autoFocus />
      </SimpleModal>

      <SimpleModal 
        show={showEditModal} onClose={() => setShowEditModal(false)} onConfirm={handleUpdateChat} title="Sohbet Adını Düzenle" confirmText="Güncelle"
      >
         <input type="text" className="form-control" value={editChatName} onChange={(e) => setEditChatName(e.target.value)} autoFocus />
      </SimpleModal>

      <SimpleModal 
        show={showDeleteModal} onClose={() => setShowDeleteModal(false)} onConfirm={handleDeleteChat} title="Sohbeti Sil?" confirmText="Sil" confirmColor="btn-danger"
      >
         <p><b>{deletingChat?.name}</b> adlı sohbeti silmek istediğine emin misin?</p>
      </SimpleModal>

    </div>
  );
};

export default Sidebar;