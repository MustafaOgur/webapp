using AutoMapper;
using Model.DTOs.Response;
using Model.Entities;

namespace Business.Mappings
{
    public class ResponseProfile : Profile
    {
        public ResponseProfile()
        {
            CreateMap<Response, ResponseDto>();

            CreateMap<CreateResponseDto, Response>();
        }
    }
}
