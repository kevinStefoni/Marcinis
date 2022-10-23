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
        
        /*
         * [BindProperty]
         * public Order? Order { get; set; }
         */

        public void OnGet()
        {
            Customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "customer");
        }

        /*
         *public void onGet()
         *{
         *  Order = SessionHelper.GetObjectFromJson<Order>(HttpContext.Session, "order");
         *}
         */

    }
}
