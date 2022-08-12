using System.Data.SqlClient;
using System.Configuration;

namespace Course_Project.Processing
{
    internal class SqlProcessing
    {
        internal static bool TryOpenConnection(string dbName, out SqlConnection sqlConnection)
        {
            string connection = ConfigurationManager.ConnectionStrings[dbName].ToString();
            sqlConnection = new SqlConnection(connection);

            sqlConnection.Open();
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                return true;
            }

            sqlConnection.CloseAsync();
            return false;
        }
    }
}
