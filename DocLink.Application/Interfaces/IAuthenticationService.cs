﻿using DocLink.Application.Dtos.AuthenticationDtos;

namespace DocLink.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
        Task<string> ForgotPasswordAsync(string email);
        Task<string> LoginAsync(LoginDto loginDto);
        Task RegisterAsync(SignUpDto signUpDto);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}