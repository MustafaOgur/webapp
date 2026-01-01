namespace Model.DTOs.Chat
{
    public class ChatDto
    {
        public string Id { get; set; }
        public string OwnerUserId { get; set; }
        public string Name { get; set; }

        //public ICollection<MessageDto>? Messages { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
