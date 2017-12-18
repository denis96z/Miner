using System;
using NUnit.Framework;

using Miner.Database;

namespace MinerTests.DatabaseTests
{
    [TestFixture]
    public class AuthenticationTests
    {
        [TestCase("ab12")]
        [TestCase("ab 2", 2)]
        public void TestCheckLogin(string login, int invalidCharIndex = -1)
        {
            Assert.AreEqual(invalidCharIndex, Authentication.CheckLogin(login));
        }

        [TestCase("ab12")]
        [TestCase("ab 2", 2)]
        public void TestCheckPassword(string password, int invalidCharIndex = -1)
        {
            Assert.AreEqual(invalidCharIndex, Authentication.CheckPassword(password));
        }
    }
}
