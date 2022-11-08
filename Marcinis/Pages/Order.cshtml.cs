using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Marcinis.Models;
using Marcinis.DAL;

namespace Marcinis.Pages
{
    public class Order : PageModel
    {

        private readonly OrderDAL DAL = new();

        public int Qty { get; set; }    

        public IList<MenuItem> menu = new List<MenuItem>();
        public IList<string> categories = new List<string>();
        

        public void OnGet()
        {
            // only change if something is returned
            menu = DAL.GetMenu() ?? menu;
            categories = DAL.GetCategories() ?? categories;

            // always have Dessert category at end
            categories = categories.OrderBy(i => i == "Dessert").ThenBy(i => i).ToList();

        }

        public void OnPost()
        {
        
        }
    }
}