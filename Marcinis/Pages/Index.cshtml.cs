using Marcinis.Helpers;
using Marcinis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marcinis.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Customer? customer { get; set; }

        public void OnGet()
        {
            customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "customer");
        }
    }
}
