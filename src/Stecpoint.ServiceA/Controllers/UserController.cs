using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stecpoint.Core.Commands;

namespace Stecpoint.ServiceA.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public UserController(ILogger<UserController> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        ///     Создание пользователя
        /// </summary>
        /// <param name="addUserCommand"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] AddUserCommand addUserCommand, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _publishEndpoint.Publish(addUserCommand, cancellationToken);
            _logger.LogInformation("The command({@AddUserCommand}) has been sent to the queue: Id={@Id}, FirstName={@FirstName}, LastName={@LastName}", addUserCommand, addUserCommand.Id, addUserCommand.FirstName, addUserCommand.LastName);
            return Ok();
        }
    }
}