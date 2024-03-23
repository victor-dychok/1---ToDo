using AutoMapper;
using Common.Api.Service;
using Common.BL;
using Common.BL.Exeptions;
using Common.Domain;
using Common.Repository;
using System.Collections.Generic;
using ToDoDomain;
using User.Service.dto;
using UserServices.dto;
using UserServices.Utils;


namespace UserServices
{
    public class UserService : IUserService
    {
        private readonly IRepository<AppUser> _userRepository;
        private readonly IRepository<AppUserRole> _userRoles;
        private readonly IRepository<AppUserAppRole> _appUR;
        private readonly ICurrentUserService _currentUserService;

        private readonly IMapper _mapper;
        public UserService(
            IRepository<AppUser> users, 
            IRepository<AppUserRole> userRole,
            IRepository<AppUserAppRole> appUR,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _userRoles = userRole;
            _userRepository = users;
            _appUR = appUR;
            _currentUserService = currentUserService;
            _mapper = mapper;

            var list = GetListAsync(null, null, null).Result.ToList();
            if (list.Count == 0)
            {

                var user = AddAsync(new UserDto
                {
                    Login = "admin",
                    Password = "12345678"
                }).Result;

                var role = _userRoles.SingleOrDefaultAsync(i => i.Name == "Admin").Result;
                var currentUser = _mapper.Map<AppUser>(user);
                var currentAppUR = _appUR.AddAsync(new AppUserAppRole
                {
                    RoleId = role.Id,
                    UserId = currentUser.Id
                });

            }

        }

        public async Task<UserGetDto> AddAsync(UserDto item, CancellationToken token = default)
        {
            if (await _userRepository.SingleOrDefaultAsync(u => u.Login == item.Login.Trim()) is not null)
            {
                throw new BadRequestExeption("This login is alreadi taken");
            }
            var entity = new AppUser()
            {
                Login = item.Login,
                PasswordHash = PasswordHashUtil.Hash(item.Password),
            };

            var addedItem = await _userRepository.AddAsync(entity, token);
            if(addedItem is null)
            {
                throw new BadRequestExeption("Can not add user");
            }

            var role = await _userRoles.SingleOrDefaultAsync(i => i.Name == "Client");
            var appUR = await _appUR.AddAsync(new AppUserAppRole { 
                Role = role,
                User = addedItem
            }, token);

            return _mapper.Map<UserGetDto>(addedItem);
        }


        public async Task<UserGetDto> GetByIdOrDefaultAsync(int id, CancellationToken token = default)
        {
            var item = await _userRepository.SingleOrDefaultAsync(x => x.Id == id);
            if (item is null)
            {
                throw new NotFoundExeption(new { Id = id });
            }
            return _mapper.Map<UserGetDto>(item);
        }

        public async Task<IReadOnlyCollection<UserGetDto>> GetListAsync(int? offset, string? name, int? limit, CancellationToken token = default)
        {
            return _mapper.Map<IReadOnlyCollection<UserGetDto>>(await _userRepository.GetListAsync(
                offset,
                limit,
                name == null ? null : u => u.Login.Contains(name),
                token: token));
        }

        public async Task<UserGetDto> UpdateAsync(UserPutDto newItem, CancellationToken token = default)
        {
            var currentUserId = int.Parse(_currentUserService.CurrentUserId);
            var userRoles = _currentUserService.CurrentUserRoles.ToList();

            var user = await _userRepository.SingleOrDefaultAsync(u => u.Id == newItem.Id);
            user.Login =newItem.Login;
            if(user.Id == currentUserId || userRoles.Contains("Admin"))
            {
                var item = await _userRepository.UpdateAsync(user, token);
                if (item is null)
                {
                    throw new BadRequestExeption("Can not update user");
                }
                return _mapper.Map<UserGetDto>(item);
            }
            else
            {
                throw new ForbidenExeption("Access denied");
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken token = default)
        {
            var currentUserId = int.Parse(_currentUserService.CurrentUserId);
            var userRoles = _currentUserService.CurrentUserRoles.ToList();
            var user = await _userRepository.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundExeption(new { Id = id });
            }
            if (user.Id == currentUserId || userRoles.Contains("Admin"))
            {
                return await _userRepository.DeleteAsync(user, token);
            }
            else
            {
                throw new ForbidenExeption("Access denied");
            }
        }

        public async Task<UserGetDto> ResetPasswordAsync(int id, UserPasswordResetDto newItem, CancellationToken token = default)
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
                return _mapper.Map<UserGetDto>(item);
            }
            else
            {
                throw new ForbidenExeption("Access denied");
            }
        }
    }
}
