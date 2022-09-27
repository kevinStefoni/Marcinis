using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Marcinis.Pages
{
    public class Order : PageModel
    {
        private readonly ILogger<Order> _logger;

        [Required (ErrorMessage = "Quantity is required.")]
        public int Qty { get; set; }
        public Order(ILogger<Order> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {

        }
    }
}