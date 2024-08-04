using LiveChatTaskMVC.Models;
using Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ChatDTOs
{
    public class ChatItem
    {
        public bool IsMessage { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime SentAt { get; set; }
    }

}
