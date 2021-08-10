using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stecpoint.Core.Commands;
using Stecpoint.Core.Queries;
using Stecpoint.Data;
using Stecpoint.Infrastructure.Repositories;

namespace Stecpoint.ServiceB.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;

        public UserController(ILogger<UserController> logger, IMediator mediator, IRepository<User> userRepository, IRepository<Organization> organizationRepository)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        /// <summary>
        ///     Связывание пользователя и организации
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("link")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] LinkUserCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            _logger.LogInformation("The command({@command}) has been sent to the queue: OrganizationId=[{@OrganizationId}], UserId= [{@UserId}]", command, command.OrganizationId, command.UserId);
            return Ok();
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetOrdersByUserName([FromQuery] GetUsersQuery query)
        {
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }
    }
}