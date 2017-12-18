using System;
using NUnit.Framework;

using Miner.Database;

namespace MinerTests.DatabaseTests
{
    [TestFixture]
    public class PgSqlManagerTests
    {
        private class FakePgSqlManager : PgSqlManager
        {
            public new string CreateCommandText(string funName, params object[] args)
            {
                return base.CreateCommandText(funName, args);
            }
        }

        [TestCase("select * from \"f1\"();", "f1")]
        [TestCase("select * from \"f2\"('value', 10);", "f2", "value", 10)]
        public void TestCreateCommand(string expected, string funName, params object[] args)
        {
            var dbManager = new FakePgSqlManager();
            Assert.AreEqual(expected, dbManager.CreateCommandText(funName, args));
        }
    }
}
