using IdentityService.DataAccess.Abstract.Models;
using IdentityService.Models;
using IdentityService.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Business.Abstract
{
    public interface IAccountManager
    {
        string HandleForgottenPasswordRequest(ForgotPasswordInput model);
        void ValidateEmailToken(string token);
        (UserMessageModel, ApplicationUser) ValidateResetPasswordRequest(ResetPasswordInput model);
        void ValidatePassword(string password, ApplicationUser userId);
        Task ResetPassword(string newPassword, string emailToken, ApplicationUser user);
        Task<bool> ChangeExpiredPasswordAsync(string newPassword, string resetPasswordToken, ApplicationUser user);
        Task ChangePasswordAsync(UserChangePasswordInput changePasswordInput);
        Task<string> GenerateExpiredPasswordResetTokenAsync(ApplicationUser user);
        bool IsUserPasswordExpired(ApplicationUser user);
    }
}
