using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System;
using System.Windows;
using System.Collections.Generic;

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
            out SqlConnection sqlConnection, string? groupByCommand = null, string? joinCommand = null)
        {
            bool isConnected = TryOpenConnection(dbName, out sqlConnection);

            if (isConnected)
            {
                DataSet data = new DataSet();
                if (groupByCommand == null)
                {
                    if (joinCommand == null) // Обычный вывод таблицы
                    {
                        sqlDataAdapter = new SqlDataAdapter(
                            $"SELECT * FROM {tableName}", sqlConnection);

                        sqlDataAdapter.Fill(data);
                        return data;
                    }
                    else // Для InnerJoin
                    {
                        sqlDataAdapter = new SqlDataAdapter(
                           joinCommand, sqlConnection);

                        try
                        {
                            sqlDataAdapter.Fill(data);
                            return data;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        return null;
                    }
                }
                else // Для GroupBy
                {

                    sqlDataAdapter = new SqlDataAdapter(
                        groupByCommand, sqlConnection);
                    try
                    {
                        sqlDataAdapter.Fill(data);
                        return data;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(groupByCommand);
                        MessageBox.Show(ex.Message);
                    }

                    return null;
                }
            }
            else
            {
                sqlDataAdapter = null;
                return null;
            }
        }

        internal static List<string> ExecuteCommand(string Command, bool isToRead = false)
        {
            SqlConnection sqlConnection;

            try
            {
                TryOpenConnection("AuctionsDB", out sqlConnection);
                SqlCommand sqlCommand = new SqlCommand(Command, sqlConnection);
                if (!isToRead)
                {
                    sqlCommand.ExecuteNonQuery();
                }
                else
                {
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<string> readerList = new List<string>();
                    while (reader.Read())
                    {
                        readerList.Add(reader.GetString(0));
                    }
                    return readerList;
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nПроверьте, все ли необходимые поля заполнены");
            }

            return null;
        }
    }


}
