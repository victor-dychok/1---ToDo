using Application.Common.Domain;
using AutoMapper;
using Common.Api.Service;
using Common.Application.Abstraction.Percistance;
using Common.Application.Exeptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.dto;
using ToDoDomain;

namespace ToDo.Application.Comands.Update
{
    public class UpdateTodoComandHandler : IRequestHandler<UpdateTodoComand, TodoGetDto>
    {
        private readonly IRepository<TodoItem> _toDoRepository;
        private readonly IRepository<AppUser> _users;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _currentUserId;
        private List<string> _userRoles;
        public UpdateTodoComandHandler(
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
        public async Task<TodoGetDto> Handle(UpdateTodoComand request, CancellationToken cancellationToken)
        {
            var todoEntity = new TodoItem();
            todoEntity = _mapper.Map<UpdateTodoComand, TodoItem>(request);
            var user = await _users.SingleOrDefaultAsync(i => i.Id == todoEntity.OwnerId);
            if (user == null)
            {
                throw new BadRequestExeption("Incorrect owner id");
            }
            _mapper.Map(request, todoEntity);
            todoEntity.UpdatedDate = DateTime.UtcNow;
            todoEntity.User = user;

            if (_currentUserId == todoEntity.OwnerId || _userRoles.Contains("Admin"))
            {
                var updatedItem = await _toDoRepository.UpdateAsync(todoEntity, cancellationToken);
                if (updatedItem is null)
                {
                    throw new Exception("Can not update ToDo item");
                }
                return _mapper.Map<TodoItem, TodoGetDto>(updatedItem);
            }
            else
            {
                throw new ForbidenExeption("Access denied");
            }
        }
    }
}
