using Course_Project.Processing;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Course_Project.AllWindows
{
    /// <summary>
    /// Логика взаимодействия для WorkWindowForAdmins.xaml
    /// </summary>
    public partial class WorkWindowForAdmins : Window
    {
        public WorkWindowForAdmins()
        {
            CurrentTableName = "Auctions";
            InitializeComponent();
            tableChangeButton.Click += (s, e) => SwapDB();
            downloadButton.Click += (s, e) => DownloadDB();
            changeButton.Click += (s, e) => UpdateDB();
            deliteSelectedButton.Click += (s, e) => Button_DeleteSelectedRows();
            usersAdministrationButton.Click += (s, e) => UsersAdministrate();
            this.Closing += (s, e) => Window_Closing();

            RefreshData();
        }

        string CurrentTableName { get; set; }
        DataSet? Data { get; set; }
        SqlDataAdapter? DataAdapter { get; set; }
        SqlConnection? Connection { get; set; }

        void SwapDB()
        {
            SwapDbWindow swapDbWindow = new SwapDbWindow();
            swapDbWindow.ShowDialog();
            if (swapDbWindow.NewTableName != null)
            { CurrentTableName = swapDbWindow.NewTableName; }

            RefreshData();
        }

        void DownloadDB()
        {
            if (Data != null)
            {
                ChoseDownloadFormatWindow choseDownloadFormatWindow = new ChoseDownloadFormatWindow(Data);
                choseDownloadFormatWindow.ShowDialog();
            }
        }

        void RefreshData()
        {
            if (Connection != null)
            {
                Connection.CloseAsync();
            }

            SqlDataAdapter sqlDataAdapter;
            SqlConnection sqlConnection;

            Data = SqlProcessing.ShowTable("AuctionsDB", CurrentTableName, out sqlDataAdapter, out sqlConnection);
            DataAdapter = sqlDataAdapter;
            Connection = sqlConnection;

            DbShowDataGrid.ItemsSource = Data.Tables[0].AsDataView();
            DbShowDataGrid.DataContext = Data.Tables[0];


            tableName.Text = $"Таблица {CurrentTableName}";
        }


        void UpdateDB()
        {
            try
            {
                if (Data != null && DataAdapter != null)
                {
                    SqlCommandBuilder builder = new SqlCommandBuilder(DataAdapter);
                    DataAdapter.UpdateCommand = builder.GetUpdateCommand();
                    DataAdapter.Update(Data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Проверьте, все ли необходимые поля были заполнены и правильно ли указаны Id");
            }
        }

        void Window_Closing()
        {
            if (Connection != null)
            {
                Connection.Close();
            }
        }

        void Button_DeleteSelectedRows()
        {
            try
            {
                if (DbShowDataGrid.SelectedItems.Count == 1)
                {
                    int selectedIndex = DbShowDataGrid.SelectedIndex;
                    if (Data != null)
                    {
                        var row = Data.Tables[0].Rows[selectedIndex];
                        row.Delete();
                    }
                }
                else if (DbShowDataGrid.SelectedItems.Count > 1)
                {
                    int rowsCount = DbShowDataGrid.SelectedItems.Count;
                    for (int i = 0; i < rowsCount; i++)
                    {
                        int selectedIndex = DbShowDataGrid.SelectedIndex;
                        if (Data != null)
                        {
                            var row = Data.Tables[0].Rows[selectedIndex + i];
                            row.Delete();
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Возникла ошибка! Повторите попытку!");
            }
        }

        void UsersAdministrate()
        {
            UserAdministrationWindow userAdministrationWindow = new UserAdministrationWindow();
            userAdministrationWindow.ShowDialog();
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "yyyy.MM.dd";
            }
        }
    }
}