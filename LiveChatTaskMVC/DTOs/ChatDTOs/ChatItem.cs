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
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }

        public string ContentType { get; set; }
        public DateTime SentAt { get; set; }
    }

}
