using Microsoft.EntityFrameworkCore;
using DataAccess.Abstract;
using Model.Entities;
using DataAccess;

namespace DataAccess.Concrete
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Chat> CreateAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task<Chat> UpdateAsync(Chat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task DeleteByIdAsync(string id)
        {
            var chat = await _context.Chats
                                     .Include(c => c.Messages)
                                     .ThenInclude(m => m.Response)
                                     .FirstOrDefaultAsync(c => c.Id == id);

            if (chat != null)
            {
                _context.Chats.Remove(chat);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAllAsync()
        {
            var allChats = await _context.Chats
                                         .Include(c => c.Messages)
                                         .ThenInclude(m => m.Response)
                                         .ToListAsync();

            _context.Chats.RemoveRange(allChats);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Chat>> GetAllAsync()
        {
            return await _context.Chats.ToListAsync();
        }

        public async Task<Chat> GetByIdAsync(string id)
        {
            return await _context.Chats
                                 .Include(c => c.Messages)
                                 .ThenInclude(m => m.Response)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
