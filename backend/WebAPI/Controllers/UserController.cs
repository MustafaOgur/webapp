using Business.Abstract;
using Core.Utilities.Security.JWT;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;

        public UserController(IUserService userService, ITokenService tokenService, IConfiguration configuration, IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _userService.RegisterAsync(registerDto);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);

            if (result.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString()),
                    new Claim(ClaimTypes.Name, result.Data.Username),
                    new Claim(ClaimTypes.Email, result.Data.Email),
                    new Claim(ClaimTypes.Role, result.Data.Role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                
                // Config'den süreyi alıyoruz
                var expiryDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]!);
                var expiry = DateTime.UtcNow.AddDays(expiryDays);

                // Refresh token'ı veritabanına kaydediyoruz
                await _refreshTokenService.AddRefreshTokenAsync(result.Data, refreshToken, expiry);

                // --- KRİTİK DEĞİŞİKLİK BURADA ---
                // Refresh Token'ı JSON olarak değil, HttpOnly Cookie olarak veriyoruz.
                SetRefreshTokenCookie(refreshToken, expiry);

                // Kullanıcıya sadece Access Token dönüyoruz. Refresh Token gizli (Cookie'de).
                return Ok(new
                {
                    AccessToken = accessToken,
                    // RefreshToken = refreshToken  <-- ARTIK BU SATIRI SİLDİK
                    Message = "Giriş başarılı, oturum cookie ile başlatıldı."
                });
            }

            return BadRequest(result);
        }



        private void SetRefreshTokenCookie(string token, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,       // Kritik nokta: Javascript (Hacker) okuyamaz.
                Expires = expires,     // Token süresi bitince çerez de silinir.
                SameSite = SameSiteMode.Strict, // CSRF saldırılarını engeller.
                Secure = true,         // Sadece HTTPS üzerinden çalışır (Localhost'ta da çalışır).
            };

            // Tarayıcıya "refreshToken" adında bir kurabiye bırakıyoruz.
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
