using System.Data;
using Marcinis.DAL;
using Marcinis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Marcinis.Pages
{
    public class LoginModel : PageModel
    {
        private readonly MarcinisDAL DAL = new();

        [BindProperty]
        public Customer customer { get; set; }

        public ActionResult OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            string sql = "uspSelectCustomerByEmailAddress";

            SqlParameter[] spParams = 
            {
                new SqlParameter("@EmailAddress", customer.EmailAddress)
            };

            DataTable customerDt = DAL.ExecSqlGetDataSet(sql, spParams, CommandType.StoredProcedure).Tables[0];

            if (customerDt.Rows.Count > 0)
            {
                string hashToCompare = Utilities.GeneratePasswordHash(Convert.FromBase64String(customerDt.Rows[0]["Salt"].ToString()), customer.Password);

                if(hashToCompare.Equals(customerDt.Rows[0]["Password"].ToString()))
                {
                    return Redirect("./Index");
                }
            }
            return RedirectToPage("./Login");
        }
    }
}
