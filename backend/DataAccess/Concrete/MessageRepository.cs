using Microsoft.EntityFrameworkCore;
using DataAccess.Abstract;
using Model.Entities;
using DataAccess;

namespace DataAccess.Concrete
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Message> AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<Message> DeleteByIdAsync(string id)
        {
            var message = await _context.Messages
                                        .Include(m => m.Response)
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }

            return message;
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatIdAsync(string chatId)
        {
            return await _context.Messages
                                 .Include(m => m.Response)
                                 .Where(m => m.ChatId == chatId)
                                 .OrderBy(m => m.Timestamp)
                                 .ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(string messageId)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
        }
    }
}
