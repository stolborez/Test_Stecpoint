using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Stecpoint.Data;
using Stecpoint.Infrastructure.Repositories;

namespace Stecpoint.Core.Commands
{
    public class LinkUserCommand : IRequest<User>
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }

        public class LinkUserCommandHandler : IRequestHandler<LinkUserCommand, User>
        {
            private readonly IRepository<User> _userRepository;
            private readonly IRepository<Organization> _organizationRepository;

            public LinkUserCommandHandler(IRepository<User> userRepository, IRepository<Organization> organizationRepository)
            {
                _userRepository = userRepository;
                _organizationRepository = organizationRepository;
            }

            public async Task<User> Handle(LinkUserCommand command, CancellationToken cancellationToken)
            {
                User user = _userRepository.GetAll().FirstOrDefault(_ => _.Id == command.UserId );
                if (user == null) throw new Exception($"User({command.UserId}) not found");

                Organization organization = _organizationRepository.GetAll().FirstOrDefault(_ => _.Id == command.OrganizationId );
                if (user == null) throw new Exception($"Organization({command.OrganizationId}) not found");

                user.Organization = organization;
                return await _userRepository.UpdateAsync(user);
            }
        }
    }
}
