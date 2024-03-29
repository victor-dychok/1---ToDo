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

namespace ToDo.Application.Comands.Create
{
    public class CreateToDoComandHandler : IRequestHandler<CreateToDoComand, TodoGetDto>
    {
        private readonly IRepository<TodoItem> _toDoRepository;
        private readonly IRepository<AppUser> _users;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _currentUserId;
        private List<string> _userRoles;
        public CreateToDoComandHandler(
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
        public async Task<TodoGetDto> Handle(CreateToDoComand request, CancellationToken cancellationToken)
        {
            int ownerId;
            if (_userRoles.Contains("Admin"))
            {
                ownerId = request.OwnerId;
            }
            else
            {
                ownerId = _currentUserId;
                request.OwnerId = _currentUserId;
            }
            var user = await _users.SingleOrDefaultAsync(u => u.Id == ownerId);

            if (user == null)
            {
                throw new BadRequestExeption("Incorrect id");
            }

            var todoEntity = _mapper.Map<CreateToDoComand, TodoItem>(request);
            todoEntity.CreatedDate = DateTime.UtcNow;
            todoEntity.UpdatedDate = DateTime.UtcNow;
            todoEntity.User = user;

            var addedItem = await _toDoRepository.AddAsync(todoEntity);

            if (addedItem is null)
            {
                throw new BadRequestExeption("Can not creade ToDo item");
            }

            return _mapper.Map<TodoItem, TodoGetDto>(addedItem);
        }
    }
}
