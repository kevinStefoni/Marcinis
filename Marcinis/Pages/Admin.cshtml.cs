using System.Data;
using Marcinis.DAL;
using Marcinis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Marcinis.Pages
{
    public class AdminModel : PageModel
    {
        private readonly MarcinisDAL DAL = new();
        private readonly OrderDAL itemRepo = new();

        [BindProperty]
        public IList<Customer> customer { get; set; }

        [BindProperty]
        public MenuItem item { get; set; }

        public void OnGet()
        {
            string customerSql = "SELECT CustomerId, FirstName, LastName, EmailAddress, PhoneNumber, LoginTypeId FROM Customers";

            DataTable customerDt = DAL.ExecSqlGetDataSet(customerSql).Tables[0];

            if(customerDt.Rows.Count > 0)
            {
                customer = new List<Customer>();

                foreach(DataRow dr in customerDt.Rows)
                {
                    Customer _cust = new Customer(); // allocate memory for Customer
                    _cust.LoginCredentials = new Login(); // have to allocate memory for new Login object
                    _cust.CustomerId = Convert.ToInt32(dr["CustomerId"]);
                    _cust.FirstName = dr["FirstName"].ToString();
                    _cust.LastName = dr["LastName"].ToString();
                    _cust.LoginCredentials.EmailAddress = dr["EmailAddress"].ToString();
                    _cust.PhoneNumber = dr["PhoneNumber"].ToString();
                    _cust.LoginTypeId = Convert.ToInt32(dr["LoginTypeId"]);
                    _cust.LoginCredentials.Password = string.Empty;
                    customer.Add(_cust);
                }
            }
        }

        public ActionResult OnPost()
        {
            // handles the conversion of the image to base64
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
                //item.PROD_IMG = Convert.ToBase64String(bytes, 0, bytes.Length);
                item.PROD_IMG = bytes;
            }

            itemRepo.AddMenuItem(item);

            return Redirect("./Admin");
        }
    }
}
