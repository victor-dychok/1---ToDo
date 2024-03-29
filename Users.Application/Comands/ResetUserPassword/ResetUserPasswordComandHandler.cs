using Application.Common.Domain;
using AutoMapper;
using Common.Api.Service;
using Common.Application.Abstraction.Percistance;
using Common.Application.Exeptions;
using Common.Application.Utils;
using Common.Repository;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServices.dto;

namespace Application.User.Application.Comands.UpdateUserPassword
{
    public class ResetUserPasswordComandHandler
    {
        private readonly IRepository<AppUser> _userRepository;
        private readonly MemoryCache _memoryCache;
        private readonly ICurrentUserService _currentUserService;

        private readonly IMapper _mapper;
        public ResetUserPasswordComandHandler(
            IRepository<AppUser> users,
            ICurrentUserService currentUserService,
            UserMemoryCache memoryCache,
            IMapper mapper)
        {
            _userRepository = users;
            _currentUserService = currentUserService;
            _memoryCache = memoryCache.Cache;
            _mapper = mapper;
        }

        public async Task<GetUserDto> RunAsync(int id, ResetPasswordComand newItem, CancellationToken token = default)
        {
            var currentUserId = int.Parse(_currentUserService.CurrentUserId);
            var userRoles = _currentUserService.CurrentUserRoles.ToList();

            var user = await _userRepository.SingleOrDefaultAsync(u => u.Id == id);
            user.PasswordHash = PasswordHashUtil.Hash(newItem.Password);
            if (user.Id == currentUserId || userRoles.Contains("Admin"))
            {
                var item = await _userRepository.UpdateAsync(user, token);
                if (item is null)
                {
                    throw new BadRequestExeption("Can not update user");
                }
                _memoryCache.Clear();
                return _mapper.Map<GetUserDto>(item);
            }
            else
            {
                throw new ForbidenExeption("Access denied");
            }
        }
    }
}
