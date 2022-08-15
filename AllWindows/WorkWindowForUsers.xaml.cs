using System;
using System.Collections.Generic;
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

namespace Course_Project
{
    /// <summary>
    /// Логика взаимодействия для WorkWindow.xaml
    /// </summary>
    public partial class WorkWindowForUsers : Window
    {
        public WorkWindowForUsers()
        {
            InitializeComponent();
            tableChangeButton.Click += (s, e) => ChangeDB();
            downloadButton.Click += (s, e) => DownloadDB();
        }

        void ChangeDB()
        {

        }

        void DownloadDB()
        {

        }
    }
}
