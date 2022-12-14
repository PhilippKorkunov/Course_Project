using System.Windows;
using Course_Project.AllWindows;
using Course_Project.Processing;


namespace Course_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();

            logInButton.Click += (s, e) => UserCheck();
            registrationButton.Click += (s, e) => RegistrationPage();
        }

        bool isShowen = false;

        private void UserCheck()
        {
            string login = loginBox.Text;
            string password = passwordBox.Password;
            bool isAdmin;

            bool isValid = Validation.IsLoginingValid(login, password);
            bool isAuthorized = new Authorization().TryAuthtorizate(login, password, out isAdmin);


            if (isValid && isAuthorized)
            {
                if (isAdmin)
                {
                    WorkWindowForAdmins workWindow = new WorkWindowForAdmins();
                    workWindow.Show();
                }
                else
                {
                    WorkWindowForUsers workWindow = new WorkWindowForUsers();
                    workWindow.Show();
                }
                Close();
            }
        }

        private void RegistrationPage()
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            Close();
        }

        void PasswordChanged(object sender, RoutedEventArgs e)
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

      

    }
}
