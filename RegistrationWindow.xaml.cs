using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;


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
            returnButton.Click += (s, e) => Return();
        }

        private void Return()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
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
            var sqlConnection = OpenConnection();

            SqlCommand sqlCommand = new SqlCommand(
                $"INSERT INTO [Table] (id_user, Name, Surname, Login, Password, Email, PhoneNumber) " +
                $"VALUES (N'1', N'llll', N'kkkkkk', N'dddd', N'ooo', N'qqqq', N'dkkdkdkd')", sqlConnection);

            MessageBox.Show(sqlCommand.ExecuteNonQuery().ToString());
            sqlConnection.CloseAsync();

            string login = loginBox.Text;
            string password = passwordBox.Password;
            string comfirmingPassword = comfirmPasswordBox.Password;
            string mail = mailBox.Text;
            string number = phoneNumberBox.Text;
            string name = nameBox.Text;
            string surname = surnameBox.Text;
            string patronymic = patronymicBox.Text;

            if (IsPasswordCompare(password, comfirmingPassword))
            {
                if (Validation.IsRegistrationValid(login, password, mail, number, name, surname, patronymic))
                {
                    MessageBox.Show("Учетная запись создана! Дождитесь подтверждения от администратора, " +
                        "после чего учетная запись станет активной");


                    Return();
                    Close();
                }
            }
        }

        SqlConnection OpenConnection()
        {
            string connection = System.Configuration.ConfigurationManager.ConnectionStrings["UsersDB"].ToString();
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            return sqlConnection;
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
