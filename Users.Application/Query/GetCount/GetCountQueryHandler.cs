using Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using UserServices.dto;
using Microsoft.Extensions.Caching.Memory;
using Common.Application.Abstraction.Percistance;
using Application.Common.Domain;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.User.Application.Query.GetCount
{
    public class GetCountQueryHandler : IRequestHandler<GetCountQuery, int>
    {
        private readonly IRepository<AppUser> _repository;
        private readonly MemoryCache _memoryCeche;

        public GetCountQueryHandler(IRepository<AppUser> repository, UserMemoryCache memoryCeche)
        {
            _repository = repository;
            _memoryCeche = memoryCeche.Cache;
        }

        public async Task<int> Handle(GetCountQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = JsonSerializer.Serialize($"Count: {request}", new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            if (_memoryCeche.TryGetValue(cacheKey, out int? result))
            {
                return result!.Value;
            }
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                result = await _repository.CountAsync(token: cancellationToken);
            }
            result = await _repository.CountAsync(u => u.Login.Contains(request.Name));

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSize(1);
            _memoryCeche.Set(cacheKey, result, cacheEntryOptions);

            return result!.Value;
        }

    }
}
