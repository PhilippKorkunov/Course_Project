using System;
using System.Data.SqlClient;
using System.Windows;
using Course_Project.Processing;

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
            var mainWindow = new LogInWindow();
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
                    bool isAdded = false;

                    SqlConnection sqlConnection1, sqlConnection2, sqlConnection3, sqlConnection4, sqlConnection5;
                    bool isConneceted1 = SqlProcessing.TryOpenConnection("UsersDB", out sqlConnection1);
                    bool isConneceted2 = SqlProcessing.TryOpenConnection("UsersDB", out sqlConnection2);
                    bool isConneceted3 = SqlProcessing.TryOpenConnection("UsersDB", out sqlConnection3);
                    bool isConneceted4 = SqlProcessing.TryOpenConnection("UsersDB", out sqlConnection4);
                    bool isConneceted5 = SqlProcessing.TryOpenConnection("UsersDB", out sqlConnection5);

                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Waiters] (Name, Surname, Patronymic, Email, PhoneNumber, Login, Password) " +
                        $"VALUES (N'{name}', N'{surname}', N'{patronymic}', '{mail}', '{number}', '{login}', CONVERT(varbinary, '{password}'))", sqlConnection1);

                    SqlCommand sqlCommandCheckSuperUser = new SqlCommand(
                        $"SELECT Login FROM SuperUsers\r\nWhere Login = '{login}'", sqlConnection2);

                    SqlCommand sqlCommandCheckUser3 = new SqlCommand(
                        $"SELECT * FROM Users\r\nWhere Login = '{login}'", sqlConnection3);

                    SqlCommand sqlCommandCheckUser4 = new SqlCommand(
                        $"SELECT * FROM Users\r\nWhere Email = '{mail}'", sqlConnection4);

                    SqlCommand sqlCommandCheckUser5 = new SqlCommand(
                        $"SELECT * FROM Users\r\nWhere PhoneNumber = '{number}'", sqlConnection5);

                    if (isConneceted1 && isConneceted2 && isConneceted3 && isConneceted4 && isConneceted5)
                    {
                        try
                        {
                            if (sqlCommandCheckSuperUser.ExecuteReader().HasRows || sqlCommandCheckUser3.ExecuteReader().HasRows) 
                            { MessageBox.Show($"Логин {login} уже занят. Придумайте другой логин!"); }

                            else if (sqlCommandCheckUser4.ExecuteReader().HasRows) { MessageBox.Show($"Почта {mail} уже привязана к другому аккаунту!"); }

                            else if (sqlCommandCheckUser5.ExecuteReader().HasRows) { MessageBox.Show($"Телефон {number} уже привязан к другому аккаунту!"); }

                            else
                            {
                                sqlCommand.ExecuteNonQuery();
                                isAdded = true;
                            }
                        }
                        catch (SqlException ex)
                        {
                            string exeption = ex.Message;
                            if (exeption.Contains(login)) { MessageBox.Show($"Логин {login} уже занят. Придумайте другой логин!"); }
                            else if (exeption.Contains(mail)) { MessageBox.Show($"Почта {mail} уже привязана к другому аккаунту!"); }
                            else if (exeption.Contains(number)) { MessageBox.Show($"Телефон {number} уже привязан к другому аккаунту!"); }

                            isAdded = false;
                        }
                        finally
                        {
                            sqlConnection1.CloseAsync();
                            sqlConnection2.CloseAsync();
                            sqlConnection3.CloseAsync();
                            sqlConnection4.CloseAsync();
                            sqlConnection5.CloseAsync();
                        }

                        if (isAdded)
                        {
                            MessageBox.Show("Учетная запись создана! Дождитесь подтверждения от администратора, " +
                            "после чего учетная запись станет активной");
                            Return();
                        }  
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка! Попробуйте позже.");
                    }
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
