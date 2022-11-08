using Microsoft.AspNetCore.Mvc.RazorPages;
using Marcinis.Models;
using Marcinis.DAL;
using Marcinis.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Marcinis.Pages
{
    public class Order : PageModel
    {

        private readonly OrderDAL DAL = new();

        [BindProperty]
        public CustomerOrder CusOrder { get; set; } = new CustomerOrder();

        [BindProperty]
        public Dictionary<string, string> OrderDetails { get; set; } = new Dictionary<string, string>();

        [BindProperty]
        public string? Search { get; set; }

        public IList<MenuItem> menu = new List<MenuItem>();
        public IList<string> categories = new List<string>();
        

        public void OnGet()
        {
            // retrieve OrderDetails
            OrderDetails = SessionHelper.GetObjectFromJson<Dictionary<string, string>>(HttpContext.Session, "OrderDetails") ?? OrderDetails;

            // only change if something is returned from DAL
            menu = DAL.GetMenu() ?? menu;

            // save the menu for use in the frontend, when the page has to be remade after POST
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);

            // only change if something is returned from DAL
            categories = DAL.GetCategories() ?? categories;

            // always have Dessert category at end
            categories = categories.OrderBy(i => i == "Dessert").ThenBy(i => i).ToList();

            // save the categories for use in the frontend, when the page has to be remade after POST
            SessionHelper.SetObjectAsJson(HttpContext.Session, "categories", categories);

        }

        public void OnPost()
        {
            // save the OrderDetails in the session too
            SessionHelper.SetObjectAsJson(HttpContext.Session, "OrderDetails", OrderDetails);

/*            // add all items that Customer selected with their respective quantities to CusOrder
            foreach(string items in OrderDetails.Keys)
            {
                if (Int32.TryParse(OrderDetails[items], out int temp))
                {
                    CusOrder.ORDER_ITEMS[items] = temp;

                }
            }

            // save the CusOrder in the session
            SessionHelper.SetObjectAsJson(HttpContext.Session, "CusOrder", CusOrder);*/
        }

        public void OnPostSearch()
        {
            if (OrderDetails.ContainsKey("Search"))
                OrderDetails = SessionHelper.GetObjectFromJson<Dictionary<string, string>>(HttpContext.Session, "OrderDetails") ?? OrderDetails;
            else
                SessionHelper.SetObjectAsJson(HttpContext.Session, "OrderDetails", OrderDetails);

            menu = DAL.SearchMenu(Search ?? string.Empty) ?? menu;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);

            categories.Clear();
            foreach(MenuItem item in menu)
                if(item.PROD_CATEGORY != null && !categories.Contains(item.PROD_CATEGORY))
                    categories.Add(item.PROD_CATEGORY);

            SessionHelper.SetObjectAsJson(HttpContext.Session, "categories", categories);
        }

        public void OnPostSortByPrice()
        {
            if (OrderDetails.ContainsKey("Search"))
                OrderDetails = SessionHelper.GetObjectFromJson<Dictionary<string, string>>(HttpContext.Session, "OrderDetails") ?? OrderDetails;
            else
                SessionHelper.SetObjectAsJson(HttpContext.Session, "OrderDetails", OrderDetails);
            menu = SessionHelper.GetObjectFromJson<IList<MenuItem>>(HttpContext.Session, "menu") ?? menu;
            menu = menu.OrderBy(o => o.PROD_PRICE).ToList();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);
        }

        public void OnPostSortByAvailability()
        {
            if (OrderDetails.ContainsKey("Search"))
                OrderDetails = SessionHelper.GetObjectFromJson<Dictionary<string, string>>(HttpContext.Session, "OrderDetails") ?? OrderDetails;
            else
                SessionHelper.SetObjectAsJson(HttpContext.Session, "OrderDetails", OrderDetails);
            menu = SessionHelper.GetObjectFromJson<IList<MenuItem>>(HttpContext.Session, "menu") ?? menu;
            menu = menu.OrderBy(o => o.PROD_QOH).Reverse().ToList();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);
        }





    }
}