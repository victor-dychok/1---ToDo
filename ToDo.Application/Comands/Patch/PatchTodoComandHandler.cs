using Application.Common.Domain;
using AutoMapper;
using Common.Api.Service;
using Common.Application.Abstraction.Percistance;
using Common.Application.Exeptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.dto;
using ToDoDomain;

namespace ToDo.Application.Comands.Patch
{
    public class PatchTodoComandHandler : IRequestHandler<PatchTodoComand, TodoGetDto>
    {
        private readonly IRepository<TodoItem> _toDoRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _currentUserId;
        private List<string> _userRoles;
        public PatchTodoComandHandler(
            IRepository<TodoItem> repository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _toDoRepository = repository;
            _currentUserId = int.Parse(_currentUserService.CurrentUserId);
            _userRoles = _currentUserService.CurrentUserRoles.ToList();
            _mapper = mapper;
        }
        public async Task<TodoGetDto> Handle(PatchTodoComand request, CancellationToken cancellationToken)
        {
            var todoItem = await _toDoRepository.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (todoItem == null)
            {
                throw new NotFoundExeption("Todo item not found");
            }

            todoItem.IsDone = request.IsDone;

            if (_currentUserId == todoItem.OwnerId || _userRoles.Contains("Admin"))
            {

                var putchedItem = await _toDoRepository.UpdateAsync(todoItem);
                if (putchedItem is null)
                {
                    throw new BadRequestExeption("Can not create item");
                }
                return _mapper.Map<TodoItem, TodoGetDto>(putchedItem);
            }
            else
            {
                throw new ForbidenExeption("Access denied");
            }
        }
    }
}
