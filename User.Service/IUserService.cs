using Common.Domain;
using UserServices.dto;

namespace UserServices
{
    public interface IUserService
    {
        public Task<IReadOnlyCollection<UserGetDto>> GetListAsync(int? offset, string? name, int? limit, CancellationToken token = default);
        public Task<UserGetDto> GetByIdOrDefaultAsync(int id, CancellationToken token = default);
        public Task<UserGetDto> AddAsync(UserDto item, CancellationToken token = default);
        public Task<UserGetDto> UpdateAsync(UserDto newItem, CancellationToken token = default);
        public Task<bool> DeleteAsync(int id, CancellationToken token = default);
    }
}
