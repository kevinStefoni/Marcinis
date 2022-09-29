using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marcinis.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public Login? LoginCredentials { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int LoginTypeId { get; set; }
        public string Salt { get; set; } = string.Empty;
    }
}
