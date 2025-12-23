using Core.Constants;
using FluentValidation;
using Model.DTOs.Message;


namespace WebAPI.ValidationRules.Message
{
    public class CreateMessageValidator : AbstractValidator<CreateMessageDto>
    {
        public CreateMessageValidator() 
        {
            RuleFor(m => m).Must(dto => !string.IsNullOrWhiteSpace(dto.Content))
                .WithMessage("CreateMessageDto tamamen boş olamaz.")
                .WithErrorCode(ErrorCodes.MESSAGE_EMPTY);

            RuleFor(m => m.Content).NotEmpty()
                .WithMessage("Message content cannot be empty.")
                .WithErrorCode(ErrorCodes.MESSAGE_CONTENT_EMPTY);

            RuleFor(m => m.Content).Length(1, 1000)
                .WithMessage("Message content must be between 1 and 1000 characters.")
                .WithErrorCode(ErrorCodes.MESSAGE_CONTENT_TOO_LONG);

            RuleFor(m => m.Content).Must(c => !string.IsNullOrWhiteSpace(c))
                .WithMessage("Message content cannot be just whitespace.")
                .WithErrorCode(ErrorCodes.MESSAGE_CONTENT_WHITESPACE);
        }
    }
}
