using DocLink.Application.Dtos.UserDtos;
using DocLink.Application.Interfaces;
using DocLink.Core.Entities;
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
    }
}
