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
        internal static bool TryAuthtorizate(string login, string password)
        {
            SqlConnection sqlConnection;
            var isConnected = SqlProcessing.TryOpenConnection("UserDB", out sqlConnection);

            if (isConnected)
            {
                SqlCommand sqlCommand = new SqlCommand($"SELECT Login, Password FROM UsersDB WHERE Login IN('{login}')", sqlConnection);
                var userArray = sqlCommand.ExecuteReader();
            }

            return true;
        }
    }
}
