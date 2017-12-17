using System;
using Miner.Game;

namespace Miner.Database
{
    public interface IDatabaseManager
    {
        void Register(string login, string password);
        void SubmitResult(string login, string password,
            GameResult result, int time);
    }
}
