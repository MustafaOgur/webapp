using System.Collections.Generic;
using Model.DTOs.Admin;
using Model.DTOs.User;
using Model.Entities;

namespace DataAccess.Abstract
{
    public interface IAdminRepository
    {
        Task<AdminDashboardDto> GetDashboardStats();

        Task<List<User>> GetAllUsers();
    }
}