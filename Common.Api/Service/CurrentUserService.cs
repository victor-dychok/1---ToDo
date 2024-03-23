using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.Api.Service
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string CurrentUserId => _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        public string[] CurrentUserRoles => _contextAccessor.HttpContext.User.Claims
            .Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
    }
}
