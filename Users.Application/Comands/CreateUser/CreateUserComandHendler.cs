using Application.Common.Domain;
using Application.User.Application;
using AutoMapper;
using Common.Application.Abstraction.Percistance;
using Common.Application.Exeptions;
using Common.Application.Utils;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using UserServices.dto;

namespace Application.Users.Application.Comands.CreateUser
{
    public class CreateUserComandHendler : IRequestHandler<CreateUserComand, GetUserDto>
    {
        private readonly IRepository<AppUser> _userRepository;
        private readonly IRepository<AppUserRole> _roles;
        private readonly IRepository<AppUserAppRole> _appUR;
        private readonly MemoryCache _memoryCache;
        private readonly IMapper _mapper;
        public CreateUserComandHendler(
            IRepository<AppUser> users,
            IRepository<AppUserRole> roles,
            IRepository<AppUserAppRole> userRoles,
            UserMemoryCache memoryCache,
            IMapper mapper) 
        {
            _roles = roles;
            _userRepository = users;
            _memoryCache = memoryCache.Cache;
            _appUR = userRoles;
            _mapper = mapper;
        }

        public async Task<GetUserDto> Handle(CreateUserComand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.SingleOrDefaultAsync(u => u.Login == request.Login.Trim()) is not null)
            {
                throw new BadRequestExeption("This login is alreadi taken");
            }
            var entity = new AppUser()
            {
                Login = request.Login,
                PasswordHash = PasswordHashUtil.Hash(request.Password),
            };

            var addedItem = await _userRepository.AddAsync(entity, cancellationToken);
            if (addedItem is null)
            {
                throw new BadRequestExeption("Can not add user");
            }

            var role = await _roles.SingleOrDefaultAsync(i => i.Name == "Client");
            var appUR = await _appUR.AddAsync(new AppUserAppRole
            {
                Role = role,
                User = addedItem
            }, cancellationToken);

            _memoryCache.Clear();
            return _mapper.Map<GetUserDto>(addedItem);
        }
    }
}
