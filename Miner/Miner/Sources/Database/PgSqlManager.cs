﻿using System;
using Devart.Data.PostgreSql;
using Miner.Game;

namespace Miner.Database
{
    /// <summary>
    /// Контроллер доступа к базе данных PostgreSql.
    /// </summary>
    public class PgSqlManager : IDatabaseManager
    {
        // Логин пользователя базы данных.
        private const string UserId = "postgres";
        // Пароль пользователя базы данных.
        private const string Password = "0000";

        /// <summary>
        /// Адрес сервера базы данных.
        /// </summary>
        protected const string Host = "localhost";

        /// <summary>
        /// Порт на сервере базы данных.
        /// </summary>
        protected const string Port = "5432";

        /// <summary>
        /// Имя базы данных на сервере.
        /// </summary>
        protected const string Database = "Miner";

        // Строка подключения к базе данных.
        private const string ConnectionString =
            "User Id=" + UserId + ";" +
            "Password=" + Password + ";" +
            "Host=" + Host + ";" +
            "Port=" + Port + ";" +
            "Database=" + Database + ";";

        // Подключение к базе данных.
        private readonly PgSqlConnection _connection =
            new PgSqlConnection(ConnectionString);

        /// <summary>
        /// Регистрирует пользователя в базе данных.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        public void Register(string login, string password)
        {
            ExecuteNonQuery(CreateCommandText("Register", login, password));
        }

        /// <summary>
        /// Добавляет в базу данных результат игры пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="result">Результат игры.</param>
        /// <param name="time">Время игры.</param>
        public void SubmitResult(string login, string password, GameResult result, int time)
        {
            ExecuteNonQuery(CreateCommandText("SubmitResult",
                login, password, result.ToString(), time));
        }

        /// <summary>
        /// Формирует запрос к базе данных из
        /// имени и аргументов функции.
        /// </summary>
        /// <param name="funName">Имя функции.</param>
        /// <param name="args">Аргументы функции.</param>
        /// <returns>Строка запроса к базе данных.</returns>
        protected string CreateCommandText(string funName, params object[] args)
        {
            string command = "select * from \"" + funName + "\"(";
            if (args.Length > 0)
            {
                int lastIndex = args.Length - 1;
                for (int i = 0; i < lastIndex; i++)
                {
                    command += CreateArgument(args[i]) + ", ";
                }
                command += CreateArgument(args[lastIndex]);
            }
            command += ");";
            return command;
        }

        // Возвращает строковое представление аргумента функции.
        private string CreateArgument(object arg)
        {
            return arg is string ? "'" + arg.ToString() + "'" : arg.ToString();
        }

        // Выполняет запрос к базе данных.
        protected void ExecuteNonQuery(string commandText)
        {
            var command = new PgSqlCommand(commandText, _connection);
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }
    }
}
