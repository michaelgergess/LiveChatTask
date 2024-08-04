using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ChatSession
    {
        public int Id { get; set; }
        public string AdminId { get; set; }
        public AppUser Admin { get; set; } // Navigation property for Admin

        public string UserId { get; set; }
        public AppUser User { get; set; } // Navigation property for User

        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public List<ChatMessage> Messages { get; set; }
    }

}
