using Model.DTOs.Response;

namespace Model.DTOs.Message
{
    public class MessageDto
    {
        public string Id { get; set; }
        public string ChatId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public ResponseDto? Response { get; set; }
    }

}
