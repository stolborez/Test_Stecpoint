using FluentValidation;
using Stecpoint.Core.Commands;

namespace Stecpoint.Core
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            RuleFor(command => command.Id).NotNull();
            RuleFor(command => command.FirstName).NotNull();
            RuleFor(command => command.LastName).NotNull();
            RuleFor(command => command.Number).NotNull();
            RuleFor(command => command.Email).NotNull();
        }
    }
}