using DocLink.Application.Dtos.AppointmentDtos;

namespace DocLink.Application.Interfaces;

public interface IAppointmentService
{
    Task CreateAsync(CreateAppointmentDto createAppointmentDto);
    Task DeleteAsync(string id);
}