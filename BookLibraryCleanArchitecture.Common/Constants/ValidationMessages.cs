namespace BookLibraryCleanArchitecture.Common.Constants
{
    public class ValidationMessages
    {
        public const string FirstNameRequired = "First name is required.";
        public const string LastNameRequired = "Last name is required.";
        public const string UserNameRequired = "User name is required.";
        public const string PasswordRequired = "Password is required.";
        public const string EmailRequired = "Email is required.";
        public const string InvalidEmail = "Invalid email address.";

        public static readonly string InvalidFirstNameLength = $"First name must be at most {ValidationLengths.FirstNameMaxLength} characters.";
        public static readonly string InvalidLastNameLength = $"Last name must be at most {ValidationLengths.LastNameMaxLength} characters.";
        public static readonly string InvalidUserNameLength = $"User name must be at most {ValidationLengths.UserNameMaxLength} characters.";
        public static readonly string InvalidEmailLength = $"Email must be at most {ValidationLengths.EmailMaxLength} characters.";

        public static readonly string PasswordMinLengthMessage = $"Password must be at least {ValidationLengths.PasswordMinLength} characters.";
        public static readonly string PasswordMaxLengthMessage = $"Password must be at most {ValidationLengths.PasswordMaxLength} characters.";
        public static readonly string EmailMaxLengthMessage = $"Email must be at most {ValidationLengths.EmailMaxLength} characters.";
        public static readonly string PasswordComplexity = "Password must contain uppercase, lowercase, digit and special character.";

        public static readonly string GenericRegistrationError = $"An error occured during user Registration.";
        public static readonly string GenericAutheticationError = $"An error occured during user Authetication.";
        public static readonly string GenericTokenGenerationError = $"An error occured during Token Generation.";
        public static readonly string GenericUnhandledExceptionError = $"Internal Server error.";

        public static readonly string AuthenticationErrorTitle = "Authentication Error";
        public static readonly string RegistrationErrorTitle = "Registration Error";
        public static readonly string TokenGenerationErrorTitle = "Token Generation Error";
        public static readonly string ValidationErrorTitle = "Validation Error";
        public static readonly string UnhandledExceptionErrorTitle = "An unexpected error occurred.";
    }
}
