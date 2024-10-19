using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace DocLink.Application.Dtos.AuthenticationDtos;

public class SignUpDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
    public string? Description { get; set; }
    public IFormFile? Profile { get; set; }
    public decimal? Price { get; set; }
}

public class SignUpDtoValidator : AbstractValidator<SignUpDto>
{
    public SignUpDtoValidator()
    {
        RuleFor(s => s.Name).NotEmpty().MaximumLength(20);
        RuleFor(s => s.Surname).NotEmpty().MaximumLength(20);
        RuleFor(s => s.Email).EmailAddress();
        RuleFor(s => s.Password).MinimumLength(6).MaximumLength(25);
        RuleFor(s => s.PasswordConfirm).Equal(s => s.Password);
        RuleFor(s => s.Description).MaximumLength(200);
    }
}