using Model.DTOs.User;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<UserDto>> RegisterAsync(RegisterDto registerDto);
        Task<IDataResult<UserDto>> LoginAsync(LoginDto loginDto);
        Task<IDataResult<UserDto>> GetUserByIdAsync(string id);

    }
}
