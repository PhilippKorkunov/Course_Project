using Course_Project.Processing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Course_Project.AllWindows
{
    public partial class WorkWindowForAdmins : Window
    {
        public WorkWindowForAdmins()
        {
            CurrentTableName = "Auctions";
            InitializeComponent();
            TableChangeButton.Click += (s, e) => SwapDB();
            DownloadButton.Click += (s, e) => DownloadDB();
            //ChangeButton.Click += (s, e) => UpdateDB();
            //DeliteSelectedButton.Click += (s, e) => Button_DeleteSelectedRows();
            UsersAdministrationButton.Click += (s, e) => UsersAdministrate();
            InsertButton.Click += (s, e) => Insert();
            this.Closing += (s, e) => Window_Closing();
            RightButtonClick.Click += (s, e) => GroupMenu();
            DeleteButton.Click += (s, e) => Delete();
            UpdateButton.Click += (s, e) => Update();
            JoinButton.Click += (s, e) => Join();

            RefreshData();
        }

        string CurrentTableName { get; set; }
        DataSet? Data { get; set; }
        //DataSet? SortedData { get; set; }
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

        void RefreshData(string? groupByCommand = null, string? joinCommand = null)
        {
            if (Connection != null)
            {
                Connection.CloseAsync();
            }

            SqlDataAdapter sqlDataAdapter;
            SqlConnection sqlConnection;

            var data = SqlProcessing.ShowTable("AuctionsDB", CurrentTableName, out sqlDataAdapter, 
                out sqlConnection, groupByCommand, joinCommand);

            if (data != null)
            {
                Data = data;
                DataAdapter = sqlDataAdapter;
                Connection = sqlConnection;

                DbShowDataGrid.ItemsSource = Data.Tables[0].AsDataView();
                DbShowDataGrid.DataContext = Data.Tables[0];

                TableName.Text = $"Таблица {CurrentTableName}";
            }
        }

        void UsersAdministrate()
        {
            UserAdministrationWindow userAdministrationWindow = new UserAdministrationWindow();
            userAdministrationWindow.ShowDialog();
        }

        void Insert()
        {
            if (Connection != null && Data != null)
            {
                InsertWindow insertWindow = new InsertWindow(CurrentTableName, Data);
                insertWindow.ShowDialog();
                RefreshData();
            }
            else
            {
                MessageBox.Show("Нет соединения с БД или же таблица пустая");
            }
        }

        void Update()
        {
            if (Connection != null && Data != null)
            {
                UpdateWindow updateWindow = new UpdateWindow(CurrentTableName, Data);
                updateWindow.ShowDialog();
                RefreshData();
            }
            else
            {
                MessageBox.Show("Нет соединения с БД или же таблица пустая");
            }
        }

        void Delete()
        {
            if (Connection != null && Data != null)
            {
                DeleteWindow deleteWindow = new DeleteWindow(CurrentTableName);
                deleteWindow.ShowDialog();
                RefreshData();
            }
            else
            {
                MessageBox.Show("Нет соединения с БД или же таблица пустая");
            }
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "yyyy.MM.dd";
            }
        }

        void GroupMenu()
        {
            string? columnName = DbShowDataGrid.SelectedCells[0].Column.Header.ToString();

            if (columnName != null)
            {
                string command = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS \r\n  " +
                    $"WHERE table_name = '{CurrentTableName}' AND COLUMN_NAME = '{columnName}';";
                string columnType = SqlProcessing.ExecuteCommand(command, true)[0];

                if (columnType == "int" || columnType == "decimal")
                {
                    GroupByWindow groupByWindow = new GroupByWindow(CurrentTableName);
                    groupByWindow.ShowDialog();

                    if (!String.IsNullOrEmpty(groupByWindow.GroupByCommand))
                    {
                        RefreshData(groupByCommand:groupByWindow.GroupByCommand);
                    }
                }
                else
                {
                    MessageBox.Show("Группировать по условию можно только числовые значения");
                }
            }
        }

        void Join()
        {
            JoinWindow joinWindow = new JoinWindow();
            joinWindow.ShowDialog();
            if (!String.IsNullOrEmpty(joinWindow.JoinCommand))
            {
                var dataToCheck = Data;
                RefreshData(joinCommand: joinWindow.JoinCommand);
                if (Data != dataToCheck)
                {
                    TableName.Text = $"InnerJoin таблиц {joinWindow.FirstTableName} и {joinWindow.SecondTableName}";
                }
            }
        }

        void Window_Closing()
        {
            if (Connection != null)
            {
                Connection.Close();
            }
        }
    }
}