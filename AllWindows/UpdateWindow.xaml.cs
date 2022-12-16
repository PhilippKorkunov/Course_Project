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
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        string CurrentTableName { get; set; }
        List<TextBox> TextList { get; set; }

        public UpdateWindow(string currentTableName, DataSet data)
        {
            InitializeComponent();

            CurrentTableName = currentTableName;

            TextBlock textBlockHead = new TextBlock();
            textBlockHead.Text = $"Изменить запись в таблице {currentTableName}";
            textBlockHead.HorizontalAlignment = HorizontalAlignment.Center;
            textBlockHead.VerticalAlignment = VerticalAlignment.Top;
            textBlockHead.FontSize = 16;
            textBlockHead.Margin = new Thickness(0, 20, 0, 0);

            Grid.Children.Add(textBlockHead);

            int coulmnsCount = data.Tables[0].Columns.Count;
            TextList = new List<TextBox>();

            for (int i = 0; i < coulmnsCount; i++)
            {

                TextBlock textBlock = new TextBlock();
                textBlock.Text = data.Tables[0].Columns[i].ColumnName + ":";
                textBlockHead.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.Margin = new Thickness(40, 70 + 50 * i, 0, -70 - 50 * i);

                TextBox textBox = new TextBox();
                textBox.Name = data.Tables[0].Columns[i].ColumnName;
                textBox.VerticalAlignment = VerticalAlignment.Top;
                textBox.Height = 25;
                textBox.Width = 325;
                textBox.FontSize = 12;
                textBox.TextAlignment = TextAlignment.Justify;
                textBox.Margin = new Thickness(0, 90 + 50 * i, 0, 0);
                TextList.Add(textBox);


                Grid.Children.Add(textBlock);
                Grid.Children.Add(textBox);
            }

            Button insertButton = new Button();
            insertButton.Content = "Обновить";
            insertButton.Background = new SolidColorBrush(Colors.Cornsilk);
            insertButton.Height = 30;
            insertButton.FontSize = 16;
            insertButton.Width = 250;
            insertButton.HorizontalAlignment = HorizontalAlignment.Center;
            insertButton.VerticalAlignment = VerticalAlignment.Top;
            insertButton.Margin = new Thickness(0, 90 + 50 * coulmnsCount + 15, 0, 0);
            Grid.Children.Add(insertButton);

            insertButton.Click += (s, e) => Update();

            Height = 90 + 50 * coulmnsCount + 100;
            MaxHeight = Height;
            MinHeight = Height;
        }


        void Update()
        {
            string updateCommand = $"UPDATE {CurrentTableName} SET";

            for(int i = 1; i < TextList.Count; i++)
            {
                var item = TextList[i];
                if (!String.IsNullOrEmpty(item.Text))
                {
                    updateCommand += $" {item.Name}=";
                    updateCommand += $"N'{item.Text}',";
                }
            }

            updateCommand = updateCommand.Remove(updateCommand.Length - 1);

            updateCommand += $" WHERE {TextList[0].Name}=N'{TextList[0].Text}'";

            //MessageBox.Show(updateCommand);
            SqlConnection sqlConnection;

            SqlProcessing.ExecuteCommand(updateCommand);
            Close();

        }
    }
}
