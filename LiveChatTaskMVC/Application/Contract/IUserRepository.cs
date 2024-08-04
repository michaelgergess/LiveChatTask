using DTO_s.ViewResult;
using DTOs.UserDTOs;
using Microsoft.EntityFrameworkCore;
using Model;

namespace LiveChatTask.Application.Contract
{
    public interface IUserRepository
    {
         IEnumerable<AppUser> GetActiveUsers();
       AppUser GetUserByName(string userName);
    }
}
