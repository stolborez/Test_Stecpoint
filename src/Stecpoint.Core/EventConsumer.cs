using System.Threading.Tasks;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Stecpoint.Core.Commands;

namespace Stecpoint.Core
{
    public class EventConsumer : IConsumer<AddUserCommand>
    {
        readonly ILogger<EventConsumer> _logger;
        private readonly IMediator _mediator;

        public EventConsumer(ILogger<EventConsumer> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<AddUserCommand> context)
        {
            await _mediator.Send(context.Message);
            _logger.LogInformation("User added: Id={@Id}, FirstName={@FirstName}, LastName={@LastName}", context.Message.Id, context.Message.FirstName, context.Message.LastName);
            await Task.CompletedTask;
        }
    }
}