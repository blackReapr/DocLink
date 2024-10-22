using FluentValidation;

namespace DocLink.Application.Dtos.AppointmentDtos;

public class AppointmentCreateDto
{
    public DateTime StartTime { get; set; }
    public string DoctorId { get; set; }
}

public class CreateAppointmentDtoValidator : AbstractValidator<AppointmentCreateDto>
{
    public CreateAppointmentDtoValidator()
    {
        RuleFor(c => c.StartTime).NotEmpty().GreaterThan(DateTime.Now);
        RuleFor(c => c.DoctorId).NotEmpty();
    }
}
