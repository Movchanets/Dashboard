using AutoMapper;
using Compass.Data.Data.ViewModels;
using Dashboard.Data.Data.Interfaces;
using Dashboard.Data.Data.Models;
using Dashboard.Data.Data.Models.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Dashboard.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly EmailService _EmailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;
        public UserService(IUserRepository userRepository, EmailService EmailService,
            IConfiguration configuration, IMapper mapper, JwtService jwtService)
        {
            _userRepository = userRepository;
            _EmailService = EmailService;
            _configuration = configuration;
            _mapper = mapper;
            _jwtService = jwtService;
        }
        public async Task<ServiceResponce> LoginUserAsync(LoginUserVM model)
        {
            var user = await _userRepository.LoginUserAsync(model);
            if (user == null)
            {
                return new ServiceResponce()
                {
                    Message = "Login incorrect",
                    IsSuccess = false
                };
            }
            var result = await _userRepository.ValidatePasswordAsync(model);
            if (result == false) { return new ServiceResponce() { Message = "Password incorrect" }; }
            var claims = new[]
            {
                    new Claim("Email",model.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.Id)
            };
            (string token, DateTime ValidTo) res = _jwtService.GenerateJwtToken(claims);
            
            return new ServiceResponce()
            {
                Message = res.token,
                IsSuccess = true,
                ExpireDate = res.ValidTo

            };
        }

        public async Task<ServiceResponce> RegisterUserAsync(RegisterUserVM model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return new ServiceResponce()
                {
                    Message = "Passwords dont match",
                    IsSuccess = false
                };
            }
            var newUser = _mapper.Map<RegisterUserVM,AppUser>(model);
           
            var result = await _userRepository.RegisterUserAsync(newUser, model.Password);
            if (result.Succeeded)
            {
                var token = await _userRepository.GenerateEmailConfirmationTokenAsync(newUser);
                var encodedEmailToken = Encoding.UTF8.GetBytes(token);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
                string url = $"{_configuration["HostSettings:URL"]}/api/User/confirmemail?userid={newUser.Id}&token={validEmailToken}";
                string emailBody = $"<h1>Confirm your Email</h1> <a href = '{url}'>Confirm email now!</a>";
                await _EmailService.SendEmailAsync(newUser.Email, "Email Confirmation", emailBody);
                return new ServiceResponce()
                {
                    Message = $"User successfully created {model.Email}",
                    IsSuccess = true
                };
            }
            else
            {
                return new ServiceResponce()
                {
                    Message = $"Error. User failed to  create {model.Email}",
                    IsSuccess = false,
                    Errors = result.Errors.Select(i => i.Description)
                };
            }
        }
        public async Task<ServiceResponce> ConfirmEmailAsync(string userid, string token)
        {
            var result = await _userRepository.ConfirmEmailAsync(userid, token);
            if (result.Succeeded)
            {
                return new ServiceResponce()
                {
                    Message = "Email Confirmed",
                    IsSuccess = true
                };
            }
            else
            {
                return new ServiceResponce()
                {
                    Message = "Some error occured",
                    IsSuccess = false,
                    Errors = result.Errors.Select(i => i.Description)
                };
            }

        }
        public async Task<ServiceResponce> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return new ServiceResponce
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
            await _EmailService.SendEmailAsync(email, "Foget password", emailBody);

            return new ServiceResponce
            {
                IsSuccess = true,
                Message = $"Reset password for {_configuration["HostSettings:URL"]} has been sent to the email successfully!"
            };
        }
        public ServiceResponce GetAllUsers()
        {
            var result = _userRepository.GetAllUsers();
            if (result == null)
            {
                return new ServiceResponce { Message = "There are no  users there", IsSuccess = false };
            
            }
            string json = JsonSerializer.Serialize(result);
            return new ServiceResponce { Message = json, IsSuccess = true };
        }
        public async Task<ServiceResponce> DeleteUserAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return new ServiceResponce { Message = "User doesnt exist", IsSuccess = false };
            }
            var result = await _userRepository.DeleteUserAsync(user);
            if (result.Succeeded)
            {
                return new ServiceResponce { Message = "User deleted successfully", IsSuccess = true };
            }
            else
            {
                return new ServiceResponce { Message = "User is not deleted ", IsSuccess = false };
            }
        }
        public async Task<ServiceResponce> ResetPasswordAsync(ResetPasswordVM model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
            {
                return new ServiceResponce
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                return new ServiceResponce
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
                return new ServiceResponce
                {
                    Message = "Password has been reset successfully!",
                    IsSuccess = true,
                };
            }
            return new ServiceResponce
            {
                Message = "Something went wrong",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

    }
}
