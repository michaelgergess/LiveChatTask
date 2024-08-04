using Model.Common;
using Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class ChatMessage : BaseEntity
    {
        private string _content;
        public int ChatSessionId { get; set; }
        public ChatSession ChatSession { get; set; } // Navigation property

        public string Content
        {
            get => _content;
            set
            {
                if (MaxContentLength.HasValue && value.Length > MaxContentLength.Value)
                { return; }
                _content = value;
            }
        }
        public int? MaxContentLength { get; set; }

        public MessageType Type { get; set; } // Enum for text, image, document, voice
        public bool IsSeen { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? SeenAt { get; set; }


        public ChatMessage(int? maxContentLength = null)
        {
            MaxContentLength = maxContentLength;
        }

        // Method to update the maximum content length
        public bool TrySetContent(string content, out string message)
        {
            if (MaxContentLength.HasValue && content.Length > MaxContentLength.Value)
            {
                message = $"Content length cannot exceed {MaxContentLength.Value} characters.";
                return false;
            }
            Content = content;
            message = "Content set successfully.";
            return true;
        }
    }


}
