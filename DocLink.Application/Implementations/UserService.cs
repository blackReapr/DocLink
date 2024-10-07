using AutoMapper;
using DocLink.Application.Dtos.UserDtos;
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
}
