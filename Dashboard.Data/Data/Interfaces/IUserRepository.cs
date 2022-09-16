using Dashboard.Data.Data.Models;
using Dashboard.Data.Data.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Data.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterUserAsync(AppUser model, string password);
        Task<AppUser> LoginUserAsync(LoginUserVM model);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser model);
        Task<IdentityResult> ConfirmEmailAsync(string id, string token);
        Task<AppUser> GetUserByIdAsync(string id);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<bool> ValidatePasswordAsync(LoginUserVM model);
        Task<string> GeneratePasswordResetTokenAsync(AppUser model);
        Task<IdentityResult> ResetPasswordAsync(AppUser model, string token, string password);
        Task<IdentityResult> DeleteUserAsync(AppUser model);
        List<UserVM> GetAllUsers();
    }
}
