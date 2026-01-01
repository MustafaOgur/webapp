using Core.Entities;

namespace Model.Entities
{
    public class Chat : BaseEntity
    {
        public string OwnerUserId { get; set; }
        public User Owner { get; set; }
        public string Name { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        //Navigation Property => Bu Chat’e ait tüm Message kayıtlarını temsil eder.
        public ICollection<Message> Messages { get; set; }
    }
}
