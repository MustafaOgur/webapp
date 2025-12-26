using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CurrentUserService(IHttpContextAccessor contextAccessor) 
        {
            _contextAccessor = contextAccessor;
        }
        public string UserId
        {
            get
            {
                var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Current user id not found in JWT claims");

                return userId;
            }
        }

        public string Role
        {
            get
            {
                return _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? "User";
            }
        }
    }
}
