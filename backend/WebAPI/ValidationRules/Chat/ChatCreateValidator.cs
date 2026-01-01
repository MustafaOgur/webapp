using Core.Constants;
using FluentValidation;
using Model.DTOs.Chat;


namespace WebAPI.ValidationRules.Chat
{
    public class ChatCreateValidator : AbstractValidator<CreateChatDto>
    {
        public ChatCreateValidator() 
        {
            RuleFor(c => c).Must(dto => !string.IsNullOrWhiteSpace(dto.Name))
                .WithMessage("CreateChatDto tamamen boş olamaz.")
                .WithErrorCode(ErrorCodes.CHAT_EMPTY);

            RuleFor(c => c.Name).NotEmpty()
                .WithMessage("Chat name cannot be empty.")
                .WithErrorCode(ErrorCodes.CHAT_NAME_EMPTY);

            RuleFor(c => c.Name).MinimumLength(3)
                .WithMessage("Chat name must be at least 3 characters.");
                

            RuleFor(c => c.Name).MaximumLength(50)
                .WithMessage("Chat name cannot exceed 50 characters.")
                .WithErrorCode(ErrorCodes.CHAT_NAME_TOO_LONG);

            RuleFor(c => c.Name).Matches(@"^[a-zA-Z0-9\s]*$")
                .WithMessage("Chat name can only contain letters, numbers, and spaces.")
                .WithErrorCode(ErrorCodes.CHAT_NAME_INVALID_CHARS);

            RuleFor(c => c.Name).Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Chat name cannot be just whitespace.")
                .WithErrorCode(ErrorCodes.CHAT_NAME_WHITESPACE);

        }
    }
}
