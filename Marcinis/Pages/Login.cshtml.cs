using System.Data;
using Marcinis.DAL;
using Marcinis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Marcinis.Helpers;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace Marcinis.Pages
{
    public class LoginModel : PageModel
    {
        private readonly CustomerDAL DAL = new();

        [BindProperty]
        public Customer Customer { get; set; } = new Customer();

        public ActionResult OnPostLogin()
        {
            // validate input if user is just logging in
            ValidateLogin();
            if (!ModelState.IsValid)
            {
                ViewData["LAST_BUTTON"] = "Login";
                return Page();
            }

            CleanUpPhoneNumber();

            Customer = DAL.GetCustomer(Customer.LoginCredentials.EmailAddress) ?? Customer;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Customer", Customer);
            
            return (Customer != null) ? Redirect("./Index") : Redirect("./Login");
            
        }

        public ActionResult OnPostRegister()
        {
            // validate input if user is just logging in
            ValidateRegister();
            if (!ModelState.IsValid)
            {
                ViewData["LAST_BUTTON"] = "Register";
                return Page();
            }

            CleanUpPhoneNumber();
            Customer.LoginTypeId = 2;

            SessionHelper.SetObjectAsJson(HttpContext.Session, "Customer", Customer);
            DAL.AddCustomer(Customer);

            return (Customer != null) ? Redirect("./Index") : Redirect("./Login");

        }

        public void ValidateLogin()
        {
            // make a list of all fields that are not needed
            IList<string> fields = new List<string>();
            fields.Add("Customer.FirstName");
            fields.Add("Customer.LastName");
            fields.Add("Customer.PhoneNumber");

            // make all unnecessary fields valid to prevent false negative on validation
            foreach (string f in fields)
            {
                ModelState.ClearValidationState(f);
                ModelState.MarkFieldValid(f);
            }

            // re-match email pattern, effectively removing UnregisteredEmail attribute, but keeping the others
            Regex regex = new (".*@.*[.].*");
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

        public void ValidateRegister()
        {
            // password is being set, so it just has to not be empty
            ModelState.ClearValidationState("Customer.LoginCredentials.Password");

            if(Customer.LoginCredentials.Password != null)
                ModelState.MarkFieldValid("Customer.LoginCredentials.Password");
            else
                ModelState.AddModelError("Customer.LoginCredentials.Password", "Please enter a password.");
        }

        public void CleanUpPhoneNumber()
        {
            // remove all (, ), -, and spaces from phone number, to make it have only digits
            if (Customer != null && Customer.PhoneNumber != null && Customer.PhoneNumber.Contains('('))
                Customer.PhoneNumber = Customer.PhoneNumber.Replace("(", "");
            if (Customer != null && Customer.PhoneNumber != null && Customer.PhoneNumber.Contains(')'))
                Customer.PhoneNumber = Customer.PhoneNumber.Replace(")", "");
            if (Customer != null && Customer.PhoneNumber != null && Customer.PhoneNumber.Contains('-'))
                Customer.PhoneNumber = Customer.PhoneNumber.Replace("-", "");
            if (Customer != null && Customer.PhoneNumber != null && Customer.PhoneNumber.Contains(' '))
                Customer.PhoneNumber = Customer.PhoneNumber.Replace(" ", "");
        }
    }
}
