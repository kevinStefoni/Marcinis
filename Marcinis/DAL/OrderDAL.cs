using System.Data;
using Marcinis.Models;
using Marcinis.DAL;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;
using Marcinis.Helpers;

namespace Marcinis.DAL
{
    public class OrderDAL
    {
        private readonly MarcinisDAL DAL = new();
        private readonly string connStr = "Server=tcp:marcinis-server.database.windows.net,1433;Initial Catalog=MarcinisDB;Persist Security Info=False;User ID=cs3773group12;Password=Pa$$word1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

        public IList<MenuItem> SearchMenu(string searchTerm)
        {
            IList<MenuItem> items = new List<MenuItem>(); // create a list of MenuItems

            // select all the appetizers from the table
            string sql = $"SELECT DISTINCT * FROM Inventory WHERE UPPER(PROD_NAME) LIKE UPPER('%{searchTerm}%') OR " +
                $"UPPER(PROD_DESC) LIKE UPPER('%{searchTerm}%') OR " +
                $"UPPER(PROD_TYPE) LIKE UPPER('%{searchTerm}%') OR " +
                $"UPPER(PROD_CATEGORY) LIKE UPPER('%{searchTerm}%');";


            // retrieve the result set
            DataSet inventoryDs = DAL.ExecSqlGetDataSet(sql);

            // avoid empty data exception--null handled in calling function
            try
            {
                if (inventoryDs.Tables[0].Rows.Count == 0)
                    return null;
            }
            catch (IndexOutOfRangeException)
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
                    _mi.PROD_ID = Convert.ToInt32(dr["PROD_ID"]);
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
            IList<string> categories = new List<string>(); // create a list of categories

            // select all categories in the DB
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
                    Console.WriteLine($"{System.DateTime.Now} - AddItem() - ex:" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            }
        }

