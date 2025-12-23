using Core.Constants;
using FluentValidation;

namespace WebAPI.ValidationRules
{
    public class IdValidator : AbstractValidator<string>
    {
        public IdValidator() 
        {
            RuleFor(id => id).NotEmpty()
                .WithMessage("Id cannot be empty")
                .WithErrorCode(ErrorCodes.ID_EMPTY);

            RuleFor(id => id).Matches(@"^[a-zA-Z0-9\-]+$")
                .WithMessage("Id must be a valid string.")
                .WithErrorCode(ErrorCodes.ID_INVALID);
        }
    }
}
