using System.ComponentModel.DataAnnotations;

namespace Marcinis.Models
{
    public class MenuItem
    {
        public int PROD_ID { get; set; }
        public string? PROD_NAME { get; set; }
        public string? PROD_DESC { get; set; }
        public string? PROD_TYPE { get; set; }
        public decimal PROD_PRICE { get; set; }
        public int PROD_QOH { get; set; }
        public string? PROD_CATEGORY { get; set; }

    }
}
