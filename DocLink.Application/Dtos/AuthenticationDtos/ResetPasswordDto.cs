using FluentValidation;

namespace DocLink.Application.Dtos.AuthenticationDtos;

public class ResetPasswordDto
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(r => r.Email).EmailAddress();
        RuleFor(r => r.Password).MinimumLength(8).MaximumLength(25);
        RuleFor(r => r.Token).NotEmpty();
        RuleFor(r => r.ConfirmPassword).Equal(r => r.Password);
    }
}
