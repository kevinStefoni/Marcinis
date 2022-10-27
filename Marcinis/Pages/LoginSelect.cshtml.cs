using Marcinis.DAL;
using Marcinis.Helpers;
using Marcinis.Models;
using Marcinis.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Marcinis.Pages
{
    public class LoginSelectModel : PageModel
    {
        private readonly CustomerDAL DAL = new();

        [BindProperty]
        public Customer customer { get; set; }

        public void OnGet()
        {

        }

        public ActionResult OnPost()
        {
            // set the LoginTypeId to Guest
            customer.LoginTypeId = 3;

            // since guest account doesn't have password, set password field to valid
            ModelState.ClearValidationState("customer.LoginCredentials.Password");
            ModelState.MarkFieldValid("customer.LoginCredentials.Password");

            if (!ModelState.IsValid)
                return Page();

            // add the guest customer to the list of customers
            DAL.AddCustomer(customer);

            // save customer in session state prior to validation
            SessionHelper.SetObjectAsJson(HttpContext.Session, "customer", customer);

            // go to the checkout page now that a guest account has been established
            return Redirect("./Checkout");
        }
    }
}
