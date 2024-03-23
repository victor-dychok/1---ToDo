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
using Common.Api.Service;
using Common.BL.Exeptions;

namespace ToDoBL
{
    public class ToDoService : IToDoService
    {
        private readonly IRepository<TodoItem> _toDoRepository;
        private readonly IRepository<AppUser> _users;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _currentUserId;
        private List<string> _userRoles;
        public ToDoService(
            IRepository<TodoItem> repository,
            ICurrentUserService currentUserService,
            IRepository<AppUser> user,
            IMapper mapper)
        {
            _mapper = mapper;

            _currentUserService = currentUserService;
            _toDoRepository = repository;
            _users = user;
            _currentUserId = int.Parse(_currentUserService.CurrentUserId);
            _userRoles = _currentUserService.CurrentUserRoles.ToList();
        }
        public async Task<IEnumerable<TodoItem>> GetListAsync(int? offset, int? ownerId, string? lable, int? limit = 10, CancellationToken cancellationToken = default)
        {

            if(_userRoles.Contains("Admin"))
            {
                return await _toDoRepository.GetListAsync(
                offset,
                limit,
                d => (string.IsNullOrWhiteSpace(lable) || d.Label.Contains(lable, StringComparison.InvariantCultureIgnoreCase))
                && (ownerId == null || d.OwnerId == ownerId.Value),
                t => t.Id);
            }
            else
            {
                return await _toDoRepository.GetListAsync(
                offset,
                limit,
                d => (string.IsNullOrWhiteSpace(lable) || d.Label.Contains(lable, StringComparison.InvariantCultureIgnoreCase))
                && (d.OwnerId == _currentUserId),
                t => t.Id);
            }
            
        }

        public async Task<TodoItem> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var item = await _toDoRepository.SingleOrDefaultAsync(t => t.Id == id);
            if(item == null)
            {
                throw new NotFoundExeption(new {Id = id});
            }
            else if(CheckAccess(item.OwnerId))
            {
                return item;
            }
            else
            {
                throw new ForbidenExeption("Access denied");
            }
        }
        public async Task<TodoItem> GetByIdIsDoneAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _toDoRepository.SingleOrDefaultAsync(d => d.Id == id);
            if(result == null)
            {
                throw new NotFoundExeption(new { Id = id});
            }
            if (CheckAccess(result.OwnerId))
            {
                return result;
            }
            else throw new ForbidenExeption("Access denied");
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {

            var item = await GetByIdAsync(id, cancellationToken);
            if(CheckAccess(item.OwnerId))
            {
                return await _toDoRepository.DeleteAsync(item);
            }
            else
            {
                throw new ForbidenExeption("Access denied");
            }
            
        }

        public async Task<TodoItem> AddAsync(CreateToDo item, CancellationToken cancellationToken)
        {
            int ownerId;
            if (_userRoles.Contains("Admin"))
            {
                ownerId = item.OwnerId;
            }
            else
            {
                ownerId = _currentUserId;
                item.OwnerId = _currentUserId;
            }
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

            if(CheckAccess(todoEntity.OwnerId))
            {
                var updatedItem = await _toDoRepository.UpdateAsync(todoEntity, cancellationToken);
                if (updatedItem is null)
                {
                    throw new Exception("Can not update ToDo item");
                }
                return updatedItem;
            }
            else
            {
                throw new ForbidenExeption("Access denied");
            }
        }

        public async Task<TodoItem> PutchAsync(int id, bool isDone, CancellationToken cancellationToken)
        {
            var todoItem = await GetByIdAsync(id, cancellationToken);
            if(todoItem == null)
            {
                throw new NotFoundExeption("Todo item not found");
            }

            todoItem.IsDone = isDone;

            if(CheckAccess(todoItem.OwnerId))
            {

                var putchedItem = await _toDoRepository.UpdateAsync(todoItem);
                if (putchedItem is null)
                {
                    throw new BadRequestExeption("Can not create item");
                }
                return putchedItem;
            }
            else
            {
                throw new ForbidenExeption("Access denied");
            }
        }

        private bool CheckAccess(int id)
        {
            return _currentUserId == id || _userRoles.Contains("Admin");
        }

    }
}
