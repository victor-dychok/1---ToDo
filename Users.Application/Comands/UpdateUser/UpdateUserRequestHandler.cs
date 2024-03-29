
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServices.dto;
using AutoMapper;
using Common.Api.Service;
using Common.Application.Abstraction.Percistance;
using Common.Application.Exeptions;
using Application.Common.Domain;
using MediatR;
using Application.Users.Application.Comands.CreateUser;
using Microsoft.Extensions.Caching.Memory;

namespace Application.User.Application.Comands.UpdateUser
{
    public class UpdateUserRequestHandler : IRequestHandler<UpdateUserComand, GetUserDto>
    {
        private readonly IRepository<AppUser> _userRepository;
        private readonly MemoryCache _memoryCache;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateUserRequestHandler(
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

        public async Task<GetUserDto> Handle(UpdateUserComand request, CancellationToken cancellationToken)
        {
            var currentUserId = int.Parse(_currentUserService.CurrentUserId);
            var userRoles = _currentUserService.CurrentUserRoles.ToList();

            var user = await _userRepository.SingleOrDefaultAsync(u => u.Login == request.Login);
            user = _mapper.Map<UpdateUserComand, AppUser>(request);
            if (user.Id == currentUserId || userRoles.Contains("Admin"))
            {
                var item = await _userRepository.UpdateAsync(user, cancellationToken);
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
