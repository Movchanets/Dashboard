using Dashboard.Data.Data.Context;
using Dashboard.Data.Data.Interfaces;
using Dashboard.Data.Data.Models;
using Dashboard.Data.Data.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Data.Data.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> LoginUserAsync(LoginUserVM model)
        {
            var result = await _userManager.FindByEmailAsync(model.Email);
            return result;
        }

        public async Task<IdentityResult> RegisterUserAsync(AppUser model, string password)
        {
            var result = await _userManager.CreateAsync(model, password);
            return result;
        }

        public async Task<bool> ValidatePasswordAsync(LoginUserVM model, string password)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _userManager.CheckPasswordAsync(user, password);
            return result;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser)
        {
            var result = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            return result;
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            return result;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(AppUser model, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(model, token);
            return result;
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser model)
        {
            var result = await _userManager.GeneratePasswordResetTokenAsync(model);
            return result;
        }

        public async Task<IdentityResult> ResetPasswordAsync(AppUser model, string token, string password)
        {
            var result = await _userManager.ResetPasswordAsync(model, token, password);
            return result;
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            using (var _context = new AppDbContext())
            {
                await _context.RefreshToken.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RefreshToken> CheckRefreshTokenAsync(string refreshToken)
        {
            using (var _context = new AppDbContext())
            {
                var result = await _context.RefreshToken.FirstOrDefaultAsync(t => t.Token == refreshToken);
                return result;
            }
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            using (var _context = new AppDbContext())
            {
                _context.RefreshToken.Update(refreshToken);
                await _context.SaveChangesAsync();
                
            }
        }   

        public async Task<IList<string>> GetRolesAsync(AppUser model)
        {
      
      var result = await _userManager.GetRolesAsync(model);
            return result;
        }

		

		async Task<List<AppUser>> IUserRepository.GetAllUsersAsync()
		{
			List<AppUser>? users = await _userManager.Users.AsNoTracking().ToListAsync();
			
            return users;
        }
	}
}
