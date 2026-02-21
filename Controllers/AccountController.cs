using System.Diagnostics;
using Mediar.Models;
using Mediar.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Mediar.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Login()
        {
            var adminExists = await _userService.AdminExists();
            return View(adminExists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (!ModelState.IsValid)
                return View();

            var token = await _userService.LoginUserAsync(email, password);

            if (token == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View();
            }

            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });

            return RedirectToAction("AlbumList", "AlbumList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Register()
        {
            var adminExists = await _userService.AdminExists();

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (userRole != "admin" && adminExists)
                return RedirectToAction("Login");
            else
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            
            if (ModelState.IsValid)
            {
                await _userService.RegisterUserAsync(user);

                var adminExists = await _userService.AdminExists();

                if (adminExists)
                    return RedirectToAction("Profile");
                else
                    return RedirectToAction("Login");
            }
            else
            {
                Debug.WriteLine("ModelState is invalid.");
            }
            
            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            IEnumerable<User> users = new List<User>();

            if (userRole == "admin")
                users = await _userService.GetAllUsers();
            
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return RedirectToAction("Profile");
        }

    }
}
