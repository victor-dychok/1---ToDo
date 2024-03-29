using Application.Common.Domain;
using AutoMapper;
using Common.Api.Service;
using Common.Application.Abstraction.Percistance;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using ToDo.Application.dto;
using ToDoDomain;
using System.Collections.ObjectModel;

namespace ToDo.Application.Query.GetList
{
    public class GetListQueryHandler : IRequestHandler<GetListQuery, IReadOnlyCollection<TodoItem>>
    {
        private readonly IRepository<TodoItem> _toDoRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly MemoryCache _memoryCeche;
        private int _currentUserId;
        private List<string> _userRoles;
        public GetListQueryHandler(
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
        public async Task<IReadOnlyCollection<TodoItem>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = JsonSerializer.Serialize(request, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            if (_memoryCeche.TryGetValue(cacheKey, out IReadOnlyCollection<TodoItem>? result))
            {
                return result!;
            }
            if (_userRoles.Contains("Admin"))
            {
                result = await _toDoRepository.GetListAsync(
                request.Offset,
                request.Limit,
                d => (string.IsNullOrWhiteSpace(request.Lable) || d.Label.Contains(request.Lable, StringComparison.InvariantCultureIgnoreCase))
                && (request.OwnerId == null || d.OwnerId == request.OwnerId),
                t => t.Id);

            }
            else
            {
                result = await _toDoRepository.GetListAsync(
                request.Offset,
                request.Limit,
                d => (string.IsNullOrWhiteSpace(request.Lable) || d.Label.Contains(request.Lable, StringComparison.InvariantCultureIgnoreCase))
                && (d.OwnerId == _currentUserId),
                t => t.Id);
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSize(5);
            _memoryCeche.Set(cacheKey, result, cacheEntryOptions);

            return result;
        }
    }
}
