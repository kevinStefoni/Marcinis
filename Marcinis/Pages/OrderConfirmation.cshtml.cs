using Marcinis.Helpers;
using Marcinis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marcinis.Pages
{
    public class OrderConfirmationModel : PageModel
    {
        [BindProperty]
        public Customer? Customer { get; set; }

        public void OnGet()
        {
            Customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "customer");
        }
    }
}
