using DocLink.Application.Dtos.AppointmentDtos;
using DocLink.Core.Entities;

namespace DocLink.Application.Interfaces;

public interface IAppointmentService
{
    Task CreateAsync(AppointmentCreateDto createAppointmentDto, string patientId);
    Task DeleteAsync(string id);
    Task ChangeStatusAsync(string appointmentId, Status status);
    Task<AppointmentReturnDto> DetailsAsync(string appointmentId);
}