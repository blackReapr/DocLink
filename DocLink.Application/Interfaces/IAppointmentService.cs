using DocLink.Application.Dtos.AppointmentDtos;

namespace DocLink.Application.Interfaces;

public interface IAppointmentService
{
    Task CreateAsync(AppointmentCreateDto createAppointmentDto, string patientId);
    Task DeleteAsync(string id);
    Task AcceptAsync(string appointmentId);
    Task RejectAsync(string appointmentId);
    Task<AppointmentReturnDto> DetailsAsync(string appointmentId);
}