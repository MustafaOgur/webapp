using Model.DTOs.Response;
using Core.Utilities.Results;

namespace Business.Abstract
{
    public interface IResponseService
    {

        Task<IDataResult<ResponseDto>> AddResponseAsync(string messageId);

        Task<IDataResult<ResponseDto?>> GetResponseByMessageIdAsync(string messageId);
    }
}
