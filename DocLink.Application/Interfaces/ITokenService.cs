using DocLink.Core.Entities;

namespace DocLink.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateJWTToken(AppUser user);
    }
}