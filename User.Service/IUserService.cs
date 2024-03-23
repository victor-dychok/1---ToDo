using Common.Domain;
using User.Service.dto;
using UserServices.dto;

namespace UserServices
{
    public interface IUserService
    {
        public Task<IReadOnlyCollection<UserGetDto>> GetListAsync(int? offset, string? name, int? limit, CancellationToken token = default);
        public Task<UserGetDto> GetByIdOrDefaultAsync(int id, CancellationToken token = default);
        public Task<UserGetDto> AddAsync(UserDto item, CancellationToken token = default);
        public Task<UserGetDto> UpdateAsync(UserPutDto newItem, CancellationToken token = default);
        public Task<UserGetDto> ResetPasswordAsync(int id, UserPasswordResetDto newItem, CancellationToken token = default);
        public Task<bool> DeleteAsync(int id, CancellationToken token = default);
    }
}
