using AutoMapper;
using Model.DTOs.Chat;
using Model.Entities;

namespace Business.Mappings
{
    public class ChatProfile : Profile
    {
        public ChatProfile() 
        {
            // Entity -> DTO Mapping
            CreateMap<Chat, ChatDto>();

            // DTO -> Entity Mapping
            CreateMap<CreateChatDto, Chat>();
            CreateMap<UpdateChatDto, Chat>();

        }
    }
}
