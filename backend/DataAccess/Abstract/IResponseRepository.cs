using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IResponseRepository
    {
        Task<Response> AddResponseAsync(Response response);
        Task<Response?> GetByMessageIdAsync(string messageId);
    }
}
