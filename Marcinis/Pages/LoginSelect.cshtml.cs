using Marcinis.DAL;
using Marcinis.Helpers;
using Marcinis.Models;
using Marcinis.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Marcinis.Pages
{
    public class LoginSelectModel : PageModel
    {
        private readonly CustomerDAL DAL = new();

        [BindProperty]
        public Customer Customer { get; set; } = new Customer();

        public ActionResult OnPost()
        {
            // set the LoginTypeId to Guest
            Customer.LoginTypeId = 3;

            // perform custom validation for LoginSelect
            LoginSelectValidation();
            if (!ModelState.IsValid)
                return Page();

            // save Customer in session state prior to validation
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Customer", Customer);

            // go to the checkout page now that a guest account has been established
            return Redirect("./Checkout");
        }

        public void LoginSelectValidation()
        {
            // since guest account doesn't have password, set password field to valid
            ModelState.ClearValidationState("Customer.LoginCredentials.Password");
            ModelState.MarkFieldValid("Customer.LoginCredentials.Password");

            // re-match email pattern, effectively removing UnregisteredEmail attribute, but keeping the others
            Regex regex = new(".*@.*[.].*");
            if (ModelState.GetFieldValidationState("Customer.LoginCredentials.EmailAddress") == ModelValidationState.Invalid
                && Customer.LoginCredentials.EmailAddress != null
                && regex.IsMatch(Customer.LoginCredentials.EmailAddress))
            {
                ModelState.ClearValidationState("Customer.LoginCredentials.EmailAddress");
                ModelState.MarkFieldValid("Customer.LoginCredentials.EmailAddress");
            }
            // email is not registered or is not valid, so cannot login with it
            else
            {
                ModelState.ClearValidationState("Customer.LoginCredentials.EmailAddress");
                ModelState.AddModelError("Customer.LoginCredentials.EmailAddress", "Please enter a valid email.");
            }

        }
    }
}
