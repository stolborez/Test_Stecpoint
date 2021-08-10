using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Stecpoint.Data;
using Stecpoint.Infrastructure.Repositories;

namespace Stecpoint.Core.Commands
{
    public class AddUserCommand : IRequest<User>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }

        public class AddUserCommandHandler : IRequestHandler<AddUserCommand, User>
        {
            private readonly IRepository<User> _userRepository;
            private readonly IMapper _mapper;

            public AddUserCommandHandler(IRepository<User> userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<User> Handle(AddUserCommand command, CancellationToken cancellationToken)
            {
                var user = _mapper.Map<User>(command);
                return await _userRepository.AddAsync(user);
            }
        }
    }
}
