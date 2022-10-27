using System.Data;
using Marcinis.Models;
using Marcinis.DAL;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;

namespace Marcinis.DAL
{
    public class OrderDAL
    {
        private readonly MarcinisDAL DAL = new();
        private readonly string connStr = "Server=tcp:marcinis-server.database.windows.net,1433;Initial Catalog=MarcinisDB;Persist Security Info=False;User ID=cs3773group12;Password=Pa$$word1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";


        public IList<MenuItem> GetMenu()
        {
            IList<MenuItem> items = new List<MenuItem>(); // create a list of MenuItems

            // select all the appetizers from the table
            string sql = "SELECT * FROM Inventory";
            

            // retrieve the result set
            DataSet inventoryDs = DAL.ExecSqlGetDataSet(sql);

            // avoid empty data exception--null handled in calling function
            try
            {
                if(inventoryDs.Tables[0].Rows.Count == 0)
                    return null;
            }
            catch(IndexOutOfRangeException)
            {
                return null;
            }

            DataTable inventoryDt = inventoryDs.Tables[0];

            // if there is any data to add, add it
            if (inventoryDt.Rows.Count > 0)
            {
                // for every appetizer, create a new MenuItem, initialzie its fields and then add it to the list
                foreach (DataRow dr in inventoryDt.Rows)
                {
                    MenuItem _mi = new MenuItem(); // allocate memory for MenuItem
                    _mi.PROD_NAME = dr["PROD_NAME"].ToString();
                    _mi.PROD_DESC = dr["PROD_DESC"].ToString();
                    _mi.PROD_TYPE = dr["PROD_TYPE"].ToString();
                    _mi.PROD_PRICE = Convert.ToDecimal(dr["PROD_PRICE"]);
                    _mi.PROD_QOH = Convert.ToInt32(dr["PROD_QOH"]);
                    _mi.PROD_CATEGORY = dr["PROD_CATEGORY"].ToString();
                    if (dr["PROD_IMG"] != null)
                    {
                        _mi.PROD_IMG = dr["PROD_IMG"] as byte[];
                    }
                    
                    items.Add(_mi);
                }
            }

            return items;
        }

        public IList<string> GetCategories()
        {
            IList<string> categories = new List<string>(); // create a list of MenuItems

            // select all the appetizers from the table
            string sql = "SELECT Inventory.PROD_CATEGORY FROM Inventory GROUP BY PROD_CATEGORY";


            // retrieve the result set
            DataSet inventoryDs = DAL.ExecSqlGetDataSet(sql);

            // avoid empty data exception--null handled in calling function
            if (inventoryDs.Tables[0].Rows.Count == 0)
                return null;

            DataTable inventoryDt = inventoryDs.Tables[0];

            // if there is any data to add, add it
            if (inventoryDt.Rows.Count > 0)
            {
                // for every appetizer, create a new MenuItem, initialzie its fields and then add it to the list
                foreach (DataRow dr in inventoryDt.Rows)
                {
                    categories.Add(dr["PROD_CATEGORY"].ToString());
                }
            }

            return categories;
        }


        public void AddMenuItem(MenuItem item)
        {
            using (SqlConnection conn = new(connStr))
            {
                SqlCommand cmd = new("uspInsertMenuItem", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PROD_ID", item.PROD_ID);
                cmd.Parameters.AddWithValue("@PROD_NAME", item.PROD_NAME);
                cmd.Parameters.AddWithValue("@PROD_DESC", item.PROD_DESC);
                cmd.Parameters.AddWithValue("@PROD_TYPE", item.PROD_TYPE);
                cmd.Parameters.AddWithValue("@PROD_PRICE", item.PROD_PRICE);
                cmd.Parameters.AddWithValue("@PROD_QOH", item.PROD_QOH);
                cmd.Parameters.AddWithValue("@PROD_CATEGORY", item.PROD_CATEGORY);
                cmd.Parameters.AddWithValue("@PROD_IMG", item.PROD_IMG);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{System.DateTime.Now.ToString()} - AddItem() - ex:" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            }
        }


    }
}
