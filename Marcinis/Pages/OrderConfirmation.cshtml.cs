using Marcinis.Helpers;
using Marcinis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;

namespace Marcinis.Pages
{
    public class OrderConfirmationModel : PageModel
    {
        [BindProperty]
        public Customer? Customer { get; set; }

        [BindProperty]
        public Dictionary<string, decimal> itemPairValues { get; set; } = new Dictionary<string, decimal>();

        [BindProperty]
        public CustomerOrder? CustomerOrder { get; set; }

        public void OnGet()
        {
            Customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "Customer");
            CustomerOrder = SessionHelper.GetObjectFromJson<CustomerOrder>(HttpContext.Session, "CustomerOrder") ?? CustomerOrder;
        }


    }
}
