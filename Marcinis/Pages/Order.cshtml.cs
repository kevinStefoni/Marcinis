using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Marcinis.Pages
{
    public class Order : PageModel
    {

        public int Qty { get; set; }   

        public void OnGet()
        {

        }

        public void OnPost()
        {
        
        }
    }
}