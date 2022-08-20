using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace Course_Project.Processing
{
    internal class Authorization
    {
        private string? Login { get; set; }
        private string? Password { get; set; }

        private bool IsFound { get; set; }
        private bool IsAdmin { get; set; }
        private bool IsSuperUser { get; set; }

        internal Authorization()
        {
            IsFound = false;
            IsAdmin = false;
            IsSuperUser = false;
        }


        internal bool TryAuthtorizate(string login, string password, out bool isAdmin, out bool isSuperUser)
        {
            Login = login;
            Password = password;

            Task.WaitAll(Task.Run(FindInUsers), Task.Run(FindInSuperUsers));

            if (!IsFound)
            {
                MessageBox.Show("Пользователь с такими данными не существует. Проверьте правильность введенных данных!");
            }

            isAdmin = IsAdmin;
            isSuperUser = IsSuperUser;

            return IsFound;
        }

        void IsUserInTable(string tableName)
        {
            SqlConnection sqlConnection;
            var isConnected = SqlProcessing.TryOpenConnection("UsersDB", out sqlConnection);
            using (sqlConnection)
            {
                if (isConnected)
                {
                    string aditionalPart = tableName == "Users" ? ", Admin" : "";

                    SqlCommand sqlCommand = new SqlCommand($"SELECT Login, Password{aditionalPart} FROM {tableName}\r\nWhere Login = '{Login}' AND Password = '{Password}'", sqlConnection);
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (tableName == "SuperUsers")
                            {
                                IsSuperUser = true;
                            }
                            else
                            {
                                while (reader.Read())
                                {
                                    IsAdmin = reader["Admin"].ToString() == "False" ? false : true;
                                }
                            }

                            IsFound = true;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Произошла ошибка! Попробуйте войти позже!");
                }

                sqlConnection.CloseAsync();
            }
        }

        void FindInUsers()
        {
            IsUserInTable("Users");
        }

        void FindInSuperUsers()
        {
            IsUserInTable("SuperUsers");
        }
    }
}
