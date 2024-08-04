using DTO_s.ViewResult;
using DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using Model;

namespace LiveChatTask.Application.Services.User
{
    public interface IuserService
    {

        Task<ResultView<GetAllUserDTO>> LoginAsync(UserLoginDTO userDto);
        Task<ResultView<UserRegisterDTO>> Registration(UserRegisterDTO account);
        Task<bool> LogoutUser();
        Task<ResultView<List<GetAllUserDTO>>> GetAllUsers();
        Task<ResultView<List<GetAllUserDTO>>> GetAllUsersPaging(int items, int pagenumber);







    }
}