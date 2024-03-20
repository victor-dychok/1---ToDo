using ToDoBL.dto;
using ToDoDomain;

namespace ToDo.BL
{
    public interface IToDoService
    {
        public Task<IEnumerable<TodoItem>> GetListAsync(int? offset, int? limit, string? lable, int? ownerId, CancellationToken cancellationToken = default);
        public Task<TodoItem> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<TodoItem> GetByIdIsDoneAsync(int id, CancellationToken cancellationToken = default);
        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        public Task<TodoItem> AddAsync(CreateToDo item, CancellationToken cancellationToken = default);
        public Task<TodoItem> UpdateAsync(UpdateToDo newItem, CancellationToken cancellationToken = default);
        public Task<TodoItem> PutchAsync(int id, bool isDone, CancellationToken cancellationToken = default);
    }
}
