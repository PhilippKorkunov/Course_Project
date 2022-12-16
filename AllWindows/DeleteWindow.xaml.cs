using Course_Project.Processing;
using System;
using System.Data.SqlClient;
using System.Windows;


namespace Course_Project.AllWindows
{
    public partial class DeleteWindow : Window
    {
        string CurrentTableName { get; set; }
        public DeleteWindow(string tableName)
        {
            InitializeComponent();

            CurrentTableName = tableName;
            DeleteButton.Click += (s, e) => Delete();
        }

        void Delete()
        {
            string idForDelete = IdForDeleteBox.Text;
            idForDelete.Replace(" ", "");
            
            if (idForDelete[idForDelete.Length - 1] == ',') { idForDelete.Remove(idForDelete.Length - 1); }

            if (isStringChecked(idForDelete))
            {
                string deleteCommand = $"DELETE FROM [{CurrentTableName}] WHERE " +
                    $"Id_{CurrentTableName.Remove(CurrentTableName.Length-1)} in ({idForDelete})";

                //MessageBox.Show(deleteCommand);
                SqlProcessing.ExecuteCommand(deleteCommand);
                Close();
            }
        }

        bool isStringChecked(string str)
        {
            for(int i = 0; i < str.Length; i++)
            {
                if (i > 0)
                {
                    if (str[i] == ',' && str[i] == str[i - 1])
                    {
                        MessageBox.Show("Неверный формат! Должна быть одна запятая!");
                        return false;
                    }
                }

                if (str[i] !=',' && !Char.IsDigit(str[i]))
                {
                    MessageBox.Show("Неверный формат! Перечисление должно быть через ','");
                    return false;
                }
            }

            return true;
        }
    }
}