        public void AddPicture(MenuItem item)
        {
            using (SqlConnection conn = new(connStr))
            {
                SqlCommand cmd = new("UPDATE Inventory SET PROD_IMG=@PROD_IMG WHERE PROD_NAME=@PROD_NAME", conn);
                cmd.Parameters.AddWithValue("@PROD_IMG", item.PROD_IMG);
                cmd.Parameters.AddWithValue("@PROD_NAME", item.PROD_NAME);


                try
                {
                    conn.Open();
                    cmd.ExecuteScalar();
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

        public void DeleteMenuItem(int id)
        {
            string sql = "uspDeleteMenuItemByProdId";

            SqlParameter[] spParams =
            {
                new SqlParameter("@PROD_ID", id)
            };

            DataSet ds = DAL.ExecSqlGetDataSet(sql, spParams, CommandType.StoredProcedure);

            Console.WriteLine(ds);

        }

        public IList<CustomerOrder> GetOrders()
        {
            IList<CustomerOrder> orders = new List<CustomerOrder>(); // create a list of orders

            // select all the attributes from the table
            string sql = "SELECT * FROM Orders";


            // retrieve the result set
            DataSet ordersDs = DAL.ExecSqlGetDataSet(sql);

            // avoid empty data exception--null handled in calling function
            try
            {
                if (ordersDs.Tables[0].Rows.Count == 0)
                    return null;
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }

            DataTable ordersDt = ordersDs.Tables[0];

            // if there is any data to add, add it
            if (ordersDt.Rows.Count > 0)
            {
                // for every order, create a new CustomerOrder, initialzie its fields and then add it to the list
                foreach (DataRow dr in ordersDt.Rows)
                {
                    CustomerOrder _co = new CustomerOrder(); // allocate memory for CustomerOrder
                    _co.ORDER_ID = Convert.ToInt32(dr["ORDER_ID"]);
                    _co.ORDER_CUST_ID = Convert.ToInt32(dr["ORDER_CUST_ID"]);
                    _co.ORDER_CREDIT_CARD_NUM = dr["ORDER_CREDIT_CARD_NUMBER"].ToString();
                    _co.ORDER_CREDIT_CARD_CVV = dr["ORDER_CREDIT_CARD_CVV"].ToString();
                    _co.ORDER_CREDIT_CARD_EXP_MONTH = Convert.ToInt32(dr["ORDER_CREDIT_CARD_EXP_MONTH"]);
                    _co.ORDER_CREDIT_CARD_EXP_YEAR = Convert.ToInt32(dr["ORDER_CREDIT_CARD_EXP_YEAR"]);
                    _co.ORDER_CREDIT_CARD_ZIP_CODE = dr["ORDER_CREDIT_CARD_ZIP_CODE"].ToString();
                    _co.ORDER_PICKUP_TIME = Convert.ToDateTime(dr["ORDER_PICKUP_TIME"]);
                    _co.ORDER_SUBTOTAL = Convert.ToDecimal(dr["ORDER_SUBTOTAL"]);
                    _co.ORDER_TAX = Convert.ToInt32(dr["ORDER_TAX"]);
                    _co.ORDER_TOTAL = Convert.ToDecimal(dr["ORDER_TOTAL"]);
                    _co.ORDER_DATE = Convert.ToDateTime(dr["ORDER_DATE"]);

                    orders.Add(_co);
                }
            }

            return orders;
        }



        public IList<DiscountCode> GetDiscountCodes()
        {
            IList<DiscountCode> discounts = new List<DiscountCode>(); // create a list of discount codes

            // select all the attributes from the table
            string sql = "SELECT * FROM DiscountCodes";


            // retrieve the result set
            DataSet discountsDs = DAL.ExecSqlGetDataSet(sql);

            // avoid empty data exception--null handled in calling function
            try
            {
                if (discountsDs.Tables[0].Rows.Count == 0)
                    return null;
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }

            DataTable discountsDt = discountsDs.Tables[0];

            // if there is any data to add, add it
            if (discountsDt.Rows.Count > 0)
            {
                // for every order, create a new CustomerOrder, initialzie its fields and then add it to the list
                foreach (DataRow dr in discountsDt.Rows)
                {
                    DiscountCode _dc = new DiscountCode(); // allocate memory for CustomerOrder
                    _dc.DiscountId = Convert.ToInt32(dr["DiscountId"]);
                    _dc.Code = dr["Code"].ToString();
                    _dc.Percentage = Convert.ToInt32(dr["Percentage"]);
                    _dc.StartDate = (DateTime)dr["StartDate"];
                    _dc.EndDate = (DateTime)dr["EndDate"];

                    discounts.Add(_dc);
                }
            }

            return discounts;
        }

        public void DeleteDiscountCode(int id)
        {
            string sql = "uspDeleteDiscountCodeById";

            SqlParameter[] spParams =
            {
                new SqlParameter("@DiscountId", id)
            };

            DataSet ds = DAL.ExecSqlGetDataSet(sql, spParams, CommandType.StoredProcedure);

        }


        public void DeleteOrder(int id)
        {
            string sql = "uspDeleteOrderById";

            SqlParameter[] spParams =
            {
                new SqlParameter("@ORDER_ID", id)
            };

            DataSet ds = DAL.ExecSqlGetDataSet(sql, spParams, CommandType.StoredProcedure);

        }


        public void AddDiscountCode(DiscountCode discount)
        {
            using (SqlConnection conn = new(connStr))
            {
                SqlCommand cmd = new("uspInsertDiscountcode", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Code", discount.Code);
                cmd.Parameters.AddWithValue("@Percentage", discount.Percentage);
                cmd.Parameters.AddWithValue("@StartDate", discount.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", discount.EndDate);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{System.DateTime.Now.ToString()} - AddDiscountCode() - ex:" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            }
        }

        public void UpdateMenuItem(MenuItem item)
        {
            string sql = "uspUpdateMenuItem";

            SqlParameter[] spParams =
            {
                new SqlParameter("@PROD_ID", item.PROD_ID),
                new SqlParameter("@PROD_NAME", item.PROD_NAME),
                new SqlParameter("@PROD_DESC", item.PROD_DESC),
                new SqlParameter("@PROD_TYPE", item.PROD_TYPE),
                new SqlParameter("@PROD_PRICE", item.PROD_PRICE),
                new SqlParameter("@PROD_QOH", item.PROD_QOH),
                new SqlParameter("@PROD_CATEGORY", item.PROD_CATEGORY),
                new SqlParameter("@PROD_IMG", item.PROD_IMG)
            };

            DataSet ds = DAL.ExecSqlGetDataSet(sql, spParams, CommandType.StoredProcedure);

            Console.WriteLine(ds);
        }

        public void AddOrder(CustomerOrder custOrder)
        {
            string sql = "uspInsertOrder";

            SqlParameter[] spParams =
            {
                new SqlParameter("@ORDER_CUST_ID", custOrder.ORDER_CUST_ID),
                new SqlParameter("@ORDER_CREDIT_CARD_NUMBER", custOrder.ORDER_CREDIT_CARD_NUM),
                new SqlParameter("@ORDER_CREDIT_CARD_CVV", custOrder.ORDER_CREDIT_CARD_CVV),
                new SqlParameter("@ORDER_CREDIT_CARD_ZIP_CODE", custOrder.ORDER_CREDIT_CARD_ZIP_CODE),
                new SqlParameter("@ORDER_CREDIT_CARD_EXP_MONTH", custOrder.ORDER_CREDIT_CARD_EXP_MONTH),
                new SqlParameter("@ORDER_CREDIT_CARD_EXP_YEAR", custOrder.ORDER_CREDIT_CARD_EXP_YEAR),
                new SqlParameter("@ORDER_PICKUP_TIME", custOrder.ORDER_PICKUP_TIME),
                new SqlParameter("@ORDER_SUBTOTAL", custOrder.ORDER_SUBTOTAL),
                new SqlParameter("@ORDER_TAX", custOrder.ORDER_TAX),
                new SqlParameter("@ORDER_TOTAL", custOrder.ORDER_TOTAL),
                new SqlParameter("@ORDER_DATE", custOrder.ORDER_DATE)
            };

            DataTable dt = DAL.ExecSqlGetDataSet(sql, spParams, CommandType.StoredProcedure).Tables[0];
            DataRow dr = dt.Rows[0];
            custOrder.ORDER_ID = Convert.ToInt32(dr.ItemArray[0]);
            AddOrderItems(custOrder);
        }

        public void AddOrderItems(CustomerOrder custOrder)
        {
            foreach (var item in custOrder.ORDER_ITEMS)
            {
                string sql = $"SELECT PROD_ID FROM Inventory WHERE PROD_NAME = '{item.Key}'";
                DataTable dt = DAL.ExecSqlGetDataSet(sql).Tables[0];
                DataRow dr = dt.Rows[0];

                sql = "uspInsertItemInOrder";

                SqlParameter[] spParams =
                {
                    new SqlParameter("@ORDER_ID", custOrder.ORDER_ID),
                    new SqlParameter("@PROD_ID", Convert.ToInt32(dr.ItemArray[0])),
                    new SqlParameter("@QTY", Convert.ToInt32(item.Value))
                };

                DataSet ds = DAL.ExecSqlGetDataSet(sql, spParams, CommandType.StoredProcedure);
            }
        }

    }
}
