using DataAccess.Abstract;
using System.Linq;
using System.Collections.Generic;
using Model.DTOs.Admin;
using Model.DTOs.User;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace DataAccess.Concrete
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardDto> GetDashboardStats()
        {
            
            var stats = new AdminDashboardDto
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalChats = await _context.Chats.IgnoreQueryFilters().CountAsync(),
                TotalMessages = await _context.Messages.IgnoreQueryFilters().CountAsync(),
                TotalAiResponses = await _context.Responses.IgnoreQueryFilters().CountAsync()
            };

            return stats;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.OrderByDescending(u => u.Id).ToListAsync();
        }

    }
}