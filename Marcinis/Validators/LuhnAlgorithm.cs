using Marcinis.DAL;
using Marcinis.Models;
using System.ComponentModel.DataAnnotations;

namespace Marcinis.Validators
{
    public class LuhnAlgorithm : ValidationAttribute
    {
        private readonly CustomerDAL DAL = new();

        public string GetErrorMessage() => "Please enter a valid credit card number.";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string creditCardNum = (string)(value ?? string.Empty);
            

            int iNumDigits = creditCardNum.Length; // store the number of digits in the card

            int iSum = 0;
            bool isSecond = false;

            // loop through the digits in reverse creditCardNum
            for (int i = iNumDigits - 1; i >= 0; --i)
            {

                // try to convert to int--if not an integer, invalid input
                if (!int.TryParse(creditCardNum[i].ToString(), out int iDig))
                {
                    return new ValidationResult(GetErrorMessage());
                }

                // double every other number
                if (isSecond == true)
                    iDig *= 2;

                // add the sum of both digits--if only one digit, then 0 will be added to the one's place
                iSum += iDig / 10;
                iSum += iDig % 10;

                isSecond = !isSecond; // toggle so that every other number is a second number
            }
            
            if(iSum % 10 == 0) // if sum is divisible by 10, valid card number
                return ValidationResult.Success;

            return new ValidationResult(GetErrorMessage());
        }
    }
}
