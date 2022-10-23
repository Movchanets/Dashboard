using Dashboard.Data.Data.Models;
using Dashboard.Data.Data.Models.ViewModels;
using Dashboard.Data.Data.ViewModels;
using Dashboard.Data.Validation;
using Dashboard.Services;
using FluentValidation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Administrators")]
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserVM model)
        {
            var validator = new LoginUserValidation();
            var validationResult = validator.Validate(model);
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
        
        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody]  string email)
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenRequestVM model)
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
            else
            {
                return BadRequest(validationResult.Errors);
            }

        }
		[Authorize]
		[HttpPost("ChangeInfoUser")]
		public async Task<IActionResult> ChangeUserInfo([FromBody] UserInfoVM model)
		{
            var result = await _userService.ChangeUserInfo(model);
            if (result.IsSuccess) { return Ok(result); }
            return BadRequest(result);
		}
		[Authorize]
		[HttpPost("ChangeUserPassword")]
		public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordVM model)
		{
			var result = await _userService.ChangeUserPassword(model);
			if (result.IsSuccess) { return Ok(result); }
			return BadRequest(result);
		}
		[Authorize(Roles = "Administrators")]
		[HttpPost("GetUsers")]
        public async Task<IActionResult> GetUsersAsync([FromBody] GetUsersVM model)
        {
            var result = await _userService.GetUsersAsync(model.pageNumber, model.pageSize);
            if (result.IsSuccess)
            {
                return Ok(result);

            }
            return BadRequest(result);
        }
        [Authorize(Roles = "Administrators")]
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var result = await _userService.GetRolesAsync();
            if (result.IsSuccess)
            {
                return Ok(result);

            }
            return BadRequest(result);
        }
		[Authorize(Roles = "Administrators")]
		[HttpGet("BlockUser")]
		public async Task<IActionResult> BlockUserAsync(string email)
		{
            var result = await _userService.BlockUserAsync(email);
			if (result.IsSuccess)
			{
				return Ok(result);

			}
			return BadRequest(result);
		}
		[Authorize(Roles = "Administrators")]
		[HttpGet("UnblockUser")]
		public async Task<IActionResult> UnblockUserAsync(string email)
		{
			var result = await _userService.UnblockUserAsync(email);
			if (result.IsSuccess)
			{
				return Ok(result);

			}
			return BadRequest(result);
		}
		[Authorize(Roles = "Administrators")]
		[HttpGet("DeleteUser")]
		public async Task<IActionResult> DeleteUserAsync(string email)
		{
			var result = await _userService.DeleteUserAsync(email);
			if (result.IsSuccess)
			{
				return Ok(result);

			}
			return BadRequest(result);
		}
		[Authorize]
		[HttpGet("LogOut")]
		public async Task<IActionResult> LogOutAsync(string email)
		{
			var result = await _userService.LogOutAsync(email);
			if (result.IsSuccess)
			{
				return Ok(result);

			}
			return BadRequest(result);
		}
	
	}
}
