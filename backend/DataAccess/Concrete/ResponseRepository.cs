using Microsoft.EntityFrameworkCore;
using DataAccess.Abstract;
using Model.Entities;
using DataAccess;

namespace DataAccess.Concrete
{
    public class ResponseRepository : IResponseRepository
    {
        private readonly AppDbContext _context;

        public ResponseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Response> AddResponseAsync(Response response)
        {
            _context.Responses.Add(response);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<Response?> GetByMessageIdAsync(string messageId)
        {
            return await _context.Responses
                                 .FirstOrDefaultAsync(r => r.MessageId == messageId);
        }
    }
}
