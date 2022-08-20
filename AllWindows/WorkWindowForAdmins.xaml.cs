using Course_Project.Processing;
using System;
using System.Collections.Generic;
using System.Data;
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
            tableName.Text = $"Таблица {CurrentTableName}";
        }

        void UpdateDB()
        {
            try
            {
                if (Data != null && DataAdapter != null)
                {
                    DataAdapter.Update(Data.Tables[0]);
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
    }
}

