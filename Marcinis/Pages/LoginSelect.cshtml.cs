using Marcinis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marcinis.Pages
{
    public class LoginSelectModel : PageModel
    {
        [BindProperty]
        public Customer customer { get; set; }
        public void OnGet()
        {

        }

        public void OnPost()
        {

        }
    }
}
