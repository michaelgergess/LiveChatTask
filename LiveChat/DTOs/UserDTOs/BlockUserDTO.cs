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
    public class BlockUserDTO
    {
        [Required(ErrorMessage = "Field can't be empty")]
        public string Name { get; set; }
        [Required]
        public bool IsBlocked { get; set; }
      
    }
}
