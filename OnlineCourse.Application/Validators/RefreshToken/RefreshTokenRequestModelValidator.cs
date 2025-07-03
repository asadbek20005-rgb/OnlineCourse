using FluentValidation;
using OnlineCourse.Application.Models.RefreshToken;

namespace OnlineCourse.Application.Validators.RefreshToken;

public class RefreshTokenRequestModelValidator : AbstractValidator<RefreshTokenRequestModel>
{
    public RefreshTokenRequestModelValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("Access token is required.")
            .MinimumLength(20).WithMessage("Access token length is too short.")
            .Must(BeBase64OrJwt).WithMessage("Access token format is invalid.");

        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .MinimumLength(20).WithMessage("Refresh token length is too short.")
            .Must(BeBase64OrJwt).WithMessage("Refresh token format is invalid.");
    }


    private bool BeBase64OrJwt(string token)
    {
        return token.Count(c => c == '.') == 2 || IsBase64String(token);
    }

    private bool IsBase64String(string str)
    {
        Span<byte> buffer = new Span<byte>(new byte[str.Length]);
        return Convert.TryFromBase64String(str, buffer, out _);
    }
}
