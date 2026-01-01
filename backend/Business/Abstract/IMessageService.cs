using Model.DTOs.Message;
using Core.Utilities.Results;

namespace Business.Abstract
{
    public interface IMessageService
    {
        Task<IDataResult<MessageDto>> AddMessageAsync(CreateMessageDto createMessageDto);

        Task<IDataResult<IEnumerable<MessageDto>>> GetMessagesByChatIdAsync(string chatId);

        Task<IDataResult<MessageDto>> GetMessageByIdAsync(string messageId);

        Task<IResult> DeleteMessageAsync(string id);
    }
}
