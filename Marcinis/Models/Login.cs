using Marcinis.Helpers;
using Marcinis.Validators;
using System.ComponentModel.DataAnnotations;

namespace Marcinis.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter an email.")]
        [RegisteredEmail]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [RegisteredPassword]
        public string Password { get; set; }

    }
}
