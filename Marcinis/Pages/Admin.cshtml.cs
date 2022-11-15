using System;
using System.Data;
using Marcinis.DAL;
using Marcinis.Helpers;
using Marcinis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Marcinis.Pages
{
    public class AdminModel : PageModel
    {
        private readonly CustomerDAL customerRepo = new();
        private readonly OrderDAL itemRepo = new();

        [BindProperty]
        public IList<Customer> customer { get; set; }

        [BindProperty]
        public Customer cust { get; set; } //customer value for submission form on page

        [BindProperty]
        public MenuItem item { get; set; }

        [BindProperty]
        public DiscountCode disc { get; set; } //discount value for submission form on page

        [BindProperty]
        public IList<MenuItem> menu { get; set; }

        [BindProperty]
        public IList<CustomerOrder> orders { get; set; }

        [BindProperty]
        public IList<DiscountCode> discounts { get; set; }

        [BindProperty]
        public int bindCustId { get; set; }

        [BindProperty]
        public int bindItemId { get; set; }

        [BindProperty]
        public int bindOrderId { get; set; }

        [BindProperty]
        public int bindDiscountId { get; set; }

        public void OnGet()
        {
            customer = customerRepo.GetAllCustomers();
            menu = itemRepo.GetMenu();
            orders = itemRepo.GetOrders();
            discounts = itemRepo.GetDiscountCodes();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "customers", customer);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "orders", orders);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "discounts", discounts);
        }

        public ActionResult OnPostAddMenuItem()
        {
            byte[] bytes = null;
            if (item.ImageFile != null)
            {
                using (Stream fs = item.ImageFile.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        bytes = br.ReadBytes((Int32)fs.Length);
                    }
                }
                item.PROD_IMG = bytes;
            }

            bool found = false;
            IList<MenuItem>? Menu = SessionHelper.GetObjectFromJson<IList<MenuItem>>(HttpContext.Session, "menu");
            foreach (var i in Menu)
            {
                if (i.PROD_ID == item.PROD_ID)
                {
                    found = true;
                    if (item.PROD_IMG == null) { item.PROD_IMG = i.PROD_IMG; }
                    
                    itemRepo.UpdateMenuItem(item);
                    menu = itemRepo.GetMenu();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);
                }
            }

            if (!found) { itemRepo.AddMenuItem(item); }

            return Redirect("./Admin");
        }

        public ActionResult OnPostAddCustomer()
        {
            bool found = false;

            IList<Customer>? Customers = SessionHelper.GetObjectFromJson<IList<Customer>>(HttpContext.Session, "customers");
            foreach (var c in Customers)
            {
                if (c.CustomerId == cust.CustomerId)
                {
                    found = true;
                    customerRepo.UpdateCustomer(cust);
                    customer = customerRepo.GetAllCustomers();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "customers", customer);
                }
            }
            if (!found)
            {
                if (cust.LoginCredentials.Password == null) 
                {
                    cust.LoginCredentials.Password = "Pa$$word1";
                }
                customerRepo.AddCustomer(cust);
            }

            return Redirect("./Admin");
        }

        public ActionResult OnPostAddDiscountCode()
        {
            itemRepo.AddDiscountCode(disc);

            return Redirect("./Admin");
        }

        public void OnPostCustomerDelete()
        {
            customerRepo.DeleteCustomer(bindCustId);
            customer = customerRepo.GetAllCustomers();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "customers", customer);
        }

        public void OnPostMenuItemDelete()
        {
            itemRepo.DeleteMenuItem(bindItemId);
            menu = itemRepo.GetMenu();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);
        }

        public void OnPostOrderDelete()
        {
            itemRepo.DeleteOrder(bindOrderId);
            orders = itemRepo.GetOrders();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "orders", orders);
        }

        public void OnPostDiscountCodeDelete()
        {
            itemRepo.DeleteDiscountCode(bindDiscountId);
            disc.Code = "test";
            discounts = itemRepo.GetDiscountCodes();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "discounts", discounts);
        }

        public void OnPostCustomerEdit()
        {
            IList<Customer>? Customers = SessionHelper.GetObjectFromJson<IList<Customer>>(HttpContext.Session, "customers");
            foreach (var c in Customers) {
                if (c.CustomerId == bindCustId)
                {
                    cust.CustomerId = c.CustomerId;
                    cust.FirstName = c.FirstName;
                    cust.LastName = c.LastName;
                    cust.LoginCredentials.EmailAddress = c.LoginCredentials.EmailAddress;
                    cust.PhoneNumber = c.PhoneNumber;
                    cust.LoginTypeId = c.LoginTypeId;
                }
            }         
        }


        public void OnPostMenuItemEdit()
        {
            IList<MenuItem>? Menu = SessionHelper.GetObjectFromJson<IList<MenuItem>>(HttpContext.Session, "menu");
            foreach (var i in Menu)
            {
                if (i.PROD_ID == bindItemId)
                {
                    item.PROD_ID = i.PROD_ID;
                    item.PROD_NAME = i.PROD_NAME;
                    item.PROD_DESC = i.PROD_DESC;
                    item.PROD_TYPE = i.PROD_TYPE;
                    item.PROD_PRICE = i.PROD_PRICE;
                    item.PROD_QOH = i.PROD_QOH;
                    item.PROD_CATEGORY = i.PROD_CATEGORY;
                }
            }
        }
    }
}
