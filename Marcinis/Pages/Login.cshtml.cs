using System.Data;
using Marcinis.DAL;
using Marcinis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Marcinis.Helpers;

namespace Marcinis.Pages
{
    public class LoginModel : PageModel
    {
        private readonly MarcinisDAL DAL = new();

        [BindProperty]
        public Customer customer { get; set; } = new Customer();

        public ActionResult OnPostLogin()
        {
            if (!ModelState.IsValid)
                return Page();

            string sql = "uspSelectCustomerByEmailAddress";

            SqlParameter[] spParams =
            {
                new SqlParameter("@EmailAddress", customer.LoginCredentials.EmailAddress)
            };

            DataTable customerDt = DAL.ExecSqlGetDataSet(sql, spParams, CommandType.StoredProcedure).Tables[0];

            if (customerDt.Rows.Count > 0)
            {
                customer.LoginCredentials.Password = customerDt.Rows[0]["Password"].ToString();
                customer.CustomerId = Convert.ToInt32(customerDt.Rows[0]["CustomerId"]);
                customer.FirstName = customerDt.Rows[0]["FirstName"].ToString();
                customer.LastName = customerDt.Rows[0]["LastName"].ToString();
                customer.PhoneNumber = customerDt.Rows[0]["PhoneNumber"].ToString();
                customer.LoginTypeId = Convert.ToInt32(customerDt.Rows[0]["LoginTypeId"]);

                SessionHelper.SetObjectAsJson(HttpContext.Session, "customer", customer);
                return Redirect("./Index");

            }
            return Redirect("./Login");
        }
    }
}
