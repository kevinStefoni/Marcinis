using Marcinis.DAL;
using Marcinis.Models;
using System.ComponentModel.DataAnnotations;

namespace Marcinis.Validators
{
    public class RegisteredPassword : ValidationAttribute
    {
        private readonly CustomerDAL DAL = new();

        public  string GetErrorMessage() => "Please enter the correct password.";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var login = (Login) validationContext.ObjectInstance;

            // if an email and password are provided, then check if they match
            if (login.EmailAddress != null &&
                login.Password != null &&
                DAL.PasswordExists(login.EmailAddress, login.Password) == false)
                    return new ValidationResult(GetErrorMessage());
            // email provided, but no password provided
            else if (login.Password == null)
                    return new ValidationResult(GetErrorMessage());

            // successful validation
            return ValidationResult.Success;
        }
    }
}
