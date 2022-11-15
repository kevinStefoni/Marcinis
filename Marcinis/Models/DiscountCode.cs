using System.ComponentModel.DataAnnotations;

namespace Marcinis.Models
{
    public class DiscountCode
    {
        [Key]
        public int DiscountId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public int Percentage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
