using AutoMapper;
using Core.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Model.DTOs.User;
using Model.Entities;
using Business.Abstract;
using DataAccess.Abstract;


namespace Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        public async Task<IDataResult<UserDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null) 
            {
                return new ErrorDataResult<UserDto>("Kullanıcı bulunamadı", ErrorCodes.USER_NOT_FOUND);
            }

            var isVerifyPasswordHash = PasswordHasher.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt);
            if (!isVerifyPasswordHash)
            {
                return new ErrorDataResult<UserDto>("Sifre hatali", ErrorCodes.PASSWORD_NOT_CORRECT);
            }

            var userDto = _mapper.Map<UserDto>(user);
            return new SuccessDataResult<UserDto>(userDto, "Giriş başarılı");
        }


        public async Task<IDataResult<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            var isEmailExist = await _userRepository.EmailExistsAsync(registerDto.Email);
            if (isEmailExist)
            {
                return new ErrorDataResult<UserDto>("Bu eposta zaten sistemde kayıtlı", ErrorCodes.EMAIL_IS_EXIST);
            }
            
            PasswordHasher.CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userRepository.AddUserAsync(user);
            var userDto = _mapper.Map<UserDto>(user);
            return new SuccessDataResult<UserDto>(userDto, "Kayıt başarılı");
        }

        public async Task<IDataResult<UserDto>> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) 
            {
                return new ErrorDataResult<UserDto>("Kullanıcı bulunamadı", ErrorCodes.USER_NOT_FOUND);
            }
            var userDto = _mapper.Map<UserDto>(user);
            return new SuccessDataResult<UserDto>(userDto);
        }
    }
}
