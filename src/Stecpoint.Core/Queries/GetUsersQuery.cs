using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stecpoint.Core.DTO;
using Stecpoint.Data;
using Stecpoint.Infrastructure.Repositories;

namespace Stecpoint.Core.Queries
{
    public class GetUsersQuery : IRequest<List<UserDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
        {
            private readonly IRepository<User> _userRepository;
            private readonly IMapper _mapper;

            public GetUsersHandler(IRepository<User> userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                var user = _userRepository.GetAll().Include(_ => _.Organization).ToList();
                return _mapper.Map<List<UserDto>>(user);
            }
        }
    }
}