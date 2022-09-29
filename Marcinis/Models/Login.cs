using System.ComponentModel.DataAnnotations;

namespace Marcinis.Models
{
    public class Login
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter an email.")]
        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
