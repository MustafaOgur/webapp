namespace Model.DTOs.Chat
{
    public class UpdateChatDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
