using Marcinis.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marcinis.Models
{
    public class LayoutModel:PageModel
    {
        public static Customer? customer { get; set; }
        
    }
}
