import React, { useEffect, useState, useRef } from "react";
import { useOutletContext } from "react-router-dom"; 
import chatService from "../services/chatService";
import { toast } from "react-toastify";
import MarkdownRenderer from "../components/MarkdownRenderer"; // Bileşeni import ettik

const Home = () => {
  // triggerSidebarRefresh'i context'ten alıyoruz
  const { selectedChatId, setSelectedChatId, triggerSidebarRefresh } = useOutletContext() || {}; 
  
  const [history, setHistory] = useState([]); 
  const [inputMessage, setInputMessage] = useState("");
  const [loading, setLoading] = useState(false);
  const messagesEndRef = useRef(null);

  // Chat ID değişince veriyi çek
  useEffect(() => {
    if (selectedChatId) {
      loadHistory(selectedChatId);
    } else {
        setHistory([]); 
    }
  }, [selectedChatId]);

  // Yeni mesaj gelince en alta kaydır
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [history]);

  const loadHistory = async (chatId) => {
    setLoading(true);
    try {
        const response = await chatService.getChatHistory(chatId);
        setHistory(response || []);
    } catch (error) {
        console.error(error);
    } finally {
        setLoading(false);
    }
  };

    const handleDownload = (content, extension, index) => {
      if (!content || !extension) return;

      // --- YENİ KISIM: KOD AYIKLAMA ---
      // Regex Mantığı: ``` ile başlayan ve biten alanı bul, içini (Group 1) al.
      // (?:[\w]*\n)? => ```yaml veya ```json gibi dil tanımlarını ve alt satıra geçişi atla.
      // ([\s\S]*?)   => Kodun kendisini yakala.
      const codeBlockRegex = /```(?:[\w]*\n)?([\s\S]*?)```/;
      const match = content.match(codeBlockRegex);

      // Eğer kod bloğu bulunduysa (match[1]) onu kullan, bulunamazsa (düz metinse) hepsini indir.
      let fileContent = match ? match[1] : content;
      
      // Başındaki ve sonundaki gereksiz boşlukları temizle
      fileContent = fileContent.trim();
      // --------------------------------

      const safeExtension = extension.startsWith('.') ? extension : `.${extension}`;
      const fileName = `devops-wizard-output-${index}${safeExtension}`;
      
      const blob = new Blob([fileContent], { type: 'text/plain' });
      const url = window.URL.createObjectURL(blob);
      
      const a = document.createElement('a');
      a.href = url;
      a.download = fileName;
      document.body.appendChild(a);
      a.click();
      
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
  };

  const handleSendMessage = async () => {
    if (!inputMessage.trim()) return;
    
    const tempItem = { userMessage: inputMessage, aiResponse: null, isTemp: true };
    setHistory(prev => [...prev, tempItem]);
    setInputMessage("");

    try {
        let currentChatId = selectedChatId;

        // Backend'e mesajı gönder
        const msgResult = await chatService.sendMessage(currentChatId, tempItem.userMessage);
        
        if (msgResult.success) {
            const newMessageData = msgResult.data;

            // Eğer yeni chat oluşturulduysa (currentChatId null idi)
            if (!currentChatId && newMessageData.chatId) {
                setSelectedChatId(newMessageData.chatId);
                currentChatId = newMessageData.chatId;

                // Sidebar'ı yenile
                if (triggerSidebarRefresh) triggerSidebarRefresh();
            }

            // AI Cevabını Tetikle
            await chatService.generateResponse(newMessageData.id);
            
            // Geçmişi Yenile
            loadHistory(currentChatId);
        }
    } catch (error) {
        console.error(error);
        toast.error("Mesaj gönderilemedi");
    }
  };

  return (
    <div className="d-flex flex-column h-100 bg-white">
      {/* ÜST BAŞLIK */}
      <div className="p-3 border-bottom bg-light d-flex justify-content-between align-items-center">
         <h5 className="m-0 text-secondary">
             {selectedChatId ? "💬 Sohbet Geçmişi" : "✨ Yeni Sohbet Başlat"}
         </h5>
      </div>

      {/* MESAJ ALANI */}
      <div className="flex-grow-1 p-4 overflow-auto" style={{ height: "calc(100vh - 140px)" }}>
        {!selectedChatId && history.length === 0 ? (
           <div className="text-center mt-5 text-muted opacity-50">
               <div style={{fontSize: "4rem"}}>🧙‍♂️</div>
               <h3>Nasıl yardımcı olabilirim?</h3>
               <p>Hemen aşağıya yazmaya başla, senin için yeni bir sohbet oluşturayım.</p>
           </div>
        ) : (
           <>
               {history.map((item, index) => (
                 <div key={index} className="mb-4">
                    {/* USER MESAJI */}
                    <div className="d-flex justify-content-end mb-2">
                        <div className="bg-primary text-white p-3 rounded-3 shadow-sm" style={{ maxWidth: "75%", borderBottomRightRadius: "0" }}>
                            {item.userMessage}
                        </div>
                    </div>

                    {/* AI MESAJI */}
                    {item.aiResponse ? (
                        <div className="d-flex justify-content-start">
                            <div className="me-2 fs-3">🤖</div>
                            <div className="d-flex flex-column" style={{ maxWidth: "85%", minWidth: "50%" }}>
                                
                                {/* --- GÜNCELLENEN KISIM: MARKDOWN RENDERER --- */}
                                <div className="bg-light border p-3 rounded-3 shadow-sm" style={{ borderTopLeftRadius: "0" }}>
                                    <MarkdownRenderer content={item.aiResponse} />
                                </div>

                                {/* İNDİR BUTONU */}
                                {item.fileExtension && (
                                    <button 
                                        className="btn btn-sm btn-outline-success mt-2 align-self-start"
                                        onClick={() => handleDownload(item.aiResponse, item.fileExtension, index)}
                                    >
                                        ⬇️ İndir ({item.fileExtension.toUpperCase()})
                                    </button>
                                )}
                            </div>
                        </div>
                    ) : item.isTemp && (
                        <div className="text-muted ms-5 small"><i>Yazıyor...</i></div>
                    )}
                 </div>
               ))}
               <div ref={messagesEndRef} />
           </>
        )}
      </div>

      {/* INPUT ALANI */}
      <div className="p-3 bg-light border-top">
        <div className="input-group input-group-lg shadow-sm">
            <input 
                type="text" 
                className="form-control border-0" 
                placeholder={selectedChatId ? "Mesajını yaz..." : "Yeni bir sohbet başlat..."} 
                value={inputMessage}
                onChange={(e) => setInputMessage(e.target.value)}
                onKeyDown={(e) => e.key === 'Enter' && handleSendMessage()}
            />
            <button className="btn btn-primary px-4" onClick={handleSendMessage}>Gönder ➤</button>
        </div>
      </div>
      
    </div>
  );
};

export default Home;