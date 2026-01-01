using Core.Entities;

namespace Model.Entities
{
    public class Response : BaseEntity
    {
        public string MessageId { get; set; }
        public Message Message { get; set; }
        public string OwnerUserId { get; set; }
        public User Owner { get; set; }
        public string Content { get; set; }
        public string? FileExtension { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    }
}
