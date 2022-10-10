using Dashboard.Data.Data.Models;
using Dashboard.Data.Data.Models.ViewModels;
using Dashboard.Data.Data.ViewModels;
using Dashboard.Data.Validation;
using Dashboard.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserVM model)
        {
            var validator = new RegisterUserValidation();
            var validationResult = validator.Validate(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);

                return Ok(result);
            }
            else
            {
                return BadRequest(validationResult.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserVM model)
        {
            var validator = new LoginUserValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);
                return Ok(result);
            }
            else
            {
                return BadRequest(validationResult.Errors);
            }
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(userId, token);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return NotFound();
            }

            var result = await _userService.ForgotPasswordAsync(email);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromForm] ResetPasswordVM model)
        {
            var validator = new ResetPasswordValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.ResetPasswordAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            else
            {

                return BadRequest(validationResult.Errors);
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshTokenAsync(TokenRequestVM model)
        {
            var validator = new TokenRequestValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.RefreshTokenAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(validationResult.Errors);
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsersAsync() 
        {
            var result = await _userService.GetAllUsersAsync();
			if (result.IsSuccess) 
            {
                return Ok(result);

            }
            return BadRequest(result);
        }
    }
}
