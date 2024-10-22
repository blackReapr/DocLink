using DocLink.Application.Dtos.UserDtos;
using DocLink.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocLink.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            IEnumerable<UserReturnDto> doctors = await _userService.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        [HttpGet("patients")]
        public async Task<IActionResult> GetPatients()
        {
            IEnumerable<UserReturnDto> doctors = await _userService.GetAllPatientAsync();
            return Ok(doctors);
        }

        [HttpGet("doctor/{id}")]
        public async Task<IActionResult> GetDoctor(string id)
        {
            DoctorReturnDto dto = await _userService.GetDoctorAsync(id);
            return Ok();
        }

        [HttpPost("doctor")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateDoctor(DoctorCreateDto doctorCreateDto)
        {
            await _userService.CreateDoctorAsync(doctorCreateDto);
            return Ok();
        }
    }
}
