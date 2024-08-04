using LiveChatTaskMVC.Models.Common;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveChatTaskMVC.Models
{
    public class FileAttachment :MessageInfo
    {
        public FileAttachment()
        {
            
        }
        public string FileName { get; set; } // Original name of the uploaded file.
        public byte[] FileContent { get; set; } // Binary content of the file.
        public string ContentType { get; set; } // MIME type of the file (e.g., image/jpeg, application/pdf).

      

    }
}
