using AutoMapper;
using Dashboard.Data.Data.Interfaces;
using Dashboard.Data.Data.Models;
using Dashboard.Data.Data.Models.ViewModels;
using Dashboard.Data.Data.ViewModels;
using Dashboard.Data.Validation;
using FluentValidation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Dashboard.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private IConfiguration _configuration;
        private EmailService _emailService;
        private JwtService _jwtService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IConfiguration configuration, EmailService emailService, IMapper mapper, JwtService jwtService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
            _mapper = mapper;
            _jwtService = jwtService;
        }
        public async Task<ServiceResponse> RegisterUserAsync(RegisterUserVM model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Register model is null.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return new ServiceResponse
                {
                    Message = "Confirm pssword do not match",
                    IsSuccess = false
                };
            }

            var newUser = _mapper.Map<RegisterUserVM, AppUser>(model);

            var result = await  _userRepository.RegisterUserAsync(newUser, model);
            if (result.Succeeded)
            {
                var token = await _userRepository.GenerateEmailConfirmationTokenAsync(newUser);

                var encodedEmailToken = Encoding.UTF8.GetBytes(token);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                string url = $"{_configuration["HostSettings:URL"]}/api/User/confirmemail?userid={newUser.Id}&token={validEmailToken}";

                string emailBody = $"<h1>Confirm your email</h1> <a href='{url}'>Confirm now</a>";
                await _emailService.SendEmailAsync(newUser.Email, "Email confirmation.", emailBody);

                var tokens = await _jwtService.GenerateJwtTokenAsync(newUser);

                return new ServiceResponse
                {
                    AccessToken = tokens.token,
                    RefreshToken = tokens.refreshToken.Token,
                    Message = "User successfully created.",
                    IsSuccess = true
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Message = "Error user not created.",
                    IsSuccess = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }
        }

        public async Task<ServiceResponse> LoginUserAsync(LoginUserVM model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                return new ServiceResponse
                {
                    Message = "Login incorrect.",
                    IsSuccess = false,
                    
                };
            }
            if(!(await _userRepository.CanUserSignIn(user))) 
            {
                return new ServiceResponse { Message = "You cannot login now, account is blocked" ,IsSuccess = false};
            }
            var result = await _userRepository.LoginUserAsync(user,model.RememberMe, model.Password);
            if (result.Succeeded) 
            {
            
            var tokens = await _jwtService.GenerateJwtTokenAsync(user);

            return new ServiceResponse
            {
                AccessToken = tokens.token,
                RefreshToken = tokens.refreshToken.Token,
                Message = "Logged in successfully.",
                IsSuccess = true,
				Payload =  await _userRepository.GetAllRolesAsync()
			};
			}
            else 
            {
                
                return _userRepository.IsUserLockedAsync(user).Result ?new ServiceResponse
                { Message = "User locked, try again in 5 minutes", IsSuccess = false }
                : new ServiceResponse { Message = "Wrong password", IsSuccess = false };
            }
		}

		public async Task<ServiceResponse> GetUsersAsync(int pageNumber, int pageSize)
		{
            int start = pageNumber * pageSize;
            int end = pageSize * pageNumber + pageSize;
            var users = await _userRepository.GetUsersAsync(start, end);
            var usersVM = new List<AllUserVM>();



            foreach (var user in users)
            {
                var Alluser = new AllUserVM()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    UserName = user.UserName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    EmailConfirm = user.EmailConfirmed,
                    isBlocked = user.LockoutEnabled,
                };
                var roles = await _userRepository.GetRolesAsync(user);
                Alluser.Role = roles.First(); 

				usersVM.Add(Alluser);
            }
          
			if (usersVM != null) 
            {
                return new ServiceResponse
                {
                    IsSuccess = true,
                    Payload = usersVM,
                    Message = "Users loaded"
                };
            }
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = "some error occured"
            };
		}

		public async Task<ServiceResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return new ServiceResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userRepository.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
                return new ServiceResponse
                {
                    Message = "Email confirmed successfully!",
                    IsSuccess = true,
                };

            return new ServiceResponse
            {
                IsSuccess = false,
                Message = "Email did not confirm",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<ServiceResponse> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if(user == null)
            {
                return new ServiceResponse
                {
                    Message = "No user associated with email",
                    IsSuccess = false
                };
            }

            var token = await _userRepository.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_configuration["HostSettings:URL"]}/ResetPassword?email={email}&token={validToken}";
            string emailBody = "<h1>Follow the instructions to reset your password</h1>" + $"<p>To reset your password <a href='{url}'>Click here</a></p>";
            await _emailService.SendEmailAsync(email, "Fogot password", emailBody);

            return new ServiceResponse
            {
                IsSuccess = true,
                Message = $"Reset password for {_configuration["HostSettings:URL"]} has been sent to the email successfully!"
            };
        }

        public async Task<ServiceResponse> ResetPasswordAsync(ResetPasswordVM model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if(user == null)
            {
                return new ServiceResponse
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                return new ServiceResponse
                {
                    IsSuccess = false,
                    Message = "Password doesn't match its confirmation",
                };
            }

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userRepository.ResetPasswordAsync(user, normalToken, model.NewPassword);
            if (result.Succeeded)
            {
                return new ServiceResponse
                {
                    Message = "Password has been reset successfully!",
                    IsSuccess = true,
                };
            }
            return new ServiceResponse
            {
                Message = "Something went wrong",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

		public async Task<ServiceResponse> RefreshTokenAsync(TokenRequestVM model)
		{
			var result = await _jwtService.VerifyTokenAsync(model);
			if (result == null)
			{
				return result;
			}
			else
			{
				return result;
			}
		}

		public async Task<ServiceResponse> GetRolesAsync()
        {
            var roles = await _userRepository.GetAllRolesAsync();
            if (roles != null)
            {
                return new ServiceResponse
                {
                    Message = "All roles sent",
                    Payload = roles,
                    IsSuccess = true
                };
            }
            return new ServiceResponse
            {
                Message = "Some error occured",
                IsSuccess = false
            };
        }

        public async Task<ServiceResponse> ChangeUserInfo(UserInfoVM model)
        {
            var validator = new UserInfoValidation();
            var validationResult =  await validator.ValidateAsync(model);
            if (validationResult.IsValid) { 
            var result = await _userRepository.ChangeUserInfo(model);
            if (result.Succeeded)
            { var user = await _userRepository.GetUserByEmailAsync(model.Email);
              var tokens = await _jwtService.GenerateJwtTokenAsync(user);
                return new ServiceResponse
                {
                    IsSuccess = true,
                    Message = "User Profile Changed",
					AccessToken = tokens.token,
					RefreshToken = tokens.refreshToken.Token,
				};
            }
            return new ServiceResponse
            {
                Message = "Some Entity error occured",
                IsSuccess = false
        };
			}
            return new ServiceResponse
            {
                Message = "Some validation error occured : ",
                Errors = validationResult.Errors.Select(i => i.ErrorCode + "  :  " + i.ErrorMessage),
                IsSuccess = false
            };
		}

        public async Task<ServiceResponse> ChangeUserPassword(ChangePasswordVM model)
        {
            var validator = new ChangePasswordValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userRepository.ChangeUserPassword(model);
                if (result.Succeeded) 
                {
					return new ServiceResponse
					{
						IsSuccess = true,
						Message = "User Password Changed"
					};
				}
                return new ServiceResponse
                {
                    Message = "Password dont match",
                    IsSuccess = false
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Message = "Some validation error occured : ",
                    Errors = validationResult.Errors.Select(i => i.ErrorCode + "  :  " + i.ErrorMessage),
                    IsSuccess = false
                };
            }
        }

        public async Task<ServiceResponse> BlockUserAsync(string email)
        {
            AppUser? user = await _userRepository.GetUserByEmailAsync(email);
            if(user == null) {
                return new ServiceResponse
                {
                    Message = "User not exists",
                    IsSuccess = false
                };
            }
            var result = await _userRepository.BlockUserAsync(user);
            if (result.Succeeded) 
            {
                return new ServiceResponse
                {
                    Message = "User Successfully blocked",
                    IsSuccess = true
                };
		}
            return new ServiceResponse
            {
                Message = "Some error occured",
                IsSuccess = false
            };
}
		public async Task<ServiceResponse> UnblockUserAsync(string email)
		{
			AppUser? user = await _userRepository.GetUserByEmailAsync(email);
			if (user == null)
			{
				return new ServiceResponse
				{
					Message = "User not exists",
					IsSuccess = false
				};
			}
			var result = await _userRepository.UnblockUserAsync(user);
			if (result.Succeeded)
			{
				return new ServiceResponse
				{
					Message = "User Successfully blocked",
					IsSuccess = true
				};
			}
			return new ServiceResponse
			{
				Message = "Some error occured",
				IsSuccess = false
			};
		}

        public async Task<ServiceResponse> DeleteUserAsync(string email)
        {
			AppUser? user = await _userRepository.GetUserByEmailAsync(email);
			if (user == null)
			{
				return new ServiceResponse
				{
					Message = "User not exists",
					IsSuccess = false
				};
			}
			var result = await _userRepository.DeleteUserAsync(user);
			if (result.Succeeded)
			{
				return new ServiceResponse
				{
					Message = "User Successfully deleted",
					IsSuccess = true
				};
			}
			return new ServiceResponse
			{
				Message = "Some error occured",
				IsSuccess = false
			};
		}

        public async Task<ServiceResponse> LogOutAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) 
            {
				return new ServiceResponse
				{
					Message = "User not exists",
					IsSuccess = false
				};
			}
			 await _userRepository.DeleteRefreshTokens(user.Id);
                return new ServiceResponse { Message = "LogOut Success", IsSuccess = true };
            
       
		}

      
		public async Task<ServiceResponse> GetNewTokenAsync(string email)
		{
			var user = await _userRepository.GetUserByEmailAsync(email);

			if (user == null)
			{
				return new ServiceResponse
				{
					Message = "Login incorrect.",
					IsSuccess = false,

				};
			}
			
			

				var tokens = await _jwtService.GenerateJwtTokenAsync(user);

				return new ServiceResponse
				{
					AccessToken = tokens.token,
					RefreshToken = tokens.refreshToken.Token,
					Message = "JwtUpdated successfully.",
					IsSuccess = true,
					Payload = await _userRepository.GetAllRolesAsync()
				};
			
		
		}
	}
}
