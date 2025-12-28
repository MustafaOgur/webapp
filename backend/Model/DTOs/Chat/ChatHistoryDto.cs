using System;

namespace Model.DTOs.Chat
{
    public class ChatHistoryDto
    {
        public string MessageId { get; set; }
        public string UserMessage { get; set; }
        public DateTime MessageDate { get; set; }
        
        public string? AiResponse { get; set; } 
        public DateTime? ResponseDate { get; set; }
    }
}