
using Marcinis.DAL;
using Marcinis.Helpers;
using Marcinis.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;

namespace Marcinis.Validators
{
    public class ValidPhoneNumber : ValidationAttribute
    {
        private readonly CustomerDAL DAL = new();

        public string GetErrorMessage() => "Please enter a valid phone number.";
        private IList<Regex> _regex = new List<Regex>();

        private void AddRegex(string pat)
        {
            // add a regex to the list of regexes to check
            Regex regex = new (pat);
            _regex.Add(regex);

        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;

            // (XXX)XXXXXXX
            AddRegex(@"^\([0-9]{3}\)[0-9]{7}$");

            // (XXX) XXXXXXX
            AddRegex(@"^\([0-9]{3}\) [0-9]{7}$");
            
            // (XXX) XXX XXXX
            AddRegex(@"^\([0-9]{3}\) [0-9]{3} [0-9]{4}$");
         
            // (XXX) - XXX - XXXX
            AddRegex(@"^\([0-9]{3}\) - [0-9]{3} - [0-9]{4}$");

            // (XXX)-XXX-XXXX
            AddRegex(@"^\([0-9]{3}\)-[0-9]{3}-[0-9]{4}$");

            // XXXXXXXXXX
            AddRegex(@"^[0-9]{10}$");

            // XXX-XXX-XXXX
            AddRegex(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$");

            // XXX XXX XXXX
            AddRegex(@"^[0-9]{3} [0-9]{3} [0-9]{4}$");

            // XXX XXXXXXX
            AddRegex(@"^[0-9]{3} [0-9]{7}$");

            // XXX - XXX - XXXX
            AddRegex(@"^[0-9]{3} - [0-9]{3} - [0-9]{4}$");

            // check each case until one matches
            if(customer != null && customer.PhoneNumber != null)
                foreach (Regex r in _regex)
                    if (r.IsMatch(customer.PhoneNumber))
                        return ValidationResult.Success;

            // no matches so it is invalid
            return new ValidationResult(GetErrorMessage());
        }
    }
}
