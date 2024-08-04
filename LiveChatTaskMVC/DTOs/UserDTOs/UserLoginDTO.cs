using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTOs.UserDTOs
{
    [NotMapped]
    public class UserLoginDTO
    {
       
        [Required(ErrorMessage = "Email can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password can't be empty")]
        public required string password { get; set; }

    }
}
