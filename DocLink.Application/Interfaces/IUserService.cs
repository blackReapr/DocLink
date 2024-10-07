using DocLink.Application.Dtos.UserDtos;
using DocLink.Core.Entities;

namespace DocLink.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserReturnDto>> GetAllDoctorsAsync();
        Task<IEnumerable<UserReturnDto>> GetAllPatientAsync();
        Task<IEnumerable<UserReturnDto>> GetAllUsersAsync();
    }
}