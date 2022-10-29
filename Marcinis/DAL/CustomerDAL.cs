using System;
using System.Data;
using Marcinis.Enums;
using Marcinis.Models;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace Marcinis.DAL
{
    public class CustomerDAL
    {
        private readonly MarcinisDAL DAL = new();
        private readonly string connStr = "Server=tcp:marcinis-server.database.windows.net,1433;Initial Catalog=MarcinisDB;Persist Security Info=False;User ID=cs3773group12;Password=Pa$$word1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

        public void AddCustomer(Customer customer)
        {
            using (SqlConnection conn = new(connStr))
            {
                SqlCommand cmd = new("uspInsertCustomer", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // guest logintypes will not have passwords
                if (customer.LoginTypeId != (int)Enums.LoginType.Guest)
                    cmd.Parameters.AddWithValue("@Password", customer.LoginCredentials.Password);

                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                cmd.Parameters.AddWithValue("@EmailAddress", customer.LoginCredentials.EmailAddress);
                cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                cmd.Parameters.AddWithValue("@LoginTypeId", customer.LoginTypeId);
                cmd.Parameters.AddWithValue("@Salt", customer.Salt);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{System.DateTime.Now.ToString()} - AddCustomer() - ex:" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            }
        }

        public IList<Customer> GetAllCustomers()
        {
            IList<Customer> customers = new List<Customer>();

            string customerSql = "SELECT CustomerId, FirstName, LastName, EmailAddress, PhoneNumber, LoginTypeId FROM Customers";

            DataTable customerDt = DAL.ExecSqlGetDataSet(customerSql).Tables[0];

            if (customerDt.Rows.Count > 0)
            {
                foreach (DataRow dr in customerDt.Rows)
                {
                    Customer _cust = new()
                    {
                        LoginCredentials = new Login(),
                        CustomerId = Convert.ToInt32(dr["CustomerId"]),
                        FirstName = dr["FirstName"].ToString() ?? string.Empty,
                        LastName = dr["LastName"].ToString() ?? string.Empty,
                        PhoneNumber = dr["PhoneNumber"].ToString() ?? string.Empty,
                        LoginTypeId = Convert.ToInt32(dr["LoginTypeId"]),
                        
                    };
                    _cust.LoginCredentials.EmailAddress = dr["EmailAddress"].ToString() ?? string.Empty;
                    _cust.LoginCredentials.Password = string.Empty;

                    customers.Add(_cust);
                }
            }

            return customers;
        }

        public Customer GetCustomer(string email)
        {
            string sql = "uspSelectCustomerByEmailAddress";

            SqlParameter[] spParams =
            {
                new SqlParameter("@EmailAddress", email)
            };

            DataTable customerDt = DAL.ExecSqlGetDataSet(sql, spParams, CommandType.StoredProcedure).Tables[0];

            if (customerDt.Rows.Count > 0)
            {
                Login login = new()
                {
                    EmailAddress = email,
                    Password = customerDt.Rows[0]["Password"].ToString()
                };
                Customer Customer = new()
                {
                    LoginCredentials = login,
                    CustomerId = Convert.ToInt32(customerDt.Rows[0]["CustomerId"]),
                    FirstName = customerDt.Rows[0]["FirstName"].ToString(),
                    LastName = customerDt.Rows[0]["LastName"].ToString(),
                    PhoneNumber = customerDt.Rows[0]["PhoneNumber"].ToString(),
                    LoginTypeId = Convert.ToInt32(customerDt.Rows[0]["LoginTypeId"])
                };

                return Customer;
                
            }

            return null; 

        }

        public bool EmailExists(string email)
        {
            string sql = $"SELECT EmailAddress FROM Customers WHERE EmailAddress='{email}'";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    if ((string)cmd.ExecuteScalar() != null)
                        return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return false;
            }
        }

        public bool PasswordExists(string email, string password)
        {
            string getpassword = $"SELECT Password FROM Customers WHERE EmailAddress='{email}'";
            string getsalt = $"SELECT Salt FROM Customers WHERE EmailAddress='{email}'";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd1 = new SqlCommand(getsalt, conn);
                SqlCommand cmd2 = new SqlCommand(getpassword, conn);
                try
                {
                    conn.Open();
                    string hashToCompare;
                    string? salt = cmd1?.ExecuteScalar()?.ToString();
                    if (salt != null)
                    {
                        hashToCompare = Utilities.GeneratePasswordHash(Convert.FromBase64String(salt), password);

                        if (hashToCompare.Equals(cmd2?.ExecuteScalar()?.ToString()))
                            return true;

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return false;
            }
        }

    }
}
