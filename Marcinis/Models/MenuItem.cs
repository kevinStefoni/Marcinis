using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marcinis.Models
{
    public class MenuItem
    {
        public int PROD_ID { get; set; }
        [Required (ErrorMessage = "Please enter a product name.")]
        public string? PROD_NAME { get; set; }

        [Required (ErrorMessage = "Please enter a product description.")]
        public string? PROD_DESC { get; set; }

        [Required(ErrorMessage = "Please enter a product type.")]
        public string? PROD_TYPE { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        [Required(ErrorMessage = "Please enter a product description.")]
        public decimal PROD_PRICE { get; set; }

        [Required(ErrorMessage = "Please enter the quantity on hand for that product.")]
        public int PROD_QOH { get; set; }

        [Required(ErrorMessage = "Please enter a product category.")]
        public string? PROD_CATEGORY { get; set; }

        [Required(ErrorMessage = "Please select a picture for the product. 440px x 330px is preferred.")]
        public byte[]? PROD_IMG { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }


    }
}
