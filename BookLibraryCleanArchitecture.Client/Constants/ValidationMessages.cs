namespace BookLibraryCleanArchitecture.Client.Constants
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
    }
}
