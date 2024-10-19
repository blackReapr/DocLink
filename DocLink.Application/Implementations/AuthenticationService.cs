using DocLink.Application.Dtos.AuthenticationDtos;
using DocLink.Application.Extensions;
using DocLink.Application.Interfaces;
using DocLink.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocLink.Application.Implementations;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthenticationService(UserManager<AppUser> userManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _roleManager = roleManager;
    }

    public async Task<string> RegisterAsync(SignUpDto signUpDto)
    {
        if (signUpDto.Password != signUpDto.PasswordConfirm) throw new Exception("Passwords do not match");
        AppUser user = new() { Name = signUpDto.Name, Email = signUpDto.Email, Surname = signUpDto.Surname, UserName = signUpDto.Email };
        IdentityResult result = await _userManager.CreateAsync(user, signUpDto.Password);
        if (!result.Succeeded) throw new Exception(result.Errors.First().Code.ToString());
        if (signUpDto.Profile != null)
        {
            if (!signUpDto.Profile.IsImage()) throw new Exception("Invalid file format");
            if (signUpDto.Profile.DoesSizeExceed(100)) throw new Exception("File size exceeds the limit");
            string filename = await signUpDto.Profile.SaveFileAsync();
            user.Profile = filename;
        }
        await _userManager.AddToRoleAsync(user, "member");
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null) throw new Exception("User not found");

        bool doesPasswordMatch = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!doesPasswordMatch) throw new Exception("Incorrect credentials");

        if (!user.EmailConfirmed) throw new Exception("Confirm your email");

        string token = _tokenService.GenerateJWTToken(user);
        return token;
    }

    public async Task ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
    {
        AppUser? appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == confirmEmailDto.Email);
        if (appUser == null) throw new Exception($"Unable to load user {confirmEmailDto.Email}");
        await _userManager.ConfirmEmailAsync(appUser, confirmEmailDto.Token);
        await _userManager.UpdateSecurityStampAsync(appUser);
    }

    public async Task<string> ForgotPasswordAsync(string email)
    {
        AppUser? appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (appUser == null) throw new Exception();
        string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
        return token;
    }

    public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        AppUser? appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordDto.Email);
        if (appUser == null) throw new Exception();
        var result = await _userManager.ResetPasswordAsync(appUser, resetPasswordDto.Token, resetPasswordDto.Password);
        if (!result.Succeeded) throw new Exception();
        await _userManager.UpdateSecurityStampAsync(appUser);
    }
}
