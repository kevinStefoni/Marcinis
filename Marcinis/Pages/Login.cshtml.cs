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
        public Login login { get; set; }

        public ActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            string sql = "uspSelectCustomerByEmailAddress";

            SqlParameter[] spParams = 
            {
                new SqlParameter("@EmailAddress", login.EmailAddress)
            };

            DataTable customerDt = DAL.ExecSqlGetDataSet(sql, spParams, CommandType.StoredProcedure).Tables[0];

            if (customerDt.Rows.Count > 0)
            {
                string hashToCompare = Utilities.GeneratePasswordHash(Convert.FromBase64String(customerDt.Rows[0]["Salt"].ToString()), login.Password);

                if(hashToCompare.Equals(customerDt.Rows[0]["Password"].ToString()))
                {
                    // we don't want to store plain text password
                    // store the hash we just compared since we know it's correct
                    login.Password = hashToCompare;

                    Customer customer = new()
                    {
                        CustomerId = Convert.ToInt32(customerDt.Rows[0]["CustomerId"]),
                        LoginCredentials = login,
                        FirstName = customerDt.Rows[0]["FirstName"].ToString(),
                        LastName = customerDt.Rows[0]["LastName"].ToString(),
                        PhoneNumber = customerDt.Rows[0]["PhoneNumber"].ToString(),
                        LoginTypeId = Convert.ToInt32(customerDt.Rows[0]["LoginTypeId"])
                    };

                    SessionHelper.SetObjectAsJson(HttpContext.Session, "customer", customer);
                    LayoutModel.customer = customer;
                    return Redirect("./Index");
                }
            }
            return Redirect("./Login");
        }
    }
}
