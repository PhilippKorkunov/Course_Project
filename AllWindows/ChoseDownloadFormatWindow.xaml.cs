using Aspose.Cells;
using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Windows;
using Workbook = Aspose.Cells.Workbook;

namespace Course_Project.AllWindows
{
    /// <summary>
    /// Логика взаимодействия для ChoseDownloadFormatWindow.xaml
    /// </summary>
    public partial class ChoseDownloadFormatWindow : Window
    {
        public ChoseDownloadFormatWindow(DataSet data)
        {
            Data = data;
            InitializeComponent();
            WordButton.Click += (s, e) => DownloadWord();
            ExcelButton.Click += (s, e) => DownloadExcel(false);
        }

        DataSet Data { get; set; }

        void DownloadWord()
        {
            string path = ChosePath(true);

            if (path == null)
            {
                MessageBox.Show("Неправильно выбран путь! Повторите попытку");
                return;
            }

            DownloadExcel(true, path);
            
            Workbook workbook = new Workbook("MyExcelFile.xls");

            workbook.Save(path, SaveFormat.Docx);

            File.Delete("MyExcelFile.xls");

            Close();
        }

        void DownloadExcel(bool isWord, string path = null)
        {
            if (!isWord)
            {
                path = ChosePath(false);

                if (path == null)
                {
                    MessageBox.Show("Неправильно выбран путь! Повторите попытку");
                    return;
                }
            }
            else
            {
                path = "MyExcelFile.xls";
            }

            if (Data != null)
            {
                ExcelLibrary.DataSetHelper.CreateWorkbook(path, Data);
            }
            else
            {
                MessageBox.Show("Данная таблица пустая!");
            }

            Close();
        }

        string ChosePath(bool isWord)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = isWord == false ? "Excel files (*.xls)|*.xls" : "Word files (*docx)|*.docx";
            saveFileDialog.InitialDirectory = @"C:\";
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

    }
}
