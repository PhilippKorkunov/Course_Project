using Course_Project.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace Course_Project.AllWindows
{
    /// <summary>
    /// Логика взаимодействия для JoinWindow.xaml
    /// </summary>
    public partial class JoinWindow : Window
    {
        internal string? JoinCommand { get; set; }
        internal string FirstTableName { get; set; }
        internal string SecondTableName { get; set; }
        public JoinWindow()
        {
            JoinCommand = "";
            FirstTableName = "";
            SecondTableName = "";
            InitializeComponent();
            JoinButton.Click += (s, e) => Join();
        }

        void Join()
        {
            string command = "SELECT  Name from Sysobjects where xtype = 'u';"; //Узнаем имена таблиц
            var tableNames = SqlProcessing.ExecuteCommand(command, true);
            FirstTableName = FirstNameBox.Text.Replace(" ", "");
            SecondTableName = SecondNameBox.Text.Replace(" ", "");

            if (tableNames.Contains(FirstTableName) && tableNames.Contains(SecondTableName) && FirstTableName != SecondTableName)
            {
                string commandForFirst = $"SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('{FirstTableName}')";//Узнаем имена колонок
                string commandForSecond = $"SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('{SecondTableName}')";

                var firstTableColumnNames = SqlProcessing.ExecuteCommand(commandForFirst, isToRead:true);
                var secondTableColumnNames = SqlProcessing.ExecuteCommand(commandForSecond, isToRead:true);
                var sameForeignKeys = Intersect(secondTableColumnNames, firstTableColumnNames);//Общие внешние ключи

                JoinCommand = $"SELECT * FROM {FirstTableName} INNER JOIN {SecondTableName} ON";

                if (sameForeignKeys.Count > 0 && String.IsNullOrEmpty(IdEquationBox.Text))
                {
                    IdEquationBox.Visibility = Visibility.Hidden;
                    foreach (string columnName in sameForeignKeys)
                    {
                        JoinCommand += $" {FirstTableName}.{columnName} = {SecondTableName}.{columnName} AND";
                    }
                    JoinCommand = JoinCommand.Substring(0, JoinCommand.Length - 4);
                    Close();
                }
                else if (String.IsNullOrEmpty(IdEquationBox.Text))
                {
                    MessageBox.Show("У полученных таблиц нет совпадений по внещним ключам.\n " +
                        "Напишите сопоставление ключей в 3-ей появившейся строке");
                    IdEquationBox.Visibility = Visibility.Visible;
                    IdEquationBox.Text = $"{FirstTableName}.<ключ>={SecondTableName}.<ключ>";
                    JoinCommand = null;
                }
                else
                {
                    IdEquationBox.Text.Replace(" ", "");
                    JoinCommand += $" {IdEquationBox.Text}";
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Одно или оба имени таблицы введены неверно! Должжны быть введены имена разных таблиц!");
                JoinCommand = null;
            }

        }

        List<string> Intersect(List<string> list1, List<string> list2)
        {
            List<string> strings = new List<string>();
            foreach(string str in list1)
            {
                if (str.Contains("Id"))
                {
                    if (list2.Contains(str))
                    {
                        strings.Add(str);
                        list2.Remove(str);
                    }
                }
                else
                {
                    list2.Remove(str);
                }
            }

            return strings;
        }
    }
}
