using System.Text;
using Marcinis.DAL;
using Marcinis.Enums;
using Marcinis.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marcinis.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly CustomerDAL customerRepo = new();

        [BindProperty]
        public Customer customer { get; set; }

        public ActionResult OnPost()
        {
            // generate salt and hash password
            customer.Salt = Utilities.GeneratePasswordSalt();
            customer.LoginCredentials.Password = Utilities.GeneratePasswordHash(Convert.FromBase64String(customer.Salt), customer.LoginCredentials.Password);

            // registration via this page will be customer logintypes
            customer.LoginTypeId = (int)LoginType.Customer;

            if (!ModelState.IsValid)
                return Page();

            customerRepo.AddCustomer(customer);

            return Redirect("./Login");
        }
    }
}
