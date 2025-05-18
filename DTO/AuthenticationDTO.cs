using FluentValidation;

namespace ExamNest.DTO
{
    public class AuthenticationDTO
    {
        public string Email { get; set; } 
        public string Password { get; set; }
    }
    public class AuthenticationDTOValidation : AbstractValidator<AuthenticationDTO>
    {
        public AuthenticationDTOValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");
        }
    }
}
