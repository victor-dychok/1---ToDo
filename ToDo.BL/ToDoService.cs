using System.Reflection;
using ToDoBL.dto;
using ToDoDomain;
using AutoMapper.Internal;
using AutoMapper;
using Common.Domain;
using System.Collections.Generic;
using System.Linq.Expressions;
using ToDoBL.Validators;
using FluentValidation;
using System.Reflection.Metadata.Ecma335;
using Common.BL;
using Common.Repository;
using ToDo.BL;

namespace ToDoBL
{
    public class ToDoService : IToDoService
    {
        private readonly IRepository<TodoItem> _toDoRepository;
        private readonly IRepository<AppUser> _users;
        private readonly IMapper _mapper;
        public ToDoService(
            IRepository<TodoItem> repository,
            IRepository<AppUser> user,
            IMapper mapper)
        {
            _mapper = mapper;

            _toDoRepository = repository;

            _users = user;
            
        }
        public async Task<IEnumerable<TodoItem>> GetListAsync(int? offset, int? ownerId, string? lable, int? limit = 10, CancellationToken cancellationToken = default)
        {
            return await _toDoRepository.GetListAsync(
                offset,
                limit,
                d => (string.IsNullOrWhiteSpace(lable) || d.Label.Contains(lable, StringComparison.InvariantCultureIgnoreCase))
                && (ownerId == null || d.OwnerId == ownerId.Value),
                t => t.Id); ;
        }

        public async Task<TodoItem> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var item = await _toDoRepository.SingleOrDefaultAsync(t => t.Id == id);
            if(item == null)
            {
                throw new NotFoundExeption(new {Id = id});
            }
            return item;
        }
        public async Task<TodoItem> GetByIdIsDoneAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _toDoRepository.SingleOrDefaultAsync(d => d.Id == id);
            if(result == null)
            {
                throw new NotFoundExeption(new { Id = id});
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var item = await GetByIdAsync(id, cancellationToken);
            return await _toDoRepository.DeleteAsync(item);
        }

        public async Task<TodoItem> AddAsync(CreateToDo item, CancellationToken cancellationToken)
        {
            int ownerId = item.OwnerId;
            var user = await _users.SingleOrDefaultAsync(u => u.Id == ownerId);

            if(user == null)
            {
                throw new BadRequestExeption("Incorrect id");
            }
            
            var todoEntity = _mapper.Map<CreateToDo, TodoItem>(item);
            todoEntity.CreatedDate = DateTime.UtcNow;
            todoEntity.UpdatedDate = DateTime.UtcNow;
            todoEntity.User = user;

            var addedItem = await _toDoRepository.AddAsync(todoEntity);

            if (addedItem is null)
            {
                throw new BadRequestExeption("Can not creade ToDo item");
            }

            return addedItem;
        }

        public async Task<TodoItem> UpdateAsync(UpdateToDo updateDto, CancellationToken cancellationToken)
        {
            var todoEntity = new TodoItem();
            todoEntity = _mapper.Map<UpdateToDo, TodoItem>(updateDto);
            var user = await _users.SingleOrDefaultAsync(i => i.Id == todoEntity.OwnerId);
            if(user == null)
            {
                throw new BadRequestExeption("Incorrect owner id");
            }
            _mapper.Map(updateDto, todoEntity);
            todoEntity.UpdatedDate = DateTime.UtcNow;
            todoEntity.User = user;

            var updatedItem = await _toDoRepository.UpdateAsync(todoEntity, cancellationToken);

            if(updatedItem is null)
            {
                throw new Exception("Can not update ToDo item");
            }

            return updatedItem;
        }

        public async Task<TodoItem> PutchAsync(int id, bool isDone, CancellationToken cancellationToken)
        {
            var TodoItem = await GetByIdAsync(id, cancellationToken);
            if(TodoItem == null)
            {
                throw new NotFoundExeption("Todo item not found");
            }

            TodoItem.IsDone = isDone;

            var putchedItem = await _toDoRepository.UpdateAsync(TodoItem);

            if(putchedItem is null)
            {
                throw new BadRequestExeption("Can not create item");
            }

            return putchedItem;
        }

    }
}
