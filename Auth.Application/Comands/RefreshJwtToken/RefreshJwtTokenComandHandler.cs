using Application.Common.Domain;
using Auth.Api.dto;
using Common.Application.Abstraction.Percistance;
using Common.Application.Exeptions;
using Common.Application.Utils;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserServices.dto;

namespace Auth.Application.Comands.RefreshJwtToken
{
    public class RefreshJwtTokenComandHandler : IRequestHandler<RefreshJwtTokenComand, JwtTokenDto>
    {
        private readonly IRepository<AppUser> _userRepository;
        private readonly IRepository<AppUserAppRole> _appUR;
        private readonly IRepository<AppUserRole> _roles;
        private readonly IRepository<RefreshToken> _tokens;
        private readonly IConfiguration _configuration;
        public RefreshJwtTokenComandHandler(
            IRepository<AppUser> users,
            IRepository<AppUserAppRole> appUR,
            IRepository<AppUserRole> roles,
            IRepository<RefreshToken> tokens,
            IConfiguration configuration)
        {
            _userRepository = users;
            _tokens = tokens;
            _roles = roles;
            _configuration = configuration;
            _appUR = appUR;
        }
        public async Task<JwtTokenDto> Handle(RefreshJwtTokenComand request, CancellationToken cancellationToken)
        {
            var currentRefreshToken = await _tokens.SingleOrDefaultAsync(e => e.Id == request.RefreshToken, cancellationToken);
            if (currentRefreshToken is null)
            {
                throw new ForbidenExeption("");
            }

            var user = await _userRepository.SingleOrDefaultAsync(u => u.Id == currentRefreshToken.UserId, cancellationToken);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Login),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var roles = await _appUR.GetListAsync(null, null, u => u.User.Login == user.Login);
            foreach (var role in roles)
            {
                var item = await _roles.SingleOrDefaultAsync(i => i.Id == role.RoleId);
                claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var dateExpites = DateTime.UtcNow.AddMinutes(15);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDeskription = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
                claims, expires: dateExpites, signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeskription)!;
            var refreshToken = await _tokens.AddAsync(new RefreshToken { UserId = user.Id, User = user }, cancellationToken);

            return new JwtTokenDto()
            {
                JwtToken = token,
                RefreshToken = currentRefreshToken.Id,
                Expires = dateExpites
            };
        }
    }
}
