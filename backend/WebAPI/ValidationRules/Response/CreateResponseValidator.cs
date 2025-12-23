using Core.Constants;
using FluentValidation;
using Model.DTOs.Response;


namespace WebAPI.ValidationRules.Response
{
    public class CreateResponseValidator : AbstractValidator<CreateResponseDto>
    {
        public CreateResponseValidator() 
        {
            RuleFor(r => r).Must(dto => !string.IsNullOrWhiteSpace(dto.Content))
                .WithMessage("CreateResponseDto tamamen boş olamaz.")
                .WithErrorCode(ErrorCodes.RESPONSE_EMPTY);

            RuleFor(r => r.Content).NotEmpty()
                .WithMessage("Response content cannot be empty.")
                .WithErrorCode(ErrorCodes.RESPONSE_CONTENT_EMPTY);

            RuleFor(r => r.Content).Must(c => !string.IsNullOrWhiteSpace(c))
                .WithMessage("Message content cannot be just whitespace.")
                .WithErrorCode(ErrorCodes.RESPONSE_CONTENT_WHITESPACE);
        }
    }
}
