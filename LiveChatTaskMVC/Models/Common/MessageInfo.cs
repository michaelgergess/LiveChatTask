using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model;
using Model.Common;

namespace LiveChatTaskMVC.Models.Common
{
    public class MessageInfo :BaseEntity
    {
       
        public bool IsSeen { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? SeenAt { get; set; }

        [Required]
        [ForeignKey("AppUser")]

        public string SenderId { get; set; }
        public virtual AppUser Sender { get; set; }

        // Foreign Key for Receiver
        [Required]
        [ForeignKey("AppUser")]
        public string ReciverId { get; set; }
        public AppUser Reciver { get; set; }
    }
}
