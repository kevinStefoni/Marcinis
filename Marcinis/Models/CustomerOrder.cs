using Marcinis.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marcinis.Models
{
    public class CustomerOrder
    {
        [Key]
        public int ORDER_ID { get; set; }

        [Required]
        public int ORDER_CUST_ID { get; set; }

        [Required (ErrorMessage ="Credit card number is required.")]
        [LuhnAlgorithm]
        [StringLength(16, ErrorMessage ="Credit card number should be 16 digits.")]
        public string? ORDER_CREDIT_CARD_NUM { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [StringLength(3, ErrorMessage = "CVV should be 3 digits.")]
        public string? ORDER_CREDIT_CARD_CVV { get; set; }

        [Required(ErrorMessage = "Credit card expiration date is required.")]
        public int ORDER_CREDIT_CARD_EXP_MONTH { get; set; }

        [Required]
        public int ORDER_CREDIT_CARD_EXP_YEAR { get; set; }

        [Required(ErrorMessage = "Zip code is required.")]
        [StringLength(5, ErrorMessage = "Zip code should be 5 digits.")]
        public string? ORDER_CREDIT_CARD_ZIP_CODE { get; set; }

        [Required(ErrorMessage = "Please select a pickup time.")]
        public DateTime ORDER_PICKUP_TIME { get; set; }

        [Required]
        public decimal ORDER_SUBTOTAL { get; set; }

        [Required]
        public decimal ORDER_TAX { get; set; }

        [Required]
        public decimal ORDER_TOTAL { get; set; }

        [Required]
        public DateTime ORDER_DATE { get; set; }

        [Required]
        public Dictionary<string, int> ORDER_ITEMS = new Dictionary<string, int>();









    }
}
