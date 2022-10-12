using Microsoft.AspNetCore.Mvc.RazorPages;
using Marcinis.Models;
using Marcinis.DAL;
using Marcinis.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Marcinis.Pages
{
    public class Order : PageModel
    {

        private readonly OrderDAL DAL = new();

        [BindProperty]
        public CustomerOrder CusOrder { get; set; } = new CustomerOrder();

        public IList<MenuItem> menu = new List<MenuItem>();
        public IList<string> categories = new List<string>();
        

        public void OnGet()
        {
            // only change if something is returned
            menu = DAL.GetMenu() ?? menu;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", menu);
            categories = DAL.GetCategories() ?? categories;

            // always have Dessert category at end
            categories = categories.OrderBy(i => i == "Dessert").ThenBy(i => i).ToList();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "categories", categories);

        }

        public void OnPost()
        {
        
        }
    }
}