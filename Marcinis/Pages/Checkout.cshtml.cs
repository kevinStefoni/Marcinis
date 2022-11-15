using Marcinis.Helpers;
using Marcinis.Models;
using Marcinis.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Marcinis.Pages
{
    public class CheckoutModel : PageModel
    {
        [BindProperty]
        public CustomerOrder CustomerOrder { get; set; } = new CustomerOrder();

        [BindProperty]
        [Required(ErrorMessage = "Credit card expiration date is required.")]
        [CreditCardExpiry]
        public string? CardExpiry { get; set; }

        public IList<SelectListItem> AvailableTimes { get; set; } = new List<SelectListItem>();
        public void OnGet()
        {
            CustomerOrder = SessionHelper.GetObjectFromJson<CustomerOrder>(HttpContext.Session, "CustomerOrder") ?? CustomerOrder;
            RetrieveInformation();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "CustomerOrder", CustomerOrder);
            GetTimes();
        }

        public ActionResult OnPost()
        {
            RetrieveInformation();
            ParseExpiry();
            if (!ModelState.IsValid)
            {
                GetTimes();
                return Page();
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "CustomerOrder", CustomerOrder);
            return Redirect("./OrderConfirmation");
        }

        public void GetTimes()
        {
            int addedTime = 15;
            while (DateTime.Now.Add(new TimeSpan(0, 0, addedTime, 0)).CompareTo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0)) < 0)
            {
                AvailableTimes.Add(new SelectListItem { Text = DateTime.Now.Add(new TimeSpan(0, 0, addedTime, 0)).ToString("hh:mm"), Value = DateTime.Now.Add(new TimeSpan(0, 0, addedTime, 0)).ToString("hh:mm") });
                addedTime += 15;
            }
        }

        public void ParseExpiry()
        {
            if (CardExpiry == null)
                return;

            if (Int32.TryParse(CardExpiry[..2], out int m))
            {
                CustomerOrder.ORDER_CREDIT_CARD_EXP_MONTH = m;
            }

            Regex one = new (@"^[0-9]{2}/[0-9]{2}$");
            Regex two = new (@"^[0-9]{2}/[0-9]{4}$");

            if (Int32.TryParse(CardExpiry[3..], out int y))
            {
                if (one.IsMatch(CardExpiry))
                    CustomerOrder.ORDER_CREDIT_CARD_EXP_YEAR = 2000 + y;
                else if(two.IsMatch(CardExpiry))
                    CustomerOrder.ORDER_CREDIT_CARD_EXP_YEAR = y;
            }

        }

        public void RetrieveInformation()
        {
            Customer Customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "Customer") ?? new Customer();
            CustomerOrder.ORDER_CUST_ID = Customer.CustomerId;

            // add all items that Customer selected with their respective quantities to CustomerOrder
            Dictionary<string, string> OrderDetails = SessionHelper.GetObjectFromJson<Dictionary<string, string>>(HttpContext.Session, "OrderDetails") ?? new Dictionary<string, string>();
            foreach (string items in OrderDetails.Keys)
            {
                if (Int32.TryParse(OrderDetails[items], out int temp))
                {
                    if (temp != 0)
                        CustomerOrder.ORDER_ITEMS[items] = temp;

                }
            }
        }
    }
}
