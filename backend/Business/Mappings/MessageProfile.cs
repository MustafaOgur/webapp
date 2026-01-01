using AutoMapper;
using Model.DTOs.Message;
using Model.Entities;

namespace Business.Mappings
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            // Entity -> DTO Mapping
            CreateMap<Message, MessageDto>();

            // DTO -> Entity Mapping
            CreateMap<CreateMessageDto, Message>();

        }
    }
}
