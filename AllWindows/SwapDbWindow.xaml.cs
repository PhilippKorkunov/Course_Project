using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для SwapDbWindow.xaml
    /// </summary>
    public partial class SwapDbWindow : Window
    {
        public SwapDbWindow()
        {
            IsSwaped = false;
            InitializeComponent();
            Auctions.Click += (s, e) => FindNewDnName(s);
            Buiers.Click += (s, e) => FindNewDnName(s);
            Items.Click += (s, e) => FindNewDnName(s);
            Lots.Click += (s, e) => FindNewDnName(s);
            Places.Click += (s, e) => FindNewDnName(s);
            Sellers.Click += (s, e) => FindNewDnName(s);
            Sellings.Click += (s, e) => FindNewDnName(s);
        }

        internal string? NewTableName { get; set; }
        internal bool IsSwaped { get; set; }

        void FindNewDnName(object s)
        {
            NewTableName = (s as Button).Content.ToString();
            IsSwaped = true;
            Close();
        }
    }
}
