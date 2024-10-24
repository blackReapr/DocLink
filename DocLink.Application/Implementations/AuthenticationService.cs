﻿using DocLink.Application.Dtos.AuthenticationDtos;
using DocLink.Application.Exceptions;
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
    private readonly IEmailService _emailService;

    public AuthenticationService(UserManager<AppUser> userManager, ITokenService tokenService, IEmailService emailService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    public async Task RegisterAsync(SignUpDto signUpDto)
    {
        if (signUpDto.Password != signUpDto.PasswordConfirm) throw new CustomException(400, "Passwords do not match");
        AppUser user = new() { Name = signUpDto.Name, Email = signUpDto.Email, Surname = signUpDto.Surname, UserName = signUpDto.Email };
        IdentityResult result = await _userManager.CreateAsync(user, signUpDto.Password);
        if (!result.Succeeded) throw new CustomException(400, result.Errors.First().Code.ToString());
        if (signUpDto.Profile != null)
        {
            if (!signUpDto.Profile.IsImage()) throw new CustomException(400, "Invalid file format");
            if (signUpDto.Profile.DoesSizeExceed(100)) throw new CustomException(400, "File size exceeds the limit");
            string filename = await signUpDto.Profile.SaveFileAsync();
            user.Profile = filename;
        }
        await _userManager.AddToRoleAsync(user, "member");
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        //todo: front link
        string link = $"http://localhost:5000/auth/verify-email?token={token}&email={user.Email}";

        string body = "";
        using (StreamReader stream = new("wwwroot/templates/verifyEmail.html"))
        {
            body = stream.ReadToEnd();
        };
        body = body.Replace("{{link}}", link);
        body = body.Replace("{{username}}", $"{user.Name} {user.Surname}");

        _emailService.SendEmail(user.Email, "Verify Email", body);
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null) throw new CustomException(400, "User not found");

        bool doesPasswordMatch = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!doesPasswordMatch) throw new CustomException(400, "Incorrect credentials");

        if (!user.EmailConfirmed) throw new CustomException(400, "Confirm your email");

        string token = _tokenService.GenerateJWTToken(user);
        return token;
    }

    public async Task ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
    {
        AppUser? appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == confirmEmailDto.Email);
        if (appUser == null) throw new CustomException(400, $"Unable to load user {confirmEmailDto.Email}");
        await _userManager.ConfirmEmailAsync(appUser, confirmEmailDto.Token);
        await _userManager.UpdateSecurityStampAsync(appUser);
    }

    public async Task<string> ForgotPasswordAsync(string email)
    {
        AppUser? appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (appUser == null) throw new CustomException(404, "User does not exist");
        string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
        return token;
    }

    public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        AppUser? appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordDto.Email);
        if (appUser == null) throw new CustomException(404, "User does not exist");
        var result = await _userManager.ResetPasswordAsync(appUser, resetPasswordDto.Token, resetPasswordDto.Password);
        if (!result.Succeeded) throw new CustomException(404, "Expired Token");
        await _userManager.UpdateSecurityStampAsync(appUser);
    }
}
