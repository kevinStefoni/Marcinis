using Marcinis.DAL;
using Marcinis.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Marcinis.Validators
{
    public class CreditCardExpiry : ValidationAttribute
    {
        private readonly CustomerDAL DAL = new();

        public string GetErrorMessage() => "Please enter a valid expiration date (##/## or ##/####).";
        private IList<Regex> _regex = new List<Regex>();

        private void AddRegex(string pat)
        {
            // add a regex to the list of regexes to check
            Regex regex = new(pat);
            _regex.Add(regex);

        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string creditCardDate = (string)(value ?? string.Empty);

            AddRegex("^[0-9]{2}/[0-9]{2}$");
            AddRegex("^[0-9]{2}/[0-9]{4}$");

            if (creditCardDate != null)
                foreach (Regex r in _regex)
                    if (r.IsMatch(creditCardDate))
                        return ValidationResult.Success;

            return new ValidationResult(GetErrorMessage());
        }
    }
}
