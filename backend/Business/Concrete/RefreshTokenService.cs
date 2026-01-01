using Core.Constants;
using Core.Utilities.Results;
using Model.DTOs.User;
using Model.Entities;
using Business.Abstract;
using DataAccess.Abstract;



namespace Business.Concrete
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<IDataResult<RefreshToken>> AddRefreshTokenAsync(UserDto userDto, string token, DateTime expiry)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userDto.Id,
                Token = token,
                ExpiryDate = expiry
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            return new SuccessDataResult<RefreshToken>(refreshToken);
        }

        public async Task<IDataResult<RefreshToken>> GetRefreshTokenAsync(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
            if (refreshToken == null)
            {
                return new ErrorDataResult<RefreshToken>("Token bulunamadı", ErrorCodes.TOKEN_NOT_FOUND);
            }
            return new SuccessDataResult<RefreshToken>(refreshToken);
        }

        public async Task<IResult> RevokeRefreshTokenAsync(RefreshToken token)
        {
            await _refreshTokenRepository.RevokeAsync(token);
            return new SuccessResult("Token kaldırıldı.");
        }
    }
}
