using AutoMapper;
using DocLink.Application.Dtos.AppointmentDtos;
using DocLink.Application.Exceptions;
using DocLink.Application.Interfaces;
using DocLink.Core.Entities;
using DocLink.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocLink.Application.Implementations;

public class AppointmentService : IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly UserManager<AppUser> _userManager;

    public AppointmentService(IMapper mapper, DataContext context, UserManager<AppUser> userManager)
    {
        _mapper = mapper;
        _context = context;
        _userManager = userManager;
    }

    public async Task ChangeStatusAsync(string appointmentId, Status status)
    {
        Appointment? appointment = await _context.Appointments.SingleOrDefaultAsync(a => a.Id.ToString() == appointmentId);
        if (appointment == null) throw new CustomException(404, "Appointment does not exist");
        appointment.Status = status;
        await _context.SaveChangesAsync();
    }

    public async Task CreateAsync(AppointmentCreateDto createAppointmentDto, string patientId)
    {
        AppUser? patient = await _context.Users.SingleOrDefaultAsync(u => u.Id == patientId);
        if (patient is null || !await _userManager.IsInRoleAsync(patient, "member")) throw new CustomException(404, "Patient does not exist");

        DateTime EndTime = createAppointmentDto.StartTime + TimeSpan.FromMinutes(30);

        AppUser? doctor = await _context.Users.SingleOrDefaultAsync(u => u.Id == createAppointmentDto.DoctorId);
        if (doctor is null || !await _userManager.IsInRoleAsync(doctor, "doctor")) throw new CustomException(404, "Doctor does not exist");

        if (createAppointmentDto.StartTime < DateTime.UtcNow || EndTime <= DateTime.UtcNow) throw new CustomException(400, "Invalid date");

        if (await _context.Appointments.AnyAsync(a => a.StartTime < EndTime && a.EndTime > createAppointmentDto.StartTime)) throw new CustomException(400, "Busy schedule");

        Appointment appointment = _mapper.Map<Appointment>(createAppointmentDto);
        appointment.Status = Status.PENDING;
        appointment.Price = doctor.Price.Value;
        appointment.EndTime = EndTime;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        Appointment? appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) throw new CustomException(404, "Appointment not found");
        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task<AppointmentReturnDto> DetailsAsync(string appointmentId)
    {
        Appointment? appointment = await _context.Appointments.SingleOrDefaultAsync(a => a.Id.ToString() == appointmentId);
        if (appointment == null) throw new CustomException(404, "Appointment does not exist");
        AppointmentReturnDto returnDto = _mapper.Map<AppointmentReturnDto>(appointment);
        return returnDto;
    }
}
