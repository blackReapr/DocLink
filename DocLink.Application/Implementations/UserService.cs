using AutoMapper;
using DocLink.Application.Dtos.AuthenticationDtos;
using DocLink.Application.Dtos.UserDtos;
using DocLink.Application.Exceptions;
using DocLink.Application.Extensions;
using DocLink.Application.Interfaces;
using DocLink.Core.Entities;
using DocLink.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocLink.Application.Implementations;

public class UserService : IUserService
{
    private readonly DataContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public UserService(DataContext context, UserManager<AppUser> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserReturnDto>> GetAllUsersAsync()
    {
        IEnumerable<AppUser> appUsers = await _context.Users.AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<UserReturnDto>>(appUsers);
    }

    public async Task<IEnumerable<UserReturnDto>> GetAllDoctorsAsync()
    {
        IEnumerable<AppUser> appUsers = await _userManager.Users.AsNoTracking().ToListAsync();
        appUsers = appUsers.Where(u => _userManager.IsInRoleAsync(u, "doctor").Result);
        return _mapper.Map<List<UserReturnDto>>(appUsers);
    }

    public async Task<IEnumerable<UserReturnDto>> GetAllPatientAsync()
    {
        IEnumerable<AppUser> appUsers = await _userManager.Users.AsNoTracking().ToListAsync();
        appUsers = appUsers.Where(u => _userManager.IsInRoleAsync(u, "member").Result);
        return _mapper.Map<List<UserReturnDto>>(appUsers);
    }

    public async Task<DoctorReturnDto> GetDoctorAsync(string id)
    {
        AppUser? appUser = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id && _userManager.IsInRoleAsync(u, "doctor").Result);
        if (appUser == null) throw new CustomException(404, "Doctor does not exist");
        IEnumerable<DateTime> appointments = await _context.Appointments.AsNoTracking().Where(a => a.DoctorId == id).Select(a => a.StartTime).ToListAsync();
        DoctorReturnDto dto = _mapper.Map<DoctorReturnDto>(appUser);
        dto.Appointments = appointments;
        return dto;
    }

    public async Task<string> CreateDoctorAsync(DoctorCreateDto dto)
    {
        if (dto.Password != dto.PasswordConfirm) throw new CustomException(400, "Passwords do not match");
        AppUser user = new() { Name = dto.Name, Email = dto.Email, Surname = dto.Surname, UserName = dto.Email };
        IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) throw new CustomException(400, result.Errors.First().Code.ToString());
        if (dto.Profile != null)
        {
            if (!dto.Profile.IsImage()) throw new CustomException(400, "Invalid file format");
            if (dto.Profile.DoesSizeExceed(100)) throw new CustomException(400, "File size exceeds the limit");
            string filename = await dto.Profile.SaveFileAsync();
            user.Profile = filename;
        }
        await _userManager.AddToRoleAsync(user, "doctor");
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }
}
