using System;
using NUnit.Framework;

using Miner.Math;

namespace MinerTests.MathTests
{
    [TestFixture]
    public class StdRandomizerTests
    {
        [Test]
        public void TestGetValue_ReturnsValueBetweenMinAndMax()
        {
            var randomizer = new StdRandomizer();
            for (int i = 0; i < 1000; i++)
            {
                var value = randomizer.GetValue(0, i);
                Assert.GreaterOrEqual(value, 0);
                Assert.LessOrEqual(value, i);
            }
        }
    }
}
