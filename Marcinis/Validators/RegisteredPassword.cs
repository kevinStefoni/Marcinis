using Marcinis.DAL;
using Marcinis.Helpers;
using Marcinis.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace Marcinis.Validators
{
    public static class RegisteredPassword
    {
        private static readonly CustomerDAL DAL = new();

        public static string GetErrorMessage() => "Please enter the correct password.";
        static public ValidationResult? IsValid(Customer customer)
        {
            // if it is a guest, then password always passes validation
            if (customer.LoginTypeId == 3)
            {
                return ValidationResult.Success;
            }
            // if an email and password are provided, then check if they match
            else if (customer.LoginCredentials.EmailAddress != null &&
                customer.LoginCredentials.Password != null &&
                DAL.PasswordExists(customer.LoginCredentials.EmailAddress, customer.LoginCredentials.Password) == false)
                    return new ValidationResult(GetErrorMessage());
            // email provided, but no password provided
            else if (customer.LoginCredentials.Password == null)
                    return new ValidationResult(GetErrorMessage());

            // successful validation
            return ValidationResult.Success;
        }
    }
}
