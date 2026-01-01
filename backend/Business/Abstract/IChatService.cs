using Model.DTOs.Chat;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IChatService
    {
        Task<IDataResult<ChatDto>> CreateChatAsync(CreateChatDto createChatDto);

        Task<IDataResult<IEnumerable<ChatDto>>> GetAllChatsAsync();

        Task<IDataResult<ChatDto>> GetChatByIdAsync(string id);

        Task<IDataResult<ChatDto>> UpdateChatAsync(string id, UpdateChatDto updateChatDto);

        Task<IResult> DeleteChatAsync(string id);

        //Task<IResult> DeleteAllChatsAsync();

        Task<IDataResult<IEnumerable<ChatHistoryDto>>> GetChatHistoryAsync(string chatId);
    }
}
