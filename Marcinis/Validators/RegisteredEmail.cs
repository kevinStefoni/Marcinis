using Marcinis.DAL;
using Marcinis.Models;
using System.ComponentModel.DataAnnotations;


namespace Marcinis.Validators
{
    public class RegisteredEmail : ValidationAttribute
    {
        private readonly CustomerDAL DAL = new();

        public string GetErrorMessage() => "Please enter a valid email.";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var login = (Login) validationContext.ObjectInstance;

            if (login.EmailAddress != null && DAL.EmailExists(login.EmailAddress) == false)
                return new ValidationResult(GetErrorMessage());
            else if (login.EmailAddress == null)
                return new ValidationResult(GetErrorMessage());

            return ValidationResult.Success;
        }
    }
}
