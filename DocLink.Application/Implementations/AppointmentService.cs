using AutoMapper;
using DocLink.Application.Dtos.AppointmentDtos;
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
        if (appointment == null) throw new Exception("Appointment does not exist");
        appointment.Status = status;
        await _context.SaveChangesAsync();
    }

    public async Task CreateAsync(AppointmentCreateDto createAppointmentDto, string patientId)
    {
        AppUser? patient = await _context.Users.SingleOrDefaultAsync(u => u.Id == patientId);
        if (patient is null || !await _userManager.IsInRoleAsync(patient, "member")) throw new Exception("Invalid patient Id");

        AppUser? doctor = await _context.Users.SingleOrDefaultAsync(u => u.Id == createAppointmentDto.DoctorId);
        if (doctor is null || !await _userManager.IsInRoleAsync(doctor, "doctor")) throw new Exception("Invalid doctor Id");

        if (createAppointmentDto.StartTime < DateTime.UtcNow || createAppointmentDto.EndTime <= DateTime.UtcNow) throw new Exception("Invalid date");

        if (createAppointmentDto.EndTime - createAppointmentDto.StartTime != TimeSpan.FromMinutes(30)) throw new Exception("Invalid interval");

        if (await _context.Appointments.AnyAsync(a => a.StartTime < createAppointmentDto.EndTime && a.EndTime > createAppointmentDto.StartTime)) throw new Exception("Busy schedule");

        Appointment appointment = _mapper.Map<Appointment>(createAppointmentDto);
        appointment.Status = Status.PENDING;
        appointment.Price = doctor.Price;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        Appointment? appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) throw new Exception("Appointment not found");
        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task<AppointmentReturnDto> DetailsAsync(string appointmentId)
    {
        Appointment? appointment = await _context.Appointments.SingleOrDefaultAsync(a => a.Id.ToString() == appointmentId);
        if (appointment == null) throw new Exception("Appointment does not exist");
        AppointmentReturnDto returnDto = _mapper.Map<AppointmentReturnDto>(appointment);
        return returnDto;
    }

    //public async Task<AppointmentReturnDto> DetailsAsync(string appointmentId)
    //{
    //    Appointment? appointment = await _context.Appointments.SingleOrDefaultAsync(a => a.Id.ToString() == appointmentId);
    //    if (appointment == null) throw new Exception("Appointment does not exist");
    //    AppointmentReturnDto returnDto = _mapper.Map<AppointmentReturnDto>(appointment);
    //    return returnDto;
    //}
}
