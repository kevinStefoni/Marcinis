using System.Data;
using Marcinis.Models;
using Marcinis.DAL;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Marcinis.DAL
{
    public class OrderDAL
    {
        private readonly MarcinisDAL DAL = new();

        public IList<MenuItem> GetAllAppetizers()
        {
            IList<MenuItem> appetizers = new List<MenuItem>(); // create a list of MenuItems for appetizers

            // select all the appetizers from the table
            string sql = "SELECT * FROM Inventory WHERE PROD_CATEGORY='Appetizers'";

            // retrieve the result set
            DataSet inventoryDs = DAL.ExecSqlGetDataSet(sql);

            if (inventoryDs.Tables[0].Rows.Count == 0)
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

                    appetizers.Add(_mi);
                }
            }

            return appetizers;
        }


    }
}
