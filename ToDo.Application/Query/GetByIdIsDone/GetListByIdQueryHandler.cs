using Application.Common.Domain;
using AutoMapper;
using Common.Api.Service;
using Common.Application.Abstraction.Percistance;
using Common.Application.Exeptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoDomain;

namespace ToDo.Application.Query.GetByIdIsDone
{
    public class GetListByIdQueryHandler : IRequestHandler<GetByIdIsDoneQuery, TodoItem>
    {
        private readonly IRepository<TodoItem> _toDoRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly MemoryCache _memoryCeche;
        private int _currentUserId;
        private List<string> _userRoles;
        public GetListByIdQueryHandler(
            IRepository<TodoItem> repository,
            ToDoMemoryCashe memoryCeche,
            ICurrentUserService currentUserService)
        {

            _currentUserService = currentUserService;
            _toDoRepository = repository;
            _memoryCeche = memoryCeche.Cache;
            _currentUserId = int.Parse(_currentUserService.CurrentUserId);
            _userRoles = _currentUserService.CurrentUserRoles.ToList();
        }
        public async Task<TodoItem> Handle(GetByIdIsDoneQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = JsonSerializer.Serialize(request, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            if (_memoryCeche.TryGetValue(cacheKey, out TodoItem? result))
            {
                return result!;
            }

            result = await _toDoRepository.SingleOrDefaultAsync(d => d.Id == request.Id);
            if (result == null)
            {
                throw new NotFoundExeption(new { Id = request.Id });
            }
            if (_currentUserId == result.OwnerId || _userRoles.Contains("Admin"))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSize(5);
                _memoryCeche.Set(cacheKey, result, cacheEntryOptions);

                return result;
            }
            else throw new ForbidenExeption("Access denied");
        }
    }
}
