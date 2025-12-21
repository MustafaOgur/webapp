using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IMessageRepository
    {
        Task<Message> AddMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesByChatIdAsync(string chatId);
        Task<Message> DeleteByIdAsync(string id);
        Task<Message> GetMessageByIdAsync(string messageId);
    }
}
