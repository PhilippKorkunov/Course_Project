using System.Windows;
using System.ComponentModel.DataAnnotations;

namespace Course_Project
{
    internal class Validation
    {

        static bool IsLoginValid(string login)
        {
            if (string.IsNullOrEmpty(login)) { return false; }

            string validSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_@-";

            if (validSymbols.Substring(validSymbols.Length - 3,3).Contains(login[0]) || 
                validSymbols.Substring(validSymbols.Length - 3, 3).Contains(login[login.Length - 1]))
            {
                return false;
            }


            foreach(char symbol in login)
            {
                if (!validSymbols.Contains(symbol))
                {
                    return false;
                }
            }

            return true;
        }

        static void LoginMessageBox()
        {
            MessageBox.Show("Ошибка!\nЛогин не должен быть пустым.\nВ логине могут быть использованы только:\n\t" +
                     "1. Символы английского алфавита\n\t 2. Цифры \n\t 3. Специальные символы : \"_\", \"@\", \"-\"" +
                     "4. Логин не может начигаться со специальных сиволов и заканчиваться на них");
        }

        static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password)) { return false; }

            string validSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!#$%&'()*+,-./:;<=>?@[\\]^_`{|}~\"";

            foreach (char symbol in password)
            {
                if (!validSymbols.Contains(symbol))
                {
                    return false;
                }
            }

            return true;
        }

        static void PasswordMessageBox()
        {
            MessageBox.Show("Ошибка!\nПароль не должен быть пустым.\nВ логине могут быть использованы только:\n\t" +
                    "1. Символы английского алфавита\n\t 2. Цифры \n\t 3. Другие символы : " +
                    "\"!\", \"#\". \"$\", \"%\", \"&\", \"'\", \"(\", \n\t\")\", \"*\", \"+\", \",\", \"-\", \".\"," +
                    "\"/\", \":\", \";\", \"<\", \"=\", \">\", \"?\",\n\t \"@\", \"[\", \"\\\", \"]\", \"^\", \"_\"," +
                    "\", \"{\", \"|\", \"}\", \"~\", \"");
        }

        static bool IsMailValid(string address) => address != null && new EmailAddressAttribute().IsValid(address);

        static void MailMessageBox()
        {
            MessageBox.Show("Ошибка!\nТакой почты не существует!");
        }

        static bool IsNameAndSurnameValid(string name, string surname) 
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname))
            {
                return false;
            }

            string validSymbols = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

            foreach (char symbol in name)
            {
                if (!validSymbols.Contains(symbol))
                {
                    return false;
                }
            }

            foreach (char symbol in surname)
            {
                if (!validSymbols.Contains(symbol))
                {
                    return false;
                }
            }

            return true;
        }

        static void NameAndSurnameMessageBox()
        {
            MessageBox.Show("Ошибка!\nВ имени и фамилии могут использоваться только буквы русского алфавита!");
        }

        internal static bool IsLoginingValid(string login, string password)
        {
            bool isValid = true;

            if (!IsLoginValid(login))
            {
                isValid = false;
                LoginMessageBox();
            }

            if (!IsPasswordValid(password))
            {
                isValid = false;
                PasswordMessageBox();
            }

            return isValid;
        }

        internal static bool IsRegistrationValid(string login, string password, string mail, string name, string surname)
        {
            bool isValid = true;

            if (!IsLoginValid(login))
            {
                isValid = false;
                LoginMessageBox();
            }

            if (!IsPasswordValid(password))
            {
                isValid = false;
                PasswordMessageBox();
            }

            if (!IsMailValid(mail))
            {
                isValid = false;
                MailMessageBox();
            }

            if (!IsNameAndSurnameValid(name, surname))
            {
                isValid = false;
                NameAndSurnameMessageBox();
            }

            return isValid;
        }
    }
}
