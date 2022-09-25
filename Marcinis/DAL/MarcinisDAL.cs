using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Marcinis.DAL
{
    public class MarcinisDAL
    {
        private static readonly string connStr = "Server=tcp:marcinis-server.database.windows.net,1433;Initial Catalog=MarcinisDB;Persist Security Info=False;User ID=cs3773group12;Password=Pa$$word1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
        private static readonly SqlConnection conn = new(connStr);

        public DataSet ExecSqlGetDataSet(string sql, SqlParameter[]? parameters = null, CommandType cmdType = CommandType.Text)
        {
            DataSet ds = new();

            using (SqlCommand sqlCmd = new(sql, conn))
            {
                sqlCmd.CommandType = cmdType;

                if (parameters != null)
                    sqlCmd.Parameters.AddRange(parameters);

                try
                {
                    conn.Open();

                    using (SqlDataAdapter sda = new())
                    {
                        sda.SelectCommand = sqlCmd;
                        sda.Fill(ds);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ExecSQLGetDataSet(sql: {sql}) - {ex.Message}");
                }
                finally { conn.Close(); }
            }
            return ds;
        }
    }
}
