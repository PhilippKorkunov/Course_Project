using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace Course_Project
{
    internal class Authorization
    {
        internal static bool AuthorizationCheck(string login, string password)
        {
            var sqlConnection = SqlProcessing.OpenConnection("UserDB");
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand($"SELECT Login, Password FROM UsersDB WHERE Login IN('{login}')", sqlConnection);
            var userArray = sqlCommand.ExecuteReader();


            return true;
        }
    }
}
