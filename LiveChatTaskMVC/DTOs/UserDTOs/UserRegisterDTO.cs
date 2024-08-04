using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DTOs.UserDTOs
{
    public class UserRegisterDTO
    {

        [Required(ErrorMessage = "Username is required")]
        [Remote("CheckUserName", "Account", ErrorMessage = "UserName Already Exist")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Email Already Exist")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$", ErrorMessage = "Password must be 8 characters, including uppercase, lowercase, digit, special.")]
        public string password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Passwords do not match")]
        public string PasswordConfirmed { get; set; }
        [Required(ErrorMessage = "Please Select Role")]
        public required string Role { get; set; }


    }
}
