using FluentValidation;

namespace DocLink.Application.Dtos.AuthenticationDtos;

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(s => s.Email).EmailAddress();
        RuleFor(s => s.Password).MinimumLength(6).MaximumLength(25);
    }
}
