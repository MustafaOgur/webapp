using AutoMapper;
using Core.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security;
using Model.DTOs.Chat;
using Model.DTOs.Message;
using Model.Entities;
using Business.Abstract;
using DataAccess.Abstract;

namespace Business.Concrete
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IChatService _chatService;
        private readonly ICurrentUserService _currentUserService;
        public MessageService(IMessageRepository messageRepository, IMapper mapper, IChatService chatService, ICurrentUserService currentUserService)
        {

            _messageRepository = messageRepository;
            _mapper = mapper;
            _chatService = chatService;
            _currentUserService = currentUserService;

        }


        public async Task<IDataResult<MessageDto>> AddMessageAsync(CreateMessageDto createMessageDto)
        {
            if (createMessageDto.ChatId == null)
            {
                var createChatDto = new CreateChatDto {Name = "Unnamed Chat"};

                var newChatResult = await _chatService.CreateChatAsync(createChatDto);

                if (!newChatResult.Success)
                    return new ErrorDataResult<MessageDto>("Chat oluşturulamadı", ErrorCodes.CHAT_CREATION_FAILED);

                createMessageDto.ChatId = newChatResult.Data.Id;

            }
            
            var message = _mapper.Map<Message>(createMessageDto);
            
            message.OwnerUserId = _currentUserService.UserId;

            var addedMessage = await _messageRepository.AddMessageAsync(message);
            var messageDto = _mapper.Map<MessageDto>(addedMessage);
            return new SuccessDataResult<MessageDto>(messageDto, "Mesaj başarıyla eklendi");
        }


        public async Task<IDataResult<MessageDto>> GetMessageByIdAsync(string messageId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);
            if (message == null)
                return new ErrorDataResult<MessageDto>("Message bulunamadı", ErrorCodes.MESSAGE_NOT_FOUND);

            var messageDto = _mapper.Map<MessageDto>(message);
            return new SuccessDataResult<MessageDto>(messageDto);
        }



        public async Task<IDataResult<IEnumerable<MessageDto>>> GetMessagesByChatIdAsync(string chatId)
        {
            var messages = await _messageRepository.GetMessagesByChatIdAsync(chatId);
            if (messages == null || !messages.Any())
            {
                return new ErrorDataResult<IEnumerable<MessageDto>>("No messages found for this chat.", ErrorCodes.MESSAGE_LIST_EMPTY);
            }

            var messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages);
            return new SuccessDataResult<IEnumerable<MessageDto>>(messageDtos);
        }

        
        public async Task<IResult> DeleteMessageAsync(string id)
        {
            var deletedMessage = await _messageRepository.DeleteByIdAsync(id);
            return new SuccessResult("Message successfully deleted.");
        }
    }

}
