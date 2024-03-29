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
using ToDoDomain;

namespace ToDo.Application.Comands.Delete
{
    public class DeleteTodoComandHandler : IRequestHandler<DeleteTodoComand, bool>
    {
        private readonly IRepository<TodoItem> _toDoRepository;
        private readonly ICurrentUserService _currentUserService;
        private int _currentUserId;
        private List<string> _userRoles;
        public DeleteTodoComandHandler(
            IRepository<TodoItem> repository,
            ICurrentUserService currentUserService,
            IRepository<AppUser> user,
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _toDoRepository = repository;
            _currentUserId = int.Parse(_currentUserService.CurrentUserId);
            _userRoles = _currentUserService.CurrentUserRoles.ToList();
        }
        public async Task<bool> Handle(DeleteTodoComand request, CancellationToken cancellationToken)
        {
            var item = await _toDoRepository.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (_currentUserId == request.Id || _userRoles.Contains("Admin"))
            {
                return await _toDoRepository.DeleteAsync(item);
            }
            else
            {
                throw new ForbidenExeption("Access denied");
            }
        }
    }
}
