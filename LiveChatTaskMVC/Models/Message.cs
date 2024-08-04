using LiveChatTaskMVC.Models;
using LiveChatTaskMVC.Models.Common;
using Microsoft.Build.Framework;
using Model.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Message : MessageInfo
    {
        private string _content;

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
        public int? MaxContentLength { get; set; } = 20;


        public Message(int? maxContentLength = null)
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
