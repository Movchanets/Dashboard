using Dashboard.Data.Data.Context;
using Dashboard.Data.Data.Interfaces;
using Dashboard.Data.Data.Models;
using Dashboard.Data.Data.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Dashboard.Data.Data.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        
        public UserRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

		}

        public async Task<SignInResult> LoginUserAsync(AppUser user, bool RememberMe , string password)
        {
            var result = await _signInManager.PasswordSignInAsync(user, password, RememberMe, true);
            return result;
        }
		public async  Task<bool> CanUserSignIn(AppUser model) 
        {
            return await _signInManager.CanSignInAsync(model);
        }

		public async Task<IdentityResult> RegisterUserAsync(AppUser model, RegisterUserVM vmodel)
        {
            var result = await _userManager.CreateAsync(model, vmodel.Password);
            var user = await GetUserByEmailAsync(model.Email);
			if (result.Succeeded && user!=null)
			{
                 _userManager.AddToRoleAsync(user, vmodel.Role).Wait();
			 }
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
            using var _context = new AppDbContext();
            var result = await _context.RefreshToken.FirstOrDefaultAsync(t => t.Token == refreshToken);
            return result;
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            using var _context = new AppDbContext();
            _context.RefreshToken.Update(refreshToken);
            await _context.SaveChangesAsync();
        }   

      

		

		public async Task<List<AppUser>> GetUsersAsync(int start, int end)
		{

            List<AppUser>? users =
                _userManager.Users.Skip(start).Take(end - start).ToList();
                //_userManager.Users.Take(new Range(new Index(start) , new Index(end))).ToList();
			
            return users;
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            using (var _context = new AppDbContext())
            {
				return  await _context.Roles.Select(i => i.Name).ToListAsync();
			}

			
            
        }

        public async Task<IList<string>> GetRolesAsync(AppUser model)
        {
            var result = await _userManager.GetRolesAsync(model);
            
                return result;
            
        }

        public async Task<IdentityResult> ChangeUserInfo(UserInfoVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) { return IdentityResult.Failed(); }
            user.Name = model.Name ;
            user.Surname = model.Surname;
            user.UserName = model.UserName;
            user.PhoneNumber = model.Phone;
             var result =  await  _userManager.UpdateAsync(user);
            return result;
        }
		public async Task<IdentityResult> ChangeUserPassword(ChangePasswordVM model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null) { return IdentityResult.Failed(); }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    
		
			return result;
		}

        public async Task<IdentityResult> BlockUserAsync(AppUser user)
        {
			var result =  await _userManager.SetLockoutEnabledAsync(user, true);

            return result;
		}
		public async Task<IdentityResult> UnblockUserAsync(AppUser user)
		{
			var result = await _userManager.SetLockoutEnabledAsync(user, false);

			return result;
		}

        public Task<IdentityResult> DeleteUserAsync(AppUser user)
        {
            var result = _userManager.DeleteAsync(user);
            return result;
        }

        public async Task DeleteRefreshTokens(string? userid)
        {
            using( AppDbContext context = new AppDbContext()) 
            {
                List<RefreshToken> tokens = await context.RefreshToken.Where(i => i.UserId == userid).ToListAsync();
                foreach (RefreshToken token in tokens)
                {
                    context.RefreshToken.Remove(token);
                }
              await context.SaveChangesAsync();
               
            }
            
        }

        public async Task<bool> IsUserLockedAsync(AppUser user)
        {
            return await _userManager.IsLockedOutAsync(user);
        }
    }
}
