using Context;
using LiveChatTask.Application.Contract;
using Microsoft.EntityFrameworkCore;
using Model;

namespace LiveChatTaskMVC.Infrastructure
{
    public class UserRepository :IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public  IEnumerable<AppUser> GetActiveUsers()
        {
            return  _context.Users.Where(u => u.IsActive).ToList();
        }

        public  AppUser GetUserByName(string userName)
        {
            return  _context.Users.Single(u => u.UserName == userName);
        }
    }
}
