using Dashboard.Data.Data.Interfaces;
using Dashboard.Data.Data.Models;
using Dashboard.Data.Data.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Dashboard.Data.Data.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser model)
        {

            var result = await _userManager.GenerateEmailConfirmationTokenAsync(model);
            return result;
        }

        public async Task<IdentityResult> RegisterUserAsync(AppUser model, string password)
        {
            var result = await _userManager.CreateAsync(model, password);
            return result;
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string id, string token)
        {
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);
            var result = await _userManager.ConfirmEmailAsync(await GetUserByIdAsync(id), normalToken);
            return result;
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            var result = await _userManager.FindByIdAsync(id);

            return result;
        }
        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);

            return result;
        }
        public async Task<AppUser> LoginUserAsync(LoginUserVM model)
        {
            var result = await _userManager.FindByEmailAsync(model.Email);



            return result;

        }

        public async Task<bool> ValidatePasswordAsync(LoginUserVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);


            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            return result;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser model)
        {

            var token = await _userManager.GeneratePasswordResetTokenAsync(model);
            return token;
        }
        public async Task<IdentityResult> ResetPasswordAsync(AppUser model, string token, string password)
        {
            var result = await _userManager.ResetPasswordAsync(model, token, password);
            return result;
        }

        public async Task<IdentityResult> DeleteUserAsync(AppUser model)
        {
            var result = await _userManager.DeleteAsync(model);
            return result;
        }

        public List<UserVM> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            List<UserVM> result = new List<UserVM>();
            foreach (var user in users)
            {
                result.Add(new UserVM
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    EmailConfirmed = user.EmailConfirmed,
                    Name = user.Name,
                    Surname = user.Surname
                });
            };
            return result;
        }
    }
}
