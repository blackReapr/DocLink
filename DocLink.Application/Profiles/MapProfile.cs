using AutoMapper;
using DocLink.Application.Dtos.AppointmentDtos;
using DocLink.Core.Entities;

namespace DocLink.Application.Profiles;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<CreateAppointmentDto, Appointment>();
    }
}
