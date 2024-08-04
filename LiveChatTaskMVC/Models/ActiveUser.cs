using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ActiveUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; } // Navigation property
        public DateTime LastSeen { get; set; }
    }

}
