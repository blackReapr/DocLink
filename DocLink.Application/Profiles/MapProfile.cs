using AutoMapper;
using DocLink.Application.Dtos.AppointmentDtos;
using DocLink.Application.Dtos.UserDtos;
using DocLink.Core.Entities;

namespace DocLink.Application.Profiles;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<AppointmentCreateDto, Appointment>();
        CreateMap<AppUser, UserReturnDto>();
        CreateMap<Appointment, AppointmentReturnDto>();
        CreateMap<AppUser, DoctorReturnDto>();
    }
}
