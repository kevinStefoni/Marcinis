using Microsoft.AspNetCore.Mvc.RazorPages;
using Marcinis.Models;
using Marcinis.DAL;
using Marcinis.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Net;

namespace Marcinis.Pages
{
    public class Order : PageModel
    {

        private readonly OrderDAL DAL = new();

        [BindProperty]
        public CustomerOrder CusOrder { get; set; } = new CustomerOrder();

        [BindProperty]
        public Dictionary<string, string> OrderDetails { get; set; } = new Dictionary<string, string>();

        public IList<MenuItem> menu = new List<MenuItem>();
        public IList<string> categories = new List<string>();
        

        public void OnGet()
        {
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
            // copy the data from OrderDetails to CusOrder.ORDER_ITEMS
            CusOrder.ORDER_ITEMS = OrderDetails;

            // save the CusOrder in the session
            SessionHelper.SetObjectAsJson(HttpContext.Session, "CusOrder", CusOrder);

            // save the OrderDetails in the session too
            SessionHelper.SetObjectAsJson(HttpContext.Session, "OrderDetails", OrderDetails);
        }



    }
}