using Marcinis.DAL;
using Marcinis.Models;
using System.ComponentModel.DataAnnotations;


namespace Marcinis.Validators
{
    public class UnregisteredEmail : ValidationAttribute
    {
        private readonly CustomerDAL DAL = new();

        public string GetErrorMessage() => "Please enter a valid email.";
        public string GetErrorMessage2() => "Email already exists.";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var login = (Login) validationContext.ObjectInstance;

            if (login.EmailAddress != null && DAL.EmailExists(login.EmailAddress) == true)
                return new ValidationResult(GetErrorMessage2());
            else if (login.EmailAddress == null)
                return new ValidationResult(GetErrorMessage());

            return ValidationResult.Success;
        }
    }
}
