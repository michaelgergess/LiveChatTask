
using LiveChatTaskMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace Model
{

    public class AppUser : IdentityUser
    {
        public DateTime LastActive { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Message> MessagesAsReciver { get; set; } = new List<Message>();
        public ICollection<Message> MessagesAsSender { get; set; } = new List<Message>();
        public ICollection<FileAttachment> FileAttachmentsAsReciver { get; set; } = new List<FileAttachment>();
        public ICollection<FileAttachment> FileAttachmentsAsSender { get; set; } = new List<FileAttachment>();
    }
}
