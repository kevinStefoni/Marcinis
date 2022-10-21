using System.Data;
using Marcinis.Models;
using Marcinis.DAL;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text;

namespace Marcinis.DAL
{
    public class OrderDAL
    {
        private readonly MarcinisDAL DAL = new();

        public IList<MenuItem> GetMenuItemsByCategory(string category)
        {
            IList<MenuItem> items = new List<MenuItem>(); // create a list of MenuItems

            // use StringBuilder to efficiently make string
            StringBuilder sb = new StringBuilder();

            // select all the items from the table for the given category
            string sql = "SELECT * FROM Inventory WHERE PROD_CATEGORY='";
            sb.Append(sql);
            sb.Append(category);
            sb.Append('\'');

            // retrieve the result set
            DataSet inventoryDs = DAL.ExecSqlGetDataSet(sb.ToString());

            // avoid empty data exception--null handled in calling function
            if (inventoryDs.Tables[0].Rows.Count == 0)
                return null;

            DataTable inventoryDt = inventoryDs.Tables[0];

            // if there is any data to add, add it
            if (inventoryDt.Rows.Count > 0)
            {
                // for every item, create a new MenuItem, initialzie its fields and then add it to the list
                foreach (DataRow dr in inventoryDt.Rows)
                {
                    MenuItem _mi = new MenuItem(); // allocate memory for customer
                    _mi.PROD_NAME = dr["PROD_NAME"].ToString();
                    _mi.PROD_DESC = dr["PROD_DESC"].ToString();
                    _mi.PROD_TYPE = dr["PROD_TYPE"].ToString();
                    _mi.PROD_PRICE = Convert.ToDecimal(dr["PROD_PRICE"]);
                    _mi.PROD_QOH = Convert.ToInt32(dr["PROD_QOH"]);
                    _mi.PROD_CATEGORY = dr["PROD_CATEGORY"].ToString();

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
            if (inventoryDs.Tables == null || inventoryDs.Tables[0].Rows.Count == 0)
                return null;

            DataTable inventoryDt = inventoryDs.Tables[0];

            // if there is any data to add, add it
            if (inventoryDt.Rows.Count > 0)
            {
                // for every appetizer, create a new MenuItem, initialzie its fields and then add it to the list
                foreach (DataRow dr in inventoryDt.Rows)
                {
                    MenuItem _mi = new MenuItem(); // allocate memory for customer
                    _mi.PROD_NAME = dr["PROD_NAME"].ToString();
                    _mi.PROD_DESC = dr["PROD_DESC"].ToString();
                    _mi.PROD_TYPE = dr["PROD_TYPE"].ToString();
                    _mi.PROD_PRICE = Convert.ToDecimal(dr["PROD_PRICE"]);
                    _mi.PROD_QOH = Convert.ToInt32(dr["PROD_QOH"]);
                    _mi.PROD_CATEGORY = dr["PROD_CATEGORY"].ToString();
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


    }
}
