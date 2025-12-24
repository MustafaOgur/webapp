
namespace Model.DTOs.Response
{
    public class ResponseDto
    {
        public string Id { get; set; }
        public string MessageId { get; set; }
        public string Content { get; set; }

        public string FileExtension { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
