using Marcinis.DAL;
using Marcinis.Helpers;
using Marcinis.Models;
using Marcinis.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Marcinis.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly OrderDAL DAL = new();
        private readonly CustomerDAL CDAL = new();

        [BindProperty]
        public CustomerOrder CustomerOrder { get; set; } = new CustomerOrder();

        [BindProperty]
        public Customer Customer { get; set; } = new Customer();


        public IList<MenuItem> menu = new List<MenuItem>();

        [BindProperty]
        [Required(ErrorMessage = "Credit card expiration date is required.")]
        [CreditCardExpiry]
        public string? CardExpiry { get; set; }

        [BindProperty]
        public string? DiscountCode { get; set; }

        public Dictionary<string, decimal> itemPairValues { get; set; } = new Dictionary<string, decimal>();

        public IList<SelectListItem> AvailableTimes { get; set; } = new List<SelectListItem>();

        public void OnGet()
        {
            CustomerOrder = SessionHelper.GetObjectFromJson<CustomerOrder>(HttpContext.Session, "CustomerOrder") ?? CustomerOrder;
            Customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "Customer") ?? Customer;
            RetrieveInformation();
            //On get we are setting customer order but we are also doing that OnPost. is this causing a bug when returning to cart and total doubling
            SessionHelper.SetObjectAsJson(HttpContext.Session, "CustomerOrder", CustomerOrder);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "itemPairValues", itemPairValues);
            GetTimes();
        }

        public ActionResult OnPost()
        {
            RetrieveInformation();
            ParseExpiry();
            FixValidation();
            if (!ModelState.IsValid)
            {
                GetTimes();
                return Page();
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "CustomerOrder", CustomerOrder);
            DAL.AddOrder(CustomerOrder);
            if(Customer.LoginTypeId == 3)
                CDAL.AddCustomer(Customer);
            return Redirect("./OrderConfirmation");
        }

        public void GetTimes()
        {
            int addedTime = 15;
            while (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")).Add(new TimeSpan(0, 0, addedTime, 0)).CompareTo(new DateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")).Year, TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")).Month, TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")).Day + 1, 20, 0, 0)) < 0)
            {
                AvailableTimes.Add(new SelectListItem { Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")).Add(new TimeSpan(0, 0, addedTime, 0)).ToString("hh:mm"), Value = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")).Add(new TimeSpan(0, 0, addedTime, 0)).ToString("hh:mm") });
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

            Regex one = new(@"^[0-9]{2}/[0-9]{2}$");
            Regex two = new(@"^[0-9]{2}/[0-9]{4}$");

            if (Int32.TryParse(CardExpiry[3..], out int y))
            {
                if (one.IsMatch(CardExpiry))
                    CustomerOrder.ORDER_CREDIT_CARD_EXP_YEAR = 2000 + y;
                else if (two.IsMatch(CardExpiry))
                    CustomerOrder.ORDER_CREDIT_CARD_EXP_YEAR = y;
            }

        }

        public void RetrieveInformation()
        {
            Customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "Customer") ?? new Customer();
            CustomerOrder.ORDER_CUST_ID = Customer.CustomerId;
            CustomerOrder.ORDER_DATE = DateTime.Now.Date;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "CustomerOrder", CustomerOrder);
            menu = DAL.GetMenu() ?? menu;
            decimal TEXAS_TAX_RATE = .0825m;

            // iterate through menu and add the product name and price to dictionary "itemPairValues"
            foreach (MenuItem item in menu)
            {
                if (item.PROD_NAME != null && !itemPairValues.ContainsKey(item.PROD_NAME))
                {
                    itemPairValues.Add(item.PROD_NAME, item.PROD_PRICE);
                }
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "itemPairValues", itemPairValues);
            // add all items that Customer selected with their respective quantities to CustomerOrder and calculate ORDER_SUBTOTAL
            Dictionary<string, string> OrderDetails = SessionHelper.GetObjectFromJson<Dictionary<string, string>>(HttpContext.Session, "OrderDetails") ?? new Dictionary<string, string>();
            CustomerOrder.ORDER_SUBTOTAL = 0; // reset order subtotal before adding it again
            foreach (string items in OrderDetails.Keys)
            {
                if (Int32.TryParse(OrderDetails[items], out int temp))
                {
                    if (temp != 0)
                    {
                        CustomerOrder.ORDER_ITEMS[items] = temp;
                        CustomerOrder.ORDER_SUBTOTAL += CustomerOrder.ORDER_ITEMS[items] * itemPairValues[items];
                    }
                }
            }

            // calculate the ORDER_TAX amount by mulitplying the subtotal by the tax rate and update CustomerOrder
            CustomerOrder.ORDER_TAX = CustomerOrder.ORDER_SUBTOTAL * TEXAS_TAX_RATE;

            // calculate the ORDER_TOTAL by adding subtotal and tax amount and update CustomerOrder
            CustomerOrder.ORDER_TOTAL = CustomerOrder.ORDER_SUBTOTAL + CustomerOrder.ORDER_TAX;

            if (DiscountCode != null)
            {
                decimal percentOff = DAL.GetDiscount(DiscountCode);
                if (percentOff != -1)
                {
                    ViewData["ISDISC"] = "YES";
                    CustomerOrder.ORDER_TOTAL = CustomerOrder.ORDER_TOTAL - (CustomerOrder.ORDER_TOTAL * (percentOff / 100));
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "percentOff", percentOff);
                }
                else
                {
                    ViewData["ISDISC"] = "NO";
                }
            }

        }

        public void FixValidation()
        {
            ModelState.ClearValidationState("Customer.LoginCredentials.Password");
            ModelState.MarkFieldValid("Customer.LoginCredentials.Password");
            ModelState.ClearValidationState("Customer.LoginCredentials.EmailAddress");
            ModelState.MarkFieldValid("Customer.LoginCredentials.EmailAddress");
        }
    }
}