using Course_Project.Processing;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Course_Project.AllWindows
{
    /// <summary>
    /// Логика взаимодействия для UserAdministration.xaml
    /// </summary>
    public partial class UserAdministrationWindow : Window
    {
        public UserAdministrationWindow()
        {
            UsersTableName = "Users";
            WaitersTableName = "Waiters";
            InitializeComponent();

            acceptButton.Click += (s, e) => AceptUsers();
            this.Closing += (s, e) => Window_Closing();
            deliteButton.Click += (s,e) => DeliteUser();

            RefreshData(UsersTableName);
            RefreshData(WaitersTableName);
        }
        string UsersTableName { get; set; }
        string WaitersTableName { get; set; }
        DataSet? UsersData { get; set; }
        DataSet? WaitersData { get; set; }
        SqlDataAdapter? UsersDataAdapter { get; set; }
        SqlDataAdapter? WaitersDataAdapter { get; set; }
        SqlConnection? UsersConnection { get; set; }
        SqlConnection? WaitersConnection { get; set; }

        void RefreshData(string tableName)
        {
            SqlDataAdapter sqlDataAdapter;
            SqlConnection sqlConnection;

            if (tableName == UsersTableName)
            {
                UsersData = SqlProcessing.ShowTable("UsersDB", tableName, out sqlDataAdapter, out sqlConnection);
                UsersDataAdapter = sqlDataAdapter;

                if (UsersConnection != null)
                {
                    UsersConnection.CloseAsync();
                }

                UsersConnection = sqlConnection;
                
                UsersDataGrid.ItemsSource = UsersData.Tables[0].AsDataView();
                UsersDataGrid.DataContext = UsersData.Tables[0];
            }

            if (tableName == WaitersTableName)
            {
                WaitersData = SqlProcessing.ShowTable("UsersDB", tableName, out sqlDataAdapter, out sqlConnection);
                WaitersDataAdapter = sqlDataAdapter;

                if (WaitersConnection != null)
                {
                    WaitersConnection.CloseAsync();
                }
                
                WaitersConnection = sqlConnection;
                WaitersDataGrid.ItemsSource = WaitersData.Tables[0].AsDataView();
                WaitersDataGrid.DataContext = WaitersData.Tables[0];
            }
        }

        void AceptUsers()
        {
            try
            {
                if (WaitersDataGrid.SelectedItems.Count == 1)
                {
                    int selectedIndex = WaitersDataGrid.SelectedIndex;
                    if (WaitersData != null)
                    {
                        var row = WaitersData.Tables[0].Rows[selectedIndex];
                        if (row["Login"] != null && row["Login"] != "Admin")
                        {
                            SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Users] (Name, Surname, Patronymic, Email," +
                                " PhoneNumber, Login, Password, Admin) " +
                        $"VALUES (N'{row["Name"]}', N'{row["Surname"]}', " +
                        $"N'{row["Patronymic"]}', '{row["Email"]}', '{row["PhoneNumber"]}', '{row["Login"]}', CONVERT(varbinary, '{row["Password"]}'), '{row["Admin"]}') ", UsersConnection);

                            sqlCommand.ExecuteNonQuery();
                            row.Delete();
                        }
                        else
                        {
                            MessageBox.Show("Нельзя создать пользователя с таким логином!");
                        }
                    }
                    
                    UpdateDB();

                    RefreshData(UsersTableName);
                    RefreshData(WaitersTableName);
                }
                else if (WaitersDataGrid.SelectedItems.Count > 1)
                {
                    MessageBox.Show("За 1 раз можно принять/удалять только 1 пользователя!");
                }

            }


            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Возникла ошибка! Повторите попытку!");
            }
        }


        void UpdateDB()
        {
            try
            {
                if (UsersData != null && UsersDataAdapter != null)
                {
                    SqlCommandBuilder builder = new SqlCommandBuilder(UsersDataAdapter);
                    UsersDataAdapter.UpdateCommand = builder.GetUpdateCommand();
                    UsersDataAdapter.Update(UsersData);
                }
            }
            catch
            {
                MessageBox.Show("Проверьте, все ли необходимые поля были заполнены");
            }

            try
            {
                if (WaitersData != null && WaitersDataAdapter != null)
                {
                    SqlCommandBuilder builder = new SqlCommandBuilder(WaitersDataAdapter);
                    WaitersDataAdapter.UpdateCommand = builder.GetUpdateCommand();
                    WaitersDataAdapter.Update(WaitersData);
                }
            }
            catch
            {
            }
        }

        void DeliteUser()
        {
            try
            {
                if (WaitersDataGrid.SelectedItems.Count == 1)
                {
                    int selectedIndex = WaitersDataGrid.SelectedIndex;
                    if (WaitersData != null)
                    {
                        var row = WaitersData.Tables[0].Rows[selectedIndex];
                        row.Delete();
                        UpdateDB();

                        RefreshData(WaitersTableName);
                    }
                }
                else if (WaitersDataGrid.SelectedItems.Count > 1)
                {
                    MessageBox.Show("За 1 раз можно удалять только 1 пользователя!");
                }

                if (UsersDataGrid.SelectedItems.Count == 1)
                {
                    int selectedIndex = UsersDataGrid.SelectedIndex;
                    if (UsersData != null)
                    {
                        var row = UsersData.Tables[0].Rows[selectedIndex];
                        row.Delete();

                        UpdateDB();

                        RefreshData(UsersTableName);
                    }
                }
                else if (WaitersDataGrid.SelectedItems.Count > 1)
                {
                    MessageBox.Show("За 1 раз можно удалять только 1 пользователя!");
                }

            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Возникла ошибка! Повторите попытку!");
            }
        }

        void Window_Closing()
        {
            if (UsersConnection != null)
            {
                UsersConnection.CloseAsync();
            }

            if (WaitersConnection != null)
            {
                WaitersConnection.CloseAsync();
            }
        }
    }
}
