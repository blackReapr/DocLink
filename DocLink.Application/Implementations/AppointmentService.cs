using AutoMapper;
using DocLink.Application.Dtos.AppointmentDtos;
using DocLink.Application.Interfaces;
using DocLink.Core.Entities;
using DocLink.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace DocLink.Application.Implementations;

public class AppointmentService : IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public AppointmentService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task CreateAsync(CreateAppointmentDto createAppointmentDto)
    {
        if (!await _context.Users.AnyAsync(u => u.Id == createAppointmentDto.DoctorId)) throw new Exception("Invalid doctor id");
        if (!await _context.Users.AnyAsync(u => u.Id == createAppointmentDto.PatientId)) throw new Exception("Invalid patientid");
        if (await _context.Appointments.AnyAsync(a => a.StartTime < createAppointmentDto.EndTime && a.EndTime > createAppointmentDto.StartTime)) throw new Exception("Busy schedule");
        Appointment appointment = _mapper.Map<Appointment>(createAppointmentDto);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        Appointment? appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) throw new Exception("Appointment not found");
        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
    }
}
