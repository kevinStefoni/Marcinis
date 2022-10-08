using System.ComponentModel.DataAnnotations;

namespace Marcinis.Models
{
    public class Order
    {
        [Key]
        public int ORDER_ID { get; set; }
        [Required]
        public int ORDER_CUST_ID { get; set; }
        [Required]
        public string ORDER_CREDIT_CARD_NUM { get; set; }

        [Required]
        public string ORDER_CREDIT_CARD_CVV { get; set; }
        [Required]
        public DateOnly ORDER_CREDIT_CARD_DATE { get; set; }
        [Required]
        public string ORDER_CREDIT_CARD_ZIP_CODE { get; set; }
        [Required]
        public TimeOnly ORDER_PICKUP_TIME { get; set; }
        [Required]
        public decimal ORDER_SUBTOTAL { get; set; }
        [Required]
        public decimal ORDER_TAX { get; set; }
        [Required]
        public decimal ORDER_TOTAL { get; set; }
        [Required]
        public DateOnly ORDER_DATE { get; set; }
        [Required]
        public Dictionary<MenuItem, int> ORDER_ITEMS { get; set; }

        






        

    }
}
