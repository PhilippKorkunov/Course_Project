using Course_Project.Processing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Course_Project.AllWindows
{
    /// <summary>
    /// Логика взаимодействия для WorkWindowForSuperUsers.xaml
    /// </summary>
    public partial class WorkWindowForSuperUsers : Window
    {
        public WorkWindowForSuperUsers()
        {
            CurrentTableName = "Auctions";
            InitializeComponent();
            tableChangeButton.Click += (s, e) => SwapDB();
            downloadButton.Click += (s, e) => DownloadDB();
            changeButton.Click += (s, e) => UpdateDB();
            deliteSelectedButton.Click += (s, e) => Button_DeleteSelectedRows();
            usersAdministrationButton.Click += (s, e) => UsersAdministrate();

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
            ChoseDownloadFormatWindow choseDownloadFormatWindow = new ChoseDownloadFormatWindow(Data);
            choseDownloadFormatWindow.ShowDialog();
        }

        void RefreshData()
        {
            SqlDataAdapter sqlDataAdapter;
            SqlConnection sqlConnection;

            Data = SqlProcessing.ShowTable(CurrentTableName, out sqlDataAdapter, out sqlConnection);
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
            catch
            {
            }
        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Connection != null)
            {
                Connection.CloseAsync();
            }
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            DataGridTextColumn? column = e.Column as DataGridTextColumn;
            if (column != null)
            {
                column.ElementStyle = dataGrid.Resources["ElementStyle"] as Style;
            }
        }

        private void Button_DeleteSelectedRows()
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
                        MessageBox.Show(i.ToString());
                        int selectedIndex = DbShowDataGrid.SelectedIndex;
                        if (Data != null)
                        {
                            var row = Data.Tables[0].Rows[selectedIndex + i];
                            row.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка! Повторите попытку!");
                MessageBox.Show(ex.Message);
            }
        }

        void UsersAdministrate()
        {

        }




    }
}

