using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marcinis.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required (ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Display (Name = "Email")]
        [Required (ErrorMessage = "Please enter an email.")]
        [EmailAddress (ErrorMessage = "Invalid email.")]
        public string EmailAddress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int LoginTypeId { get; set; }
        public string Salt { get; set; } = string.Empty;
    }
}
