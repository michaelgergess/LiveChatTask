using AutoMapper;
using DTO_s.ViewResult;
using DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using Model;

namespace LiveChatTask.Application.Services.User
{
    public class UserService : IuserService
    {

        private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _SignInManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;



        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task CheckOrCreateRole(string role)
        {
            bool ExistsRole = await _roleManager.RoleExistsAsync(role.ToLower());

            if (!ExistsRole)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

        }
        public async Task<ResultView<UserRegisterDTO>> Registration(UserRegisterDTO account)
        {
            // Check if the user already exists by email or username
            var existUserEmail = await _UserManager.FindByEmailAsync(account.Email);
            var existUserName = await _UserManager.FindByNameAsync(account.UserName);

            if (existUserName != null || existUserEmail != null)
            {
                return new ResultView<UserRegisterDTO>
                {
                    Entity = account,
                    IsSuccess = false,
                    Message = "User already exists"
                };
            }

            // Map UserRegisterDTO to AppUser entity
            var userModel = _mapper.Map<AppUser>(account);

            // Create the user
            var result = await _UserManager.CreateAsync(userModel, account.password);

            if (!result.Succeeded)
            {
                return new ResultView<UserRegisterDTO>
                {
                    Entity = account,
                    IsSuccess = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            // Check if the role exists
            var roleExists = await _roleManager.RoleExistsAsync(account.Role);
            if (!roleExists)
            {
                // Optionally, create the role if it doesn't exist
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(account.Role));
                if (!roleResult.Succeeded)
                {
                    // Handle role creation failure
                    return new ResultView<UserRegisterDTO>
                    {
                        Entity = account,
                        IsSuccess = false,
                        Message = "Role creation failed"
                    };
                }
            }

            return new ResultView<UserRegisterDTO>
            {
                Entity = account,
                IsSuccess = true,
                Message = "Successfully created account"
            };
        }

        public async Task<ResultView<GetAllUserDTO>> LoginAsync(UserLoginDTO userDto)
        {
            var oldUser = await _UserManager.FindByEmailAsync(userDto.Email);

            if (oldUser == null)
            {
                return new ResultView<GetAllUserDTO> { Entity = null, Message = "Email not found", IsSuccess = false };
            }


            var result = await _SignInManager.CheckPasswordSignInAsync(oldUser, userDto.password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                GetAllUserDTO userObj = new GetAllUserDTO()
                {
                    Email = userDto.Email,
                    UserName = oldUser.UserName
                };
                await _SignInManager.SignInAsync(oldUser, false);
                return new ResultView<GetAllUserDTO> { Entity = userObj, Message = "Login Successfully", IsSuccess = true };
            }

            return new ResultView<GetAllUserDTO> { Entity = null, Message = "Invalid password", IsSuccess = false };
        }
        public async Task<bool> LogoutUser()
        {
            await _SignInManager.SignOutAsync();
            return true;
        }


        public async Task<ResultView<List<GetAllUserDTO>>> GetAllUsers()
        {

            var users = _UserManager.Users.ToList();

            var userRole = _roleManager.Roles.SingleOrDefault(r => r.Name == "user");

            var usersInUserRole = users.Where(u => _UserManager.IsInRoleAsync(u, userRole.Name).Result);



            if (usersInUserRole == null)
            {
                return new ResultView<List<GetAllUserDTO>>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "No users found."
                };
            }
            var userlist = usersInUserRole.ToList();
            var userDTOs = _mapper.Map<List<GetAllUserDTO>>(userlist);

            return new ResultView<List<GetAllUserDTO>>
            {
                Entity = userDTOs,
                IsSuccess = true,
                Message = "Successfully retrieved all users."
            };
        }

        public async Task<ResultView<List<GetAllUserDTO>>> GetAllUsersPaging(int Count, int pagenumber)
        {
            var AlldAta = _UserManager.Users;
            if (AlldAta == null)
            {
                return new ResultView<List<GetAllUserDTO>>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "No users found."
                };
            }
            var userlist = AlldAta.Skip(Count * (pagenumber - 1)).Take(Count).ToList();
            var userDTOs = _mapper.Map<List<GetAllUserDTO>>(userlist);

            return new ResultView<List<GetAllUserDTO>>
            {
                Entity = userDTOs,
                IsSuccess = true,
                Message = "Successfully retrieved all users."
            };


        }

        public Task<ResultView<UserRegisterDTO>> Registration(UserRegisterDTO account, string? RoleName)
        {
            throw new NotImplementedException();
        }
    }
}

