using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marcinis.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public Login LoginCredentials { get; set; } = new Login();
        [Required (ErrorMessage = "Please enter your first name.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your last name.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter your phone number.")]
        public string PhoneNumber { get; set; }
        [Required]
        public int LoginTypeId { get; set; }
        public string Salt { get; set; } = string.Empty;
    }
}
