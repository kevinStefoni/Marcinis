using Microsoft.AspNetCore.Mvc.RazorPages;
using Marcinis.Models;
using Marcinis.DAL;
using Marcinis.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marcinis.Pages
{
    public class Order : PageModel
    {

        private readonly OrderDAL DAL = new();

        [BindProperty]
        public Dictionary<string, string> OrderDetails { get; set; } = new Dictionary<string, string>();

        [BindProperty]
        public string? Search { get; set; }

        public IList<MenuItem> menu = new List<MenuItem>();
        public IList<string> categories = new List<string>();

        public Dictionary<string, IList<SelectListItem>> AllAvailableQuantities { get; set; } = new Dictionary<string, IList<SelectListItem>>();
        public void OnGet()
        {
            ViewData["DISPLAY"] = "POST";

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

            GetQuantities();
        }

        public void OnPost()
        {
            if (OrderDetails.ContainsKey(Search ?? "Search"))
                OrderDetails = SessionHelper.GetObjectFromJson<Dictionary<string, string>>(HttpContext.Session, "OrderDetails") ?? OrderDetails;
            else
                SessionHelper.SetObjectAsJson(HttpContext.Session, "OrderDetails", OrderDetails);

            ViewData["DISPLAY"] = "POST";

            // only change if something is returned from DAL
            menu = DAL.GetMenu() ?? menu;

            // save the menu for use in the frontend, when the page has to be remade after POST
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);

            GetQuantities();
        }

        public void OnPostSearch()
        {
            ViewData["DISPLAY"] = "SEARCH";

            if (OrderDetails.ContainsKey("Search") || OrderDetails.ContainsValue(Search ?? "Search"))
                OrderDetails = SessionHelper.GetObjectFromJson<Dictionary<string, string>>(HttpContext.Session, "OrderDetails") ?? OrderDetails;
            else
            {
                Dictionary<string, string> OldOrderDetails = SessionHelper.GetObjectFromJson<Dictionary<string, string>>(HttpContext.Session, "OrderDetails") ?? OrderDetails;
                foreach (string item in OrderDetails.Keys)
                {
                    if (!OrderDetails[item].Equals("0"))
                    {
                        OldOrderDetails[item] = OrderDetails[item];

                    }
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "OrderDetails", OldOrderDetails);
            }

            menu = DAL.SearchMenu(Search ?? string.Empty) ?? menu;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);

            categories.Clear();
            foreach(MenuItem item in menu)
                if(item.PROD_CATEGORY != null && !categories.Contains(item.PROD_CATEGORY))
                    categories.Add(item.PROD_CATEGORY);

            SessionHelper.SetObjectAsJson(HttpContext.Session, "categories", categories);

            GetQuantities();
        }

        public void OnPostSortByPrice()
        {
            ViewData["DISPLAY"] = "PRICE";

            if (OrderDetails.ContainsKey(Search ?? "Search"))
                OrderDetails = SessionHelper.GetObjectFromJson<Dictionary<string, string>>(HttpContext.Session, "OrderDetails") ?? OrderDetails;
            else
                SessionHelper.SetObjectAsJson(HttpContext.Session, "OrderDetails", OrderDetails);
            menu = SessionHelper.GetObjectFromJson<IList<MenuItem>>(HttpContext.Session, "menu") ?? menu;
            menu = menu.OrderBy(o => o.PROD_PRICE).ToList();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);

            GetQuantities();
        }

        public void OnPostSortByAvailability()
        {
            ViewData["DISPLAY"] = "AVAILABILITY";

            if (OrderDetails.ContainsKey(Search ?? "Search"))
                OrderDetails = SessionHelper.GetObjectFromJson<Dictionary<string, string>>(HttpContext.Session, "OrderDetails") ?? OrderDetails;
            else
                SessionHelper.SetObjectAsJson(HttpContext.Session, "OrderDetails", OrderDetails);
            menu = SessionHelper.GetObjectFromJson<IList<MenuItem>>(HttpContext.Session, "menu") ?? menu;
            menu = menu.OrderBy(o => -1 * o.PROD_QOH).ToList();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);

            GetQuantities();
        }


        public void GetQuantities()
        {
            IList<SelectListItem> AvailableQuantities;

            AllAvailableQuantities.Clear();
            foreach (MenuItem m in menu)
            {
                AvailableQuantities = new List<SelectListItem>();
                for (int i = 0; i <= m.PROD_QOH && i <= 5; i++)
                    AvailableQuantities.Add(new SelectListItem(i.ToString(), i.ToString()));
                if(m.PROD_NAME != null)
                    AllAvailableQuantities.Add(m.PROD_NAME, AvailableQuantities);
            }
        }


    }
}