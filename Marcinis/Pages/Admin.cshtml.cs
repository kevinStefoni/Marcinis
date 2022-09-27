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

        [BindProperty]
        public IList<Customer> customer { get; set; }

        public void OnGet()
        {
            string sql = "SELECT CustomerId, FirstName, LastName, EmailAddress, PhoneNumber, LoginTypeId FROM Customers";

            DataTable customerDt = DAL.ExecSqlGetDataSet(sql).Tables[0];

            if(customerDt.Rows.Count > 0)
            {
                customer = new List<Customer>();

                foreach(DataRow dr in customerDt.Rows)
                {
                    Customer _cust = new Customer(); // allocate memory for customer
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
    }
}
