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
        public int id { get; set; }

        [BindProperty]
        public int itemId { get; set; }

        [BindProperty]
        public int orderId { get; set; }

        [BindProperty]
        public int discountId { get; set; }

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

        public ActionResult OnPost()
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


            itemRepo.AddMenuItem(item);

            return Redirect("./Admin");
        }

        public ActionResult OnPostAddCustomer()
        {
            customerRepo.AddCustomer(cust);

            return Redirect("./Admin");
        }

        public ActionResult OnPostAddDiscountCode()
        {
            itemRepo.AddDiscountCode(disc);

            return Redirect("./Admin");
        }

        public void OnPostCustomerDelete()
        {
            customerRepo.DeleteCustomer(id);
            customer = customerRepo.GetAllCustomers();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "customers", customer);
        }

        public void OnPostMenuItemDelete()
        {
            itemRepo.DeleteMenuItem(itemId);
            menu = itemRepo.GetMenu();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);
        }

        public void OnPostOrderDelete()
        {
            itemRepo.DeleteMenuItem(orderId);
            orders = itemRepo.GetOrders();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "orders", orders);
        }

        public void OnPostDiscountCodeDelete()
        {
            itemRepo.DeleteDiscountCode(discountId);
            discounts = itemRepo.GetDiscountCodes();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "discounts", discounts);
        }
    }
}
