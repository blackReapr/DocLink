using FluentValidation;

namespace DocLink.Application.Dtos.AppointmentDtos;

public class CreateAppointmentDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string DoctorId { get; set; }
    public string PatientId { get; set; }
}

public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
{
    public CreateAppointmentDtoValidator()
    {
        RuleFor(c => c.StartTime).NotEmpty().GreaterThan(DateTime.Now);
        RuleFor(c => c.EndTime).NotEmpty().GreaterThan(c => c.StartTime);
        RuleFor(c => c.DoctorId).NotEmpty();
        RuleFor(c => c.PatientId).NotEmpty();
    }
}
