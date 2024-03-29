using Application.Common.Domain;
using AutoMapper;
using Common.Application.Abstraction.Percistance;
using Common.Application.Exeptions;
using Common.Repository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Users.Application.Query.GetById;
using UserServices.dto;

namespace Application.User.Application.Query.GetById
{
    public class GetByIdOrDefaultQueryHandler : IRequestHandler<GetByIdQuery, GetUserDto>
    {
        private readonly IRepository<AppUser> _repository;
        private readonly MemoryCache _memoryCeche;
        private readonly IMapper _mapper;

        public GetByIdOrDefaultQueryHandler(IRepository<AppUser> repository, UserMemoryCache memoryCeche, IMapper mapper)
        {
            _repository = repository;
            _memoryCeche = memoryCeche.Cache;
            _mapper = mapper;
        }

        public async Task<GetUserDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = JsonSerializer.Serialize(request, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            if (_memoryCeche.TryGetValue(cacheKey, out GetUserDto? result))
            {
                return result!;
            }

            var item = await _repository.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (item is null)
            {
                throw new NotFoundExeption(new { Id = request.Id });
            }
            result = _mapper.Map<GetUserDto>(item);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSize(5);
            _memoryCeche.Set(cacheKey, result, cacheEntryOptions);

            return result;
        }
    }
}
