using System;

namespace Miner.Database
{
    /// <summary>
    /// Предоставляет методы проверки данных,
    /// передаваемых в базу данных.
    /// </summary>
    public static class Authentication
    {
        /// <summary>
        /// Проверяет корректность логина пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <returns>True, если логин корректный, иначе - false.</returns>
        public static int CheckLogin(string login)
        {
            return CheckInput(login);
        }

        /// <summary>
        /// Проверяет корректность пароля пользователя.
        /// </summary>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>True, если пароль корректный, иначе - false.</returns>
        public static int CheckPassword(string password)
        {
            return CheckInput(password);
        }

        // Проверяет корректность введенной строки.
        private static int CheckInput(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (!Char.IsLetter(c) && !Char.IsDigit(c))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
