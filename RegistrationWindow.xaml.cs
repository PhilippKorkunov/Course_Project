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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();

            registrationButton.Click += (s, e) => Registation();
        }

        public void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passwordBox.Password))
            {
                passwordBlock.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                passwordBlock.Visibility = Visibility.Visible;
            }

            if (!string.IsNullOrEmpty(comfirmPasswordBox.Password))
            {
                comfirmPasswordBlock.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty(comfirmPasswordBox.Password))
            {
                comfirmPasswordBlock.Visibility = Visibility.Visible;
            }
        }

        public void Registation()
        {
            string login = loginBox.Text;
            string password = passwordBox.Password;
            string comfirmingPassword = comfirmPasswordBox.Password;
            string mail = mailBox.Text;
            string name = nameBox.Text;
            string surname = surnameBox.Text;

            if (IsPasswordCompare(password, comfirmingPassword))
            {
                if (Validation.IsRegistrationValid(login, password, mail, name, surname))
                {
                    MessageBox.Show("Учетная запись создана! Дождитесь подтверждения от администратора, " +
                        "после чего учетная запись станет активной");
                    //NewWindow.Open
                    Close();
                }
            }
        }

        static bool IsPasswordCompare(string password, string comfirmingPassword)
        {
            if (password != comfirmingPassword)
            {
                MessageBox.Show("Пароли должны совпадать!");
                return false;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Длина пароля должна быть не меньше 8 символов!");
                return false;
            }

            return true;
        }
    }
}
