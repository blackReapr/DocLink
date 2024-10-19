using DocLink.Application.Dtos.AppointmentDtos;
using DocLink.Application.Interfaces;
using DocLink.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocLink.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly UserManager<AppUser> _userManager;

        public AppointmentController(IAppointmentService appointmentService, UserManager<AppUser> userManager)
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
        }

        [HttpPost("")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> Create(AppointmentCreateDto createAppointmentDto)
        {
            AppUser? appUser = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == User.Identity.Name);
            if (appUser == null || !await _userManager.IsInRoleAsync(appUser, "member")) return Unauthorized();
            await _appointmentService.CreateAsync(createAppointmentDto, appUser.Id);
            return Ok();
        }

        [HttpPut("status/{appointmentId}")]
        [Authorize(Roles = "doctor")]
        public async Task<IActionResult> Accept(string appointmentId)
        {
            AppUser? appUser = await _userManager.Users.SingleOrDefaultAsync(u => u.Name == User.Identity.Name);
            if (appUser == null || !await _userManager.IsInRoleAsync(appUser, "doctor")) return Unauthorized();
            await _appointmentService.ChangeStatusAsync(appointmentId, Status.ACCEPTED);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string appointmentId)
        {
            AppointmentReturnDto returnDto = await _appointmentService.DetailsAsync(appointmentId);
            return Ok(returnDto);
        }
    }
}
