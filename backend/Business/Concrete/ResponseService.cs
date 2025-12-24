using AutoMapper;
using Core.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security;
using Model.Entities;
using Business.Abstract;
using DataAccess.Abstract;
using Model.DTOs.Response;
using Core.Utilities.ExternalServices;
using Business.Helpers;

namespace Business.Concrete
{
    public class ResponseService : IResponseService
    {
        private readonly IResponseRepository _responseRepository;
        private readonly ILlmClient _llmClient;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public ResponseService(IResponseRepository responseRepository, 
            IMapper mapper, ILlmClient llmClient, 
            IMessageService messageService,
            ICurrentUserService currentUserService)
        {

            _responseRepository = responseRepository;
            _llmClient = llmClient;
            _messageService = messageService;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }


        public async Task<IDataResult<ResponseDto>> AddResponseAsync(string messageID)
        {
            var messageResult = await _messageService.GetMessageByIdAsync(messageID);
            if (messageResult == null)
                return new ErrorDataResult<ResponseDto>("Message bulunamadı", ErrorCodes.MESSAGE_NOT_FOUND);

            string llmResponse = await _llmClient.SendMessageAsync(
                messageResult.Data.Content,
                LlmPrompts.systemPrompt
                );

            var response = new Response
            {
                MessageId = messageID,
                Content = llmResponse,
                OwnerUserId = _currentUserService.UserId,
                FileExtension = FileParseManager.GetFileExtension(llmResponse),
            };

            var addedResponse = await _responseRepository.AddResponseAsync(response);
            var responseDto = _mapper.Map<ResponseDto>(addedResponse);
            return new SuccessDataResult<ResponseDto>(responseDto, "Response oluşturuldu");
        }


        public async Task<IDataResult<ResponseDto?>> GetResponseByMessageIdAsync(string messageId)
        {
            var response = await _responseRepository.GetByMessageIdAsync(messageId);
            if (response == null)
                return new ErrorDataResult<ResponseDto?>("Response bulunamadı", ErrorCodes.RESPONSE_NOT_FOUND);
            var responseDto = _mapper.Map<ResponseDto>(response);
            return new SuccessDataResult<ResponseDto>(responseDto, "Response başarıyla döndürüldü");
        }
    }

}
