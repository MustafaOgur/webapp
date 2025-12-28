using AutoMapper;
using Core.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security;
using Model.DTOs.Chat;
using Model.Entities;
using Business.Abstract;
using DataAccess.Abstract;

namespace Business.Concrete
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public ChatService(IChatRepository chatRepository, IMapper mapper, ICurrentUserService currentUserService) 
        { 

            _chatRepository = chatRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        
        public async Task<IDataResult<ChatDto>> CreateChatAsync(CreateChatDto createChatDto)
        {
            var chat = _mapper.Map<Chat>(createChatDto);

            // OwnerId'yi Jwt'den almak !!!
            chat.OwnerUserId = _currentUserService.UserId;

            var createdChat = await _chatRepository.CreateAsync(chat);
            var chatDto = _mapper.Map<ChatDto>(createdChat);

            return new SuccessDataResult<ChatDto>(chatDto, "Chat başarıyla oluşturuldu.");
        }


        public async Task<IDataResult<IEnumerable<ChatDto>>> GetAllChatsAsync()
        {
            var chats = await _chatRepository.GetAllAsync();
            if (chats == null || !chats.Any())
            {
                return new ErrorDataResult<IEnumerable<ChatDto>>(Enumerable.Empty<ChatDto>(), "Hiç chat bulunamadı.", ErrorCodes.CHAT_LIST_EMPTY);
            }
            var chatDtos = _mapper.Map<IEnumerable<ChatDto>>(chats);
            return new SuccessDataResult<IEnumerable<ChatDto>>(chatDtos, "Chat listesi getirildi.");
        }


        public async Task<IDataResult<ChatDto>> GetChatByIdAsync(string id)
        {
            var chat = await _chatRepository.GetByIdAsync(id);
            if (chat == null)
            {
                return new ErrorDataResult<ChatDto>($"{id} ID'sine sahip chat bulunamadı.", ErrorCodes.CHAT_NOT_FOUND);
            }

            var chatDto = _mapper.Map<ChatDto>(chat);
            return new SuccessDataResult<ChatDto>(chatDto, "Chat bulundu.");
        }


        public async Task<IDataResult<ChatDto>> UpdateChatAsync(string id, UpdateChatDto updateChatDto)
        {
            var existingChat = await _chatRepository.GetByIdAsync(id);
            if (existingChat == null)
            {
                return new ErrorDataResult<ChatDto>($"{id} ID'sine sahip chat bulunamadı.", ErrorCodes.CHAT_NOT_FOUND);
            }

            var updatedChat = _mapper.Map(updateChatDto, existingChat);
            var chat = await _chatRepository.UpdateAsync(updatedChat);
            
            var chatDto = _mapper.Map<ChatDto>(chat);
            return new SuccessDataResult<ChatDto>(chatDto, "Chat güncellendi.");
        }


        public async Task<IResult> DeleteChatAsync(string id)
        {
            var existingChat = await _chatRepository.GetByIdAsync(id);
            if (existingChat == null) return new ErrorResult($"{id} ID'sine sahip chat bulunamadı", ErrorCodes.CHAT_NOT_FOUND);

            await _chatRepository.DeleteByIdAsync(id);
            return new SuccessResult($"{id} ID'sine sahip chat silindi");
        }

        public async Task<IDataResult<IEnumerable<ChatHistoryDto>>> GetChatHistoryAsync(string chatId)
        {
            // 1. Chat'i çek
            var chat = await _chatRepository.GetByIdAsync(chatId);

            if (chat == null)
            {
                return new ErrorDataResult<IEnumerable<ChatHistoryDto>>("Chat bulunamadı.", ErrorCodes.CHAT_NOT_FOUND);
            }

            // 2. Güvenlik Kontrolü
            if (chat.OwnerUserId != _currentUserService.UserId)
            {
                return new ErrorDataResult<IEnumerable<ChatHistoryDto>>("Bu sohbeti görüntüleme yetkiniz yok."); 
            }

            // 3. Entity -> DTO Dönüşümü (DÜZELTİLDİ: Timestamp kullanıldı)
            var history = chat.Messages
                .OrderBy(m => m.Timestamp) // Sıralama Timestamp'e göre
                .Select(m => new ChatHistoryDto
                {
                    MessageId = m.Id,
                    UserMessage = m.Content,
                    MessageDate = m.Timestamp, // CreatedDate -> Timestamp oldu
                    
                    AiResponse = m.Response?.Content, 
                    ResponseDate = m.Response?.Timestamp // CreatedDate -> Timestamp oldu
                })
                .ToList();

            return new SuccessDataResult<IEnumerable<ChatHistoryDto>>(history, "Sohbet geçmişi getirildi.");
        }

    }

}
