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
        public CustomerOrder CustomerOrder { get; set; } = new CustomerOrder();

        public void OnGet()
        {
            CustomerOrder = SessionHelper.GetObjectFromJson<CustomerOrder>(HttpContext.Session, "CustomerOrder") ?? CustomerOrder;
        }

        public void OnPost()
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "CustomerOrder", CustomerOrder);
        }
    }
}
