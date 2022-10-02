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

        public IList<MenuItem> appetizers = new List<MenuItem>(); 

        public void OnGet()
        {
            // only change appetizers if something is returned
            appetizers = DAL.GetAllAppetizers() ?? appetizers;
        }

        public void OnPost()
        {
        
        }
    }
}