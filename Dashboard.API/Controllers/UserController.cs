using Compass.Data.Data.ViewModels;
using Compass.Data.Validation;
using Dashboard.Data.Data.Models.ViewModels;
using Dashboard.Data.Validation;
using Dashboard.Services;
using Microsoft.AspNetCore.Http;
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
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return Conflict(result);
                }
            }
            else
            {
                return BadRequest(validationResult.Errors);
            }
        }
        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userid, string token)
        {
            if (string.IsNullOrWhiteSpace(userid) || string.IsNullOrWhiteSpace(token)) { return NotFound(); }
            var result = await _userService.ConfirmEmailAsync(userid, token);
            if (result.IsSuccess)

            {
                return RedirectToPage("/EmailConfirm");
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserVM model)
        {
            var validator = new LoginUserValidatation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return Conflict(result);
                }
            }
            else
            {
                return BadRequest(validationResult.Errors);
            }
        }


        [HttpPost("ForgotPassword")]
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
                    return RedirectToPage("/PasswordChanged");
                }
                return BadRequest(result);
            }
            else
            {
                return BadRequest(validationResult.Errors);
            }
        }
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUserAsync(string email) 
        {
            var result = await _userService.DeleteUserAsync(email);
            if (result.IsSuccess) 
            {
                return Ok(result);
            }
            else { return BadRequest(result); }
        }
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers() 
        {
            var result =  _userService.GetAllUsers();
            return Ok(result);
        }

    }
}
   
