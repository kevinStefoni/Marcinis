using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public byte[]? PROD_IMG { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
