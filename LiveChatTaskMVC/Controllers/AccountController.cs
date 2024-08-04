using DTOs.UserDTOs;
using LiveChatTask.Application.Services.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model;

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

        public IActionResult GetUsers()
        {
            var users = _userManager.Users
               .Where(u=>u.UserName!= User.Identity.Name)
               .Select(u => new
               {
                   Id = u.Id,
                   Name = u.UserName
               }).ToList();
            return Json(users); // Return the list of users as JSON
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roles = _roleManager.Roles
               .Select(r => new
               {
                   Id = r.Id,
                   Name = r.Name
               }).ToList();
            ViewBag.Roles = roles;
            return View(); // P
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return View(registerDTO); 

            var result = await _userService.Registration(registerDTO);
            if (result.IsSuccess)
            {
                return RedirectToAction("Login", new { Email = registerDTO.Email, Password = registerDTO.password });
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(registerDTO); 
        }

        
      

        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Return the Login view
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO userDto)
        {
            if (ModelState.IsValid)
            {

                var result = await _userService.LoginAsync(userDto);
                if (result.IsSuccess == true)
                {
                    return RedirectToAction("Index", "LiveChat");

                }
                ModelState.AddModelError("", result.Message); // Add custom error message for general validation errors
                return View(userDto);
            }
            else
            {
                return View(userDto);
            }

            return RedirectToAction("ChatPage"); // Redirect to the ChatPage
            }

        

        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUser();
            return RedirectToAction("Login"); // Redirect to the Login page
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
