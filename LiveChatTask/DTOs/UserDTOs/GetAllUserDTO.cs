using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.UserDTOs
{
    [NotMapped]
    public class GetAllUserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
