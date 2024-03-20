using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServices.dto;

namespace UserServices
{
    public interface IAutnService
    {
        public Task<JwtTokenDto> GetJwtTokenAsync(AuthDto auth, CancellationToken cancellationToken = default);
        public Task<JwtTokenDto> GetJwtByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
