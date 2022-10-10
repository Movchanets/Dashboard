using AutoMapper;
using Dashboard.Data.Data.Interfaces;
using Dashboard.Data.Data.Models;
using Dashboard.Data.Data.Models.ViewModels;
using Dashboard.Data.Data.ViewModels;
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

            var result = await  _userRepository.RegisterUserAsync(newUser, model.Password);
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
            var user = await _userRepository.LoginUserAsync(model);

            if (user == null)
            {
                return new ServiceResponse
                {
                    Message = "Login incorrect.",
                    IsSuccess = false
                };
            }

            var result = await _userRepository.ValidatePasswordAsync(model, model.Password);
            if (!result)
            {
                return new ServiceResponse
                {
                    Message = "Password incorrect.",
                    IsSuccess = false
                };
            }

            var tokens = await _jwtService.GenerateJwtTokenAsync(user);

            return new ServiceResponse
            {
                AccessToken = tokens.token,
                RefreshToken = tokens.refreshToken.Token,
                Message = "Logeed in successfully.",
                IsSuccess = true, 
            };
        }

		public async Task<ServiceResponse> GetAllUsersAsync()
		{
            var users = await _userRepository.GetAllUsersAsync();
            var usersVM = new List<AllUserVM>();



            foreach (var user in users)
            {
                var Alluser = new AllUserVM()
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed
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
                    Message = "User loaded"
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
            return result;
        }
    }
}
