using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public static class ErrorCodes
    {
        // Genel
        public const string VALIDATION_FAILED = "SMC-GEN-VAL-0001";

        // Chat Validation
        public const string CHAT_EMPTY = "SMC-CHAT-VAL-0001";
        public const string CHAT_NAME_EMPTY = "SMC-CHAT-VAL-0002";
        public const string CHAT_NAME_TOO_SHORT = "SMC-CHAT-VAL-0003";
        public const string CHAT_NAME_TOO_LONG = "SMC-CHAT-VAL-0004";
        public const string CHAT_NAME_INVALID_CHARS = "SMC-CHAT-VAL-0005";
        public const string CHAT_NAME_WHITESPACE = "SMC-CHAT-VAL-0006";
        public const string CHAT_UPDATED_AT_INVALID = "SMC-CHAT-VAL-0007";
        public const string CHAT_ID_INVALID = "SMC-CHAT-VAL-0008";

        // Message Validation
        public const string MESSAGE_EMPTY = "SMC-MSG-VAL-0001";
        public const string MESSAGE_CONTENT_EMPTY = "SMC-MSG-VAL-0002";
        public const string MESSAGE_CONTENT_TOO_LONG = "SMC-MSG-VAL-0003";
        public const string MESSAGE_CONTENT_WHITESPACE = "SMC-MSG-VAL-0004";

        // Response Validation
        public const string RESPONSE_EMPTY = "SMC-RESP-VAL-0001";
        public const string RESPONSE_CONTENT_EMPTY = "SMC-RESP-VAL-0002";
        public const string RESPONSE_CONTENT_WHITESPACE = "SMC-RESP-VAL-0003";

        // Id Validation
        public const string ID_EMPTY = "SMC-GEN-VAL-0010";
        public const string ID_INVALID = "SMC-GEN-VAL-0011";

        // Chat Service
        public const string CHAT_NOT_FOUND = "CHT-0001";          // Chat bulunamadı
        public const string CHAT_CREATION_FAILED = "CHT-0002";    // Chat oluşturulamadı
        public const string CHAT_UPDATE_FAILED = "CHT-0003";      // Chat güncellenemedi
        public const string CHAT_DELETE_FAILED = "CHT-0004";      // Chat silinemedi
        public const string CHAT_LIST_EMPTY = "CHT-0005";         // Chat listesi boş
        public const string CHAT_VALIDATION_FAILED = "CHT-0006"; // Chat validasyonu başarısız

        // Message Service
        public const string MESSAGE_NOT_FOUND = "MSG-0001";          // Mesaj bulunamadı
        public const string MESSAGE_CREATION_FAILED = "MSG-0002";    // Mesaj oluşturulamadı
        public const string MESSAGE_DELETE_FAILED = "MSG-0003";      // Mesaj silinemedi
        public const string MESSAGE_VALIDATION_FAILED = "MSG-0004";  // Mesaj validasyonu başarısız
        public const string MESSAGE_LIST_EMPTY = "MSG-0005";         // Chat’e ait mesaj yok

        // Response Service
        public const string RESPONSE_NOT_FOUND = "RSP-0001";          // Response bulunamadı
        public const string RESPONSE_CREATION_FAILED = "RSP-0002";    // Response oluşturulamadı
        public const string RESPONSE_VALIDATION_FAILED = "RSP-0003";  // Response validasyonu başarısız

        public static string PASSWORD_NOT_CORRECT { get; set; }
        public static string USER_NOT_FOUND { get; set; }
        public static string EMAIL_IS_EXIST { get; set; }
        public static string TOKEN_NOT_FOUND { get; set; }
    }
}
