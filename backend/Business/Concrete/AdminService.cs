using Business.Abstract;
using DataAccess.Abstract;
using Core.Utilities.Results;
using Model.DTOs.Admin;
using Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model.DTOs.User;
using AutoMapper;

namespace Business.Concrete
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;

        public AdminService(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<AdminDashboardDto>> GetDashboardStatsAsync()
        {
            var data = await _adminRepository.GetDashboardStats();
            
            return new SuccessDataResult<AdminDashboardDto>(data, "Dashboard istatistikleri başarıyla getirildi.");
        }

        public async Task<IDataResult<List<UserDto>>> GetAllUsersAsync()
        {
            var users = await _adminRepository.GetAllUsers();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            
            return new SuccessDataResult<List<UserDto>>(userDtos, "Tüm kullanıcılar listelendi.");
        }
    }
}