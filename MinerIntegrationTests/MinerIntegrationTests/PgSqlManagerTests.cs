using System;
using NUnit.Framework;
using Miner.Database;
using Miner.Game;

namespace MinerIntegrationTests
{
    [TestFixture]
    public class PgSqlManagerTests
    {
        private class FakeDbManager : PgSqlManager
        {
            public void ClearDatabase()
            {
                var command = "delete from \"Results\";";
                ExecuteNonQuery(command);
                command = "delete from \"Users\";";
                ExecuteNonQuery(command);
            }
        }

        [Test]
        public void TestRegister_Success()
        {
            var db = new FakeDbManager();
            db.ClearDatabase();
            db.Register("Denis", "1234");
        }

        [Test]
        public void TestRegister_FailToRegisterTwice()
        {
            var db = new FakeDbManager();
            db.ClearDatabase();
            db.Register("Denis", "1234");

            Assert.Throws<Exception>(() =>
            {
                db.Register("Denis", "0000");
            });
        }

        [TestCase("Denis", "1234")]
        public void TestSubmitResult_Success(string login,
            string password, bool expected)
        {
            var db = new FakeDbManager();
            db.ClearDatabase();
            db.Register(login, password);

            db.SubmitResult(login, password, GameResult.Loss, 0);
            Assert.Throws<Exception>(() =>
            {
                db.SubmitResult(login, String.Empty, GameResult.Loss, 0);
            });
        }
    }
}
