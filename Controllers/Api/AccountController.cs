using Mediar.Models;
using Mediar.Models.DTOs;
using Mediar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mediar.Controllers.Api
{
    [ApiController]
    [Route("api/account")]
    public class AccountApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AccountApiController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _userService.LoginUserAsync(
                request.Email,
                request.Password
            );

            if (token == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new
            {
                access_token = token
            });
        }

        [HttpPost("register")]
        [AllowAnonymous]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request)
		{
			var user = new User
			{
				Name = request.Name,
				Email = request.Email,
				Password = request.Password,
				Permission = request.Permission
			};

			try
			{
				await _userService.RegisterUserAsync(user);
				return Ok(new { message = "User registered successfully" });
			}
			catch (Exception ex)
			{
				// Return a 400 Bad Request with the exception message
				return BadRequest(new { message = ex.Message });
			}
		}


        [HttpGet("users")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }


        [HttpDelete("users/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "view,edit,delete,admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateUser(
            int id,
            [FromForm] UpdateUserRequest request)
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null)
                return NotFound();

            if (_authService.ValidatePassword(request.Password, user.Password))
            {
                user.Name = request.Name ?? user.Name;

                if (request.NewPassword != null)
                {
                    user.Password = _authService.HashPassword(request.NewPassword);
                }
                await _userService.UpdateUser(user);
                return NoContent();
            } else
            {
                return Unauthorized();
            }
            
        }

    }
}
