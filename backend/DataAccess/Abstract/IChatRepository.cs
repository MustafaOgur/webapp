using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IChatRepository
    {
        Task<Chat> CreateAsync(Chat chat);
        Task<Chat> UpdateAsync(Chat chat);
        Task DeleteByIdAsync(string id);
        Task<Chat> GetByIdAsync(string id);
        Task<IEnumerable<Chat>> GetAllAsync();
        Task DeleteAllAsync();

    }
}
