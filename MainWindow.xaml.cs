using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Course_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            logInButton.Click += (s, e) => UserCheck();
            registrationButton.Click += (s, e) => RegistrationPage();
            showPasswordButton.Click += (s, e) => ShowPassword();


        }

        bool isShowen = false;

        private void UserCheck()
        {
            string login = loginBox.Text;
            string password = passwordBox.Password;

            bool isValid = Validation.IsLoginingValid(login, password);
            bool isAuthorized = Authorization.AuthorizationCheck(login, password);

            if (isValid && isAuthorized)
            {
                //WorkWindow open
                Close();
            }
        }

        private void RegistrationPage()
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            Close();
        }

        public void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passwordBox.Password))
            {
                passwordBlock.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(passwordBox.Password) && isShowen)
            {
                passwordBlock.Visibility = Visibility.Visible;
            }
        }

        async void ShowPassword()
        {
            passwordShowBlock.Text = passwordBox.Password;
            passwordShowBlock.Visibility = Visibility.Visible;
            passwordBox.Password = null;
            isShowen = true;
            await Task.Run(async () =>
            {
                Thread.Sleep(1000);
                passwordShowBlock.Visibility = Visibility.Collapsed;
                passwordBox.Password = passwordShowBlock.Text; 
            });
        }

    }
}
