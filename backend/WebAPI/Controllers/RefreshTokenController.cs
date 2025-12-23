using Business.Abstract;
using Core.Utilities.Security.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokenController(IUserService userService, ITokenService tokenService, IConfiguration configuration, IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshDto)
        {
            var existingToken = await _refreshTokenService.GetRefreshTokenAsync(refreshDto.Token);

            if (!existingToken.Success || existingToken.Data.ExpiryDate <= DateTime.UtcNow || existingToken.Data.IsRevoked)
            {
                return BadRequest("Geçersiz veya süresi dolmuş refresh token.");
            }

            var userResult = await _userService.GetUserByIdAsync(existingToken.Data.UserId);
            if (!userResult.Success)
            {
                return BadRequest("Kullanıcı bulunamadı.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userResult.Data.Id.ToString()),
                new Claim(ClaimTypes.Name, userResult.Data.Username),
                new Claim(ClaimTypes.Email, userResult.Data.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var newAccessToken = _tokenService.GenerateAccessToken(claims);

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var expiry = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]!));

            await _refreshTokenService.RevokeRefreshTokenAsync(existingToken.Data);
            await _refreshTokenService.AddRefreshTokenAsync(userResult.Data, newRefreshToken, expiry);

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
