using System;
using System.Linq;

namespace Miner.Database
{
    public static class Authentication
    {
        public static int CheckLogin(string login)
        {
            for (int i = 0; i < login.Length; i++)
            {
                char c = login[i];
                if (!Char.IsLetter(c) && !Char.IsDigit(c))
                {
                    return i;
                }
            }
            return -1;
        }

        public static int CheckPassword(string password)
        {
            var validSymbols = new char[]
            {
                ' ', '_'
            };

            for (int i = 0; i < password.Length; i++)
            {
                char c = password[i];
                if (!Char.IsLetter(c) && !Char.IsDigit(c) && !validSymbols.Contains(c))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
