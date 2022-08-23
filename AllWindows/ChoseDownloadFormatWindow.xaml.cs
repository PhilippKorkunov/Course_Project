using Aspose.Cells;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using System;
using System.Data;
using System.IO;
using System.Windows;
using Spire.Doc;
using Spire.Doc.Documents;
using Document = Spire.Doc.Document;
using Paragraph = Spire.Doc.Documents.Paragraph;
using Spire.DataExport.RTF;

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
            ExcelButton.Click += (s, e) => DownloadExcel();
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


            RTFExport rtfExport = new RTFExport();
            rtfExport.DataSource = Spire.DataExport.Common.ExportSource.DataTable;
            rtfExport.DataTable = Data.Tables[0];
            rtfExport.FileName = path;
            rtfExport.ActionAfterExport = Spire.DataExport.Common.ActionType.None;
            rtfExport.SaveToFile();

            Close();
        }

        void DownloadExcel()
        {
            string path = ChosePath(false);

            if (path == null)
            {
                MessageBox.Show("Неправильно выбран путь! Повторите попытку");
                return;
            }

            if (Data != null)
            {
                using (XLWorkbook workbook = new XLWorkbook())
                {
                    workbook.Worksheets.Add(Data);
                    workbook.SaveAs(path);
                }
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
            saveFileDialog.Filter = isWord == false ? "Excel Workbook|*.xlsx" : "Word files (*doc)|*.doc";
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
