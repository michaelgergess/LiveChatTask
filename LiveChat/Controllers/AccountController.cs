using DTOs.UserDTOs;
using LiveChatTask.Application.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LiveChatApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IuserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            IuserService userService,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles =  _roleManager.Roles
                .Select(r => new
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList();
            return View(roles); // Pass roles to the view
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Return the Register view
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return View(registerDTO); // Return the view with the validation errors

            var result = await _userService.Registration(registerDTO);
            if (result.IsSuccess)
            {
                return RedirectToAction("Login", new { Email = registerDTO.Email, Password = registerDTO.password });
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(registerDTO); // Return the view with the error message
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChatPage()
        {
            return View(); // Return the ChatPage view
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            // var res = await _worldChatService.GetAll();
            return View(); // Return the view with messages
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Return the Login view
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO userDto)
        {
            if (!ModelState.IsValid)
                return View(userDto); // Return the view with validation errors

            var usr = await _userService.LoginAsync(userDto);

            if (usr.IsSuccess)
            {
                var Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, usr.Entity.Email),
                    new Claim(ClaimTypes.Name, usr.Entity.UserName),
                };
                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"]));

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["jwt:Issuer"],
                    audience: _configuration["jwt:Audiences"],
                    expires: DateTime.Now.AddDays(50),
                    claims: Claims,
                    signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature)
                );

                var StringToken = new JwtSecurityTokenHandler().WriteToken(token);

                // Set the JWT token in a cookie or session for MVC applications
                Response.Cookies.Append("JWTToken", StringToken, new CookieOptions { HttpOnly = true, Secure = true });

                return RedirectToAction("ChatPage"); // Redirect to the ChatPage
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(userDto); // Return the view with the error message
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUser();
            Response.Cookies.Delete("JWTToken"); // Remove the JWT token
            return RedirectToAction("Login"); // Redirect to the Login page
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var res = await _userService.GetAllUsers();
            return View(res.Entity); // Pass the list of users to the view
        }

        [HttpGet]
        public async Task<IActionResult> CheckUserName(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            return Json(user == null);
        }

        [HttpGet]
        public async Task<IActionResult> CheckEmail(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            return Json(user == null);
        }
    }
}
