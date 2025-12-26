
namespace Model.DTOs.Admin
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }
        public int TotalChats { get; set; }
        public int TotalMessages { get; set; }
        public int TotalAiResponses { get; set; }
    }
}