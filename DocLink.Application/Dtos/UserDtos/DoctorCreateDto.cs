using DocLink.Application.Dtos.AuthenticationDtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace DocLink.Application.Dtos.UserDtos;

public class DoctorCreateDto
{
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public IFormFile Profile { get; set; }
    public string Description { get; set; }
    public DateTime BirthDate { get; set; }
    public decimal Price { get; set; }
}


public class DoctorCreateDtoValidator : AbstractValidator<DoctorCreateDto>
{
    public DoctorCreateDtoValidator()
    {
        RuleFor(s => s.Name).NotEmpty().MaximumLength(20);
        RuleFor(s => s.Surname).NotEmpty().MaximumLength(20);
        RuleFor(s => s.Email).EmailAddress();
        RuleFor(s => s.Password).MinimumLength(6).MaximumLength(25);
        RuleFor(s => s.PasswordConfirm).Equal(s => s.Password);
        RuleFor(s => s.Description).MaximumLength(200);
        RuleFor(s => s.Price).InclusiveBetween(50, 200);
        RuleFor(s => s.BirthDate).LessThan(DateTime.UtcNow - TimeSpan.FromDays(365 * 20));
    }
}
