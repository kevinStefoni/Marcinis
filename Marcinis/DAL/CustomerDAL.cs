using System;
using System.Data;
using Marcinis.Enums;
using Marcinis.Models;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;


namespace Marcinis.DAL
{
    public class CustomerDAL
    {
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

        public void GetAllCustomers()
        {
            List<Customer> customerList = new();

            string sql = "SELECT CustomerId, Password, FirstName, LastName, EmailAddress, PhoneNumber, LoginTypeId FROM Customers";

            using (SqlConnection conn = new(connStr))
            {
                SqlCommand cmd = new(sql, conn);

                try
                {
                    conn.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        Console.WriteLine("{0}\t{1}\t{2}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


    }
}
