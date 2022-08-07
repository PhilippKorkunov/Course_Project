using System.Windows;

namespace Course_Project
{
    internal class Validation
    {

        private static bool LoginValidation(string login)
        {
            if (string.IsNullOrEmpty(login)) { return false; }

            string validSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_@-";

            foreach(char symbol in login)
            {
                if (!validSymbols.Contains(symbol))
                {
                    return false;
                }

            }

            return true;
        }

        private static bool PasswordValidation(string password)
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

        internal static bool ValidationCheck(string login, string password)
        {
            bool isValid = true;

            if (!LoginValidation(login))
            {
                isValid = false;
                MessageBox.Show("Ошибка!\nЛогин не должен быть пустым.\nВ логине могут быть использованы только:\n\t" +
                    "1. Символы английского алфавита\n\t 2. Цифры \n\t 3. Другие символы : \"_\", \"@\", \"-\"");
            }

            if (!PasswordValidation(password))
            {
                isValid = false;
                MessageBox.Show("Ошибка!\nПароль не должен быть пустым.\nВ логине могут быть использованы только:\n\t" +
                    "1. Символы английского алфавита\n\t 2. Цифры \n\t 3. Другие символы : " +
                    "\"!\", \"#\". \"$\", \"%\", \"&\", \"'\", \"(\", \n\t\")\", \"*\", \"+\", \",\", \"-\", \".\"," +
                    "\"/\", \":\", \";\", \"<\", \"=\", \">\", \"?\",\n\t \"@\", \"[\", \"\\\", \"]\", \"^\", \"_\"," +
                    "\", \"{\", \"|\", \"}\", \"~\", \"");
            }

            return isValid;
        }
    }
}
