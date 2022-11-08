﻿using System.Data;
using Marcinis.DAL;
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
        public MenuItem item { get; set; }

        public void OnGet()
        {
            customer = customerRepo.GetAllCustomers();
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

            itemRepo.AddPicture(item);

            return Redirect("./Admin");
        }
    }
}
