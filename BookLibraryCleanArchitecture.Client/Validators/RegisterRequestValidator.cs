using BookLibraryCleanArchitecture.Application.Dtos;
using FluentValidation;
using BookLibraryCleanArchitecture.Common.Constants;

namespace BookLibraryCleanArchitecture.Client.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .Must(x => x == null || x.Trim().Length > 0).WithMessage(ValidationMessages.FirstNameRequired)
                .MaximumLength(ValidationLengths.FirstNameMaxLength).WithMessage(ValidationMessages.InvalidFirstNameLength);

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .Must(x => x == null || x.Trim().Length > 0).WithMessage(ValidationMessages.LastNameRequired)
                .MaximumLength(ValidationLengths.LastNameMaxLength).WithMessage(ValidationMessages.InvalidLastNameLength);

            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.Stop)
                .Must(x => x == null || x.Trim().Length > 0).WithMessage(ValidationMessages.UserNameRequired)
                .MaximumLength(ValidationLengths.UserNameMaxLength).WithMessage(ValidationMessages.InvalidUserNameLength);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(ValidationMessages.PasswordRequired)
                .MinimumLength(ValidationLengths.PasswordMinLength).WithMessage(ValidationMessages.PasswordMinLengthMessage)
                .MaximumLength(ValidationLengths.PasswordMaxLength).WithMessage(ValidationMessages.PasswordMaxLengthMessage)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$")
                    .WithMessage(ValidationMessages.PasswordComplexity);

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .Must(x => x == null || x.Trim().Length > 0).WithMessage(ValidationMessages.EmailRequired)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmail)
                .MaximumLength(ValidationLengths.EmailMaxLength).WithMessage(ValidationMessages.InvalidEmailLength);
        }
    }
}
