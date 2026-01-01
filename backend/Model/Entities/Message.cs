using Core.Entities;

namespace Model.Entities
{
    public class Message : BaseEntity
    {
        // Veritabanındaki foreign key, hangi Chat’e ait olduğunu tutar.
        public string ChatId { get; set; }

        // Navigation Property => C# tarafında o Chat entity’sine erişim sağlar.
        public Chat Chat { get; set; }
        public string OwnerUserId { get; set; }
        public User Owner { get; set; }
        public string Content { get; set; }

        public Response Response { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        

    }
}
