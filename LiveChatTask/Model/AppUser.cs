
using Microsoft.AspNetCore.Identity;

namespace Model
{

    public class AppUser : IdentityUser
    {
        public DateTime LastActive { get; set; }
        public bool IsActive { get; set; }

        public ICollection<ChatSession> ChatSessions { get; set; }
    }
}
