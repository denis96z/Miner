using System;
using Devart.Data.PostgreSql;
using Miner.Game;

namespace Miner.Database
{
    class PgSqlManager : IDatabaseManager
    {
        private const string USER_ID = "postgres";
        private const string PASSWORD = "0000";
        private const string HOST = "localhost";
        private const string PORT = "5432";
        private const string DATABASE = "Miner";

        private const string CONNECTION_STRING =
            "User Id=" + USER_ID + ";" +
            "Password=" + PASSWORD + ";" +
            "Host=" + HOST + ";" +
            "Port=" + PORT + ";" +
            "Database=" + DATABASE + ";";

        private PgSqlConnection connection =
            new PgSqlConnection(CONNECTION_STRING);

        public void Register(string login, string password)
        {
            ExecuteNonQuery(CreateCommandText("Register", login, password));
        }

        public void SubmitResult(string login, string password,
            GameResult result, int time)
        {
            ExecuteNonQuery(CreateCommandText("SubmitResult",
                login, password, result.ToString(), time));
        }

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

        private string CreateArgument(object arg)
        {
            return arg is string ? "'" + arg.ToString() + "'" : arg.ToString();
        }

        protected void ExecuteNonQuery(string commandText)
        {
            var command = new PgSqlCommand(commandText, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
