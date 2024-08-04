using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ChatDTOs
{
    public class ChatDTO
    {
        public string UserName { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
