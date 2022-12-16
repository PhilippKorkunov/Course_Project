using System;
using System.Collections.Generic;
using System.Windows;

namespace Course_Project.AllWindows
{
    public partial class GroupByWindow : Window
    {
        internal string? GroupByCommand { get; set; }
        string TableName { get; set; }
        public GroupByWindow(string tableName)
        {
            GroupByCommand = null;
            TableName = tableName;
            InitializeComponent();
            Header.Text = $"GroupBy для таблицы {tableName}";
            GroupByButton.Click += (s, e) => GroupBy();
        }

        void GroupBy()
        {
            if (!String.IsNullOrEmpty(HavingBox.Text) && String.IsNullOrEmpty(GroupByBox.Text))
            {
                MessageBox.Show("Заполните поле 'Group By'");
            }
            else
            {
                Dictionary<string, string> dictForGroupBy = new Dictionary<string, string>();
                GroupByBox.Text.Replace(" ", "");
                HavingBox.Text.Replace(" ", "");
                if (!String.IsNullOrEmpty(HavingBox.Text))
                {
                    try
                    {
                        dictForGroupBy.Add("GroupBy", GroupByBox.Text);
                        var splitedHaving = HavingBox.Text.Split(")");

                        dictForGroupBy.Add("HavingFunc", splitedHaving[0] + ")");
                        dictForGroupBy.Add("HavingCondition", splitedHaving[1]);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\nОжидаются агрегатные функции SQL");
                    }
                }
                else if (!String.IsNullOrEmpty(GroupByBox.Text)) { dictForGroupBy.Add("GroupBy", GroupByBox.Text); }


                if (dictForGroupBy.ContainsKey("HavingFunc"))
                {
                    GroupByCommand = $"SELECT {dictForGroupBy["GroupBy"]}, {dictForGroupBy["HavingFunc"]} as '{dictForGroupBy["HavingFunc"]}'" +
                    $" FROM {TableName} Group By {dictForGroupBy["GroupBy"]}";
                    GroupByCommand += $" HAVING {dictForGroupBy["HavingFunc"]}{dictForGroupBy["HavingCondition"]}";
                }
                else
                {
                    GroupByCommand = $"SELECT {dictForGroupBy["GroupBy"]} " +
                                            $"FROM {TableName} Group By {dictForGroupBy["GroupBy"]}";

                }

                Close();
            }
        }
    }
}
