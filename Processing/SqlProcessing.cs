using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System;

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

        internal static DataSet ShowTable(string dbName, string tableName, out SqlDataAdapter sqlDataAdapter, 
            out SqlConnection sqlConnection)
        {
            bool isConnected = TryOpenConnection(dbName, out sqlConnection);

            if (isConnected)
            {
                sqlDataAdapter = new SqlDataAdapter(
                    $"SELECT * FROM {tableName}", sqlConnection);

                DataSet data = new DataSet();

                sqlDataAdapter.Fill(data);

                return data;
            }
            else
            {
                sqlDataAdapter = null;
                return null;
            }
        }
    }


}
