using Core.Utilities.Results;
using Model.DTOs.Admin;
using Model.DTOs.User;
using Model.Entities;


namespace Business.Abstract
{
    public interface IAdminService
    {
        Task<IDataResult<AdminDashboardDto>> GetDashboardStatsAsync();

        Task<IDataResult<List<UserDto>>> GetAllUsersAsync();
    }
}