using System.Data.SqlClient;
using System.Configuration;

namespace Course_Project
{
    internal class SqlProcessing
    {
        internal static SqlConnection OpenConnection(string dbName)
        {
            string connection = ConfigurationManager.ConnectionStrings[dbName].ToString();
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}
