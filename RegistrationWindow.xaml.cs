using System;
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
            string login = loginBox.Text;
            string password = passwordBox.Password;
            string comfirmingPassword = comfirmPasswordBox.Password;
            string mail = mailBox.Text;
            string number = phoneNumberBox.Text;
            string name = nameBox.Text;
            string surname = surnameBox.Text;
            string patronymic = patronymicBox.Text;

            if (IsPasswordsEquals(password, comfirmingPassword))
            {
                if (Validation.IsRegistrationValid(login, password, mail, number, name, surname, patronymic))
                {
                    bool isAdded = true;

                    var sqlConnection = SqlProcessing.OpenConnection("UsersDB");

                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Users] (Name, Surname, Patronymic, Email, PhoneNumber, Login, Password) " +
                        $"VALUES (N'{name}', N'{surname}', N'{patronymic}', '{mail}', '{number}', '{login}', CONVERT(varbinary, '{password}'))", sqlConnection);

                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        string exeption = ex.Message;
                        if (exeption.Contains(login)) { MessageBox.Show($"Логин {login} уже занят. Придумайте другой логин!"); }
                        if (exeption.Contains(mail)) { MessageBox.Show($"Почта {mail} уже привязана к другому аккаунту!"); }
                        if (exeption.Contains(number)) { MessageBox.Show($"Телефон {number} уже привязан к другому аккаунту!"); }

                        isAdded = false;
                        
                    }
                    finally
                    {
                        sqlConnection.CloseAsync();
                    }

                    if (isAdded) 
                    {
                        MessageBox.Show("Учетная запись создана! Дождитесь подтверждения от администратора, " +
                        "после чего учетная запись станет активной");
                    }

                    Return();
                    Close();
                }
            }
        }


        static bool IsPasswordsEquals(string password, string comfirmingPassword)
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
