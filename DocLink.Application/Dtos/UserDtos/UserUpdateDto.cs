using FluentValidation;

namespace DocLink.Application.Dtos.UserDtos;

public class UserUpdateDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Description { get; set; }
    public DateTime BirthDate { get; set; }
    public decimal Price { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
}

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        RuleFor(s => s.Name).NotEmpty().MaximumLength(20);
        RuleFor(s => s.Surname).NotEmpty().MaximumLength(20);
        RuleFor(s => s.Password).Empty().MinimumLength(6).MaximumLength(25);
        RuleFor(s => s.PasswordConfirm).Equal(s => s.Password);
        RuleFor(s => s.Description).MaximumLength(200);
        RuleFor(s => s.BirthDate).LessThan(DateTime.UtcNow - TimeSpan.FromDays(365 * 20));
        RuleFor(s => s.Price).InclusiveBetween(50, 200);
    }
}
