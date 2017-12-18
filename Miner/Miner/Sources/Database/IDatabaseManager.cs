using System;
using Miner.Game;

namespace Miner.Database
{
    /// <summary>
    /// Контроллер доступа к базе данных.
    /// </summary>
    public interface IDatabaseManager
    {
        /// <summary>
        /// Регистрирует пользователя в базе данных.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        void Register(string login, string password);

        /// <summary>
        /// Добавляет в базу данных результат игры пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="result">Результат игры.</param>
        /// <param name="time">Время игры.</param>
        void SubmitResult(string login, string password, GameResult result, int time);
    }
}
