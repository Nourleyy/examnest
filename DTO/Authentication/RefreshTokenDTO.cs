using FluentValidation;

namespace ExamNest.DTO.Authentication;

public class RefreshTokenDTO
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }


    class RefreshTokenDTOValidation : AbstractValidator<RefreshTokenDTO>
    {
        public RefreshTokenDTOValidation()
        {
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required.")
                                         .NotNull().WithMessage("Refresh token cannot be null.");

            RuleFor(x => x.AccessToken).NotEmpty().WithMessage("Access token is required.")
                                       .NotNull().WithMessage("Access token cannot be null.");


        }
    }
}