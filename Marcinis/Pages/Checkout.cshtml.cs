using Marcinis.Helpers;
using Marcinis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marcinis.Pages
{
    public class CheckoutModel : PageModel
    {
        [BindProperty]
        public Customer? Customer { get; set; }

        [BindProperty]
        public CustomerOrder? CustomerOrder { get; set; }

        [BindProperty]
        public Dictionary<string, string> OrderDetails { get; set; } = new Dictionary<string, string>();

        public void OnGet()
        {
            OrderDetails = SessionHelper.GetObjectFromJson<Dictionary<string, string>>(HttpContext.Session, "OrderDetails") ?? OrderDetails;
        }
    }
}
