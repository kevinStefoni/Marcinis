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

        [BindProperty]
        public string? SortType { get; set; }

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

        public void OnPost()
        {
            OnGet();
        }

        public ActionResult OnPostAddMenuItem()
        {
            ViewData["WHERETO"] = "productTable";

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
            ViewData["WHERETO"] = "customerTable";

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
            ViewData["WHERETO"] = "discountTable";
            itemRepo.AddDiscountCode(disc);
            return Redirect("./Admin");
        }

        public void OnPostCustomerDelete()
        {
            ViewData["WHERETO"] = "customerTable";
            customerRepo.DeleteCustomer(bindCustId);
            customer = customerRepo.GetAllCustomers();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "customers", customer);
        }

        public void OnPostMenuItemDelete()
        {
            ViewData["WHERETO"] = "productTable";
            itemRepo.DeleteMenuItem(bindItemId);
            menu = itemRepo.GetMenu();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);
        }

        public void OnPostOrderDelete()
        {
            ViewData["WHERETO"] = "orderTable";
            itemRepo.DeleteOrder(bindOrderId);
            orders = itemRepo.GetOrders();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "orders", orders);
        }

        public void OnPostDiscountCodeDelete()
        {
            ViewData["WHERETO"] = "discountTable";
            itemRepo.DeleteDiscountCode(bindDiscountId);
            disc.Code = "test";
            discounts = itemRepo.GetDiscountCodes();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "discounts", discounts);
        }

        public void OnPostCustomerEdit()
        {
            ViewData["WHERETO"] = "custInput";
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
            ViewData["WHERETO"] = "menuItemInput";
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
                    if (i.PROD_IMG != null)
                    {
                        item.PROD_IMG = i.PROD_IMG as byte[];
                    }
                    
                }
            }
        }

        public void OnPostProduct()
        {
            OnGet();
            ViewData["WHERETO"] = "productTable";
        }

        public void OnPostCustomer()
        {
            OnGet();
            ViewData["WHERETO"] = "customerTable";
        }

        public void OnPostOrder()
        {
            OnGet();
            ViewData["WHERETO"] = "orderTable";
        }

        public void OnPostDiscount()
        {
            OnGet();
            ViewData["WHERETO"] = "discountTable";
        }

        public void OnPostSortCustomers()
        {
            ViewData["WHERETO"] = "customerTable";
            customer = SessionHelper.GetObjectFromJson<IList<Customer>>(HttpContext.Session, "customers") ?? customer;
            
            switch (SortType)
            {
                case "customerid":
                    ViewData["ADMIN_DISPLAY"] = "customerid";
                    customer = customer.OrderBy(c => c.CustomerId).ToList();
                    break;

                case "firstname":
                    ViewData["ADMIN_DISPLAY"] = "firstname";
                    customer = customer.OrderBy(c => c.FirstName).ToList();
                    break;

                case "lastname":
                    ViewData["ADMIN_DISPLAY"] = "lastname";
                    customer = customer.OrderBy(c => c.LastName).ToList();
                    break;

                case "emailaddress":
                    ViewData["ADMIN_DISPLAY"] = "emailaddress";
                    customer = customer.OrderBy(c => c.LoginCredentials.EmailAddress).ToList();
                    break;

                case "phonenumber":
                    ViewData["ADMIN_DISPLAY"] = "phonenumber";
                    customer = customer.OrderBy(c => c.PhoneNumber).ToList();
                    break;

                case "logintype":
                    ViewData["ADMIN_DISPLAY"] = "logintype";
                    customer = customer.OrderBy(c => c.LoginTypeId).ToList();
                    break;
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "customers", customer);
        }
        public void OnPostSortMenuItems()
        {
            ViewData["WHERETO"] = "productTable";
            menu = SessionHelper.GetObjectFromJson<IList<MenuItem>>(HttpContext.Session, "menu") ?? menu;

            switch (SortType)
            {
                case "productid":
                    ViewData["ADMIN_DISPLAY"] = "productid";
                    menu = menu.OrderBy(m => m.PROD_ID).ToList();
                    break;

                case "productname":
                    ViewData["ADMIN_DISPLAY"] = "productname";
                    menu = menu.OrderBy(m => m.PROD_NAME).ToList();
                    break;

                case "productdesc":
                    ViewData["ADMIN_DISPLAY"] = "productdesc";
                    menu = menu.OrderBy(m => m.PROD_DESC).ToList();
                    break;

                case "producttype":
                    ViewData["ADMIN_DISPLAY"] = "producttype";
                    menu = menu.OrderBy(m => m.PROD_TYPE).ToList();
                    break;

                case "productprice":
                    ViewData["ADMIN_DISPLAY"] = "productprice";
                    menu = menu.OrderBy(m => m.PROD_PRICE).ToList();
                    break;

                case "productqoh":
                    ViewData["ADMIN_DISPLAY"] = "productqoh";
                    menu = menu.OrderBy(m => m.PROD_QOH).ToList();
                    break;

                case "productcategory":
                    ViewData["ADMIN_DISPLAY"] = "productcategory";
                    menu = menu.OrderBy(m => m.PROD_CATEGORY).ToList();
                    break;
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);
        }

        public void OnPostSortOrders()
        {
            ViewData["WHERETO"] = "orderTable";
            orders = SessionHelper.GetObjectFromJson<IList<CustomerOrder>>(HttpContext.Session, "orders") ?? orders;

            switch (SortType)
            {
                case "orderid":
                    ViewData["ADMIN_DISPLAY"] = "orderid";
                    orders = orders.OrderBy(o => o.ORDER_ID).ToList();
                    break;

                case "customerid":
                    ViewData["ADMIN_DISPLAY"] = "customerid";
                    orders = orders.OrderBy(o => o.ORDER_CUST_ID).ToList();
                    break;

                case "ccnumber":
                    ViewData["ADMIN_DISPLAY"] = "ccnumber";
                    orders = orders.OrderBy(o => o.ORDER_CREDIT_CARD_NUM).ToList();
                    break;

                case "cccvv":
                    ViewData["ADMIN_DISPLAY"] = "cccvv";
                    orders = orders.OrderBy(o => o.ORDER_CREDIT_CARD_CVV).ToList();
                    break;

                case "ccexpirymonth":
                    ViewData["ADMIN_DISPLAY"] = "ccexpirymonth";
                    orders = orders.OrderBy(o => o.ORDER_CREDIT_CARD_EXP_MONTH).ToList();
                    break;

                case "ccexpiryyear":
                    ViewData["ADMIN_DISPLAY"] = "ccexpiryyear";
                    orders = orders.OrderBy(o => o.ORDER_CREDIT_CARD_EXP_YEAR).ToList();
                    break;

                case "cczipcode":
                    ViewData["ADMIN_DISPLAY"] = "cczipcode";
                    orders = orders.OrderBy(o => o.ORDER_CREDIT_CARD_ZIP_CODE).ToList();
                    break;

                case "orderpickuptime":
                    ViewData["ADMIN_DISPLAY"] = "orderpickuptime";
                    orders = orders.OrderBy(o => o.ORDER_PICKUP_TIME).ToList();
                    break;

                case "ordersubtotal":
                    ViewData["ADMIN_DISPLAY"] = "ordersubtotal";
                    orders = orders.OrderBy(o => o.ORDER_SUBTOTAL).ToList();
                    break;

                case "ordertotal":
                    ViewData["ADMIN_DISPLAY"] = "ordertotal";
                    orders = orders.OrderBy(o => o.ORDER_TOTAL).ToList();
                    break;

                case "orderdate":
                    ViewData["ADMIN_DISPLAY"] = "orderdate";
                    orders = orders.OrderBy(o => o.ORDER_DATE).ToList();
                    break;

            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "orders", orders);
        }

        public void OnPostSortDiscounts()
        {
            ViewData["WHERETO"] = "discountTable";
            discounts = SessionHelper.GetObjectFromJson<IList<DiscountCode>>(HttpContext.Session, "discounts") ?? discounts;

            switch (SortType)
            {
                case "discountid":
                    ViewData["ADMIN_DISPLAY"] = "discountid";
                    discounts = discounts.OrderBy(d => d.DiscountId).ToList();
                    break;

                case "customercode":
                    ViewData["ADMIN_DISPLAY"] = "customercode";
                    discounts = discounts.OrderBy(d => d.Code).ToList();
                    break;

                case "percentoff":
                    ViewData["ADMIN_DISPLAY"] = "percentoff";
                    discounts = discounts.OrderBy(d => d.Percentage).ToList();
                    break;

                case "startdate":
                    ViewData["ADMIN_DISPLAY"] = "startdate";
                    discounts = discounts.OrderBy(d => d.StartDate).ToList();
                    break;

                case "enddate":
                    ViewData["ADMIN_DISPLAY"] = "enddate";
                    discounts = discounts.OrderBy(d => d.EndDate).ToList();
                    break;
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "discounts", discounts);
        }


    }
}
