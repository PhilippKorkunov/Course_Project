using Course_Project.AllWindows;
using Course_Project.Processing;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;


namespace Course_Project
{
    /// <summary>
    /// Логика взаимодействия для WorkWindow.xaml
    /// </summary>
    public partial class WorkWindowForUsers : Window
    {
        public WorkWindowForUsers()
        {
            CurrentTableName = "Auctions";
            InitializeComponent();
            tableChangeButton.Click += (s, e) => SwapDB();
            downloadButton.Click += (s, e) => DownloadDB();

            RefreshData();
        }

        string CurrentTableName { get; set; }
        DataSet? Data { get; set; }
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
            Connection = sqlConnection;

            DbShowDataGrid.ItemsSource = Data.Tables[0].AsDataView();
            tableName.Text = $"Таблица {CurrentTableName}";
        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Connection != null)
            {
                Connection.CloseAsync();
            }
        }

        void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "yyyy.MM.dd";
            }
        }
    }
}
