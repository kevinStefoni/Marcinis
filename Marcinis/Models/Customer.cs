using System.ComponentModel.DataAnnotations;

namespace Marcinis.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int LoginTypeId { get; set; }
        public string Salt { get; set; }
    }
}
