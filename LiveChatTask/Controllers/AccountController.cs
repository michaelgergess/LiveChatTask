using DTOs.UserDTOs;
using LiveChatTask.Application.Contract;
using LiveChatTask.Application.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LiveChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IuserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IConfiguration _configuration;
      //  private readonly IChatRepository _worldChatService;
      // private readonly IHubContext<ChatHub> _hubContext;

        public AccountController(
            IuserService userService,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,

            IConfiguration configuration)
       //  IChatRepository worldChatService)
       //     IHubContext<ChatHub> hubContext)
        {
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;

            _configuration = configuration;
          //  _worldChatService = worldChatService;
         //   _hubContext = hubContext;
        }
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles
                                                    .Select(r=> new {
                                                        Id = r.Id,
                                                        Name = r.Name
                                                                          }).ToListAsync();
            return Ok(roles);
        }
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return Ok("Register page content");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Registration(registerDTO);
            if (result.IsSuccess)
            {
                return Ok(new { RedirectUrl = Url.Action("Login", new { Email = registerDTO.Email, Password = registerDTO.password }) });
            }

            return BadRequest(result.Message);
        }
        [Authorize]
        [HttpGet("ChatPage")]
        public IActionResult ChatPage()
        {
            return Ok("Chat page content");
        }

        [HttpGet("GetAllMessages")]
        public async Task<IActionResult> GetAllMessages()
        {
         //   var res = await _worldChatService.GetAll();
            return Ok();
        }
      

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return Ok("Login page content");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
                var usr = await _userService.LoginAsync(userDto);

                if (usr.IsSuccess == true)
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
                    return Ok(new { StringToken, Expire = token.ValidTo });
                }
                return Unauthorized(ModelState);

            
            return Unauthorized(ModelState);
        }


        

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUser();
            return Ok("Logged out successfully");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var res = await _userService.GetAllUsers();
            return Ok(res.Entity);
        }

       

        [HttpGet("CheckUserName")]
        public async Task<IActionResult> CheckUserName([FromQuery] string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            return Ok(user == null);
        }

        [HttpGet("CheckEmail")]
        public async Task<IActionResult> CheckEmail([FromQuery] string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            return Ok(user == null);
        }
    }
}
