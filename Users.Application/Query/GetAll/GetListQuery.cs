using MediatR;
using UserServices.dto;

namespace Application.Users.Application.Query.GetAll
{
    public class GetListQuery : BaseUsersFilter, IRequest<IReadOnlyCollection<GetUserDto>>
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
    }
}
