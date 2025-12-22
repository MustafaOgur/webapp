using Model.DTOs.User;
using Core.Utilities.Results;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IRefreshTokenService
    {
        Task<IDataResult<RefreshToken>> AddRefreshTokenAsync(UserDto user, string token, DateTime expiry);
        Task<IDataResult<RefreshToken?>> GetRefreshTokenAsync(string token);
        Task<IResult> RevokeRefreshTokenAsync(RefreshToken token);
    }
}
