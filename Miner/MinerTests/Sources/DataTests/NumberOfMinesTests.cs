using System;
using NUnit.Framework;

using Miner.Data;

namespace MinerTests.DataTests
{
    [TestFixture]
    public class NumberOfMinesTests
    {
        [Test]
        public void TestConstructor_ValidValue_ObjectIsCreated()
        {
            for (int i = NumberOfMines.MinValue; i <= NumberOfMines.MaxValue; i++)
            {
                var numberOfMines = new NumberOfMines(i);
            }
        }

        [TestCase(NumberOfMines.MinValue - 1)]
        [TestCase(NumberOfMines.MaxValue + 1)]
        public void TestConstructor_InvalidValue_ThrowsException(int value)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var numberOfMines = new NumberOfMines(value);
            });
        }

        [Test]
        public void TestValueGetterSetter_ValidValue()
        {
            var numberOfMines = new NumberOfMines();
            for (int i = NumberOfMines.MinValue; i <= NumberOfMines.MaxValue; i++)
            {
                numberOfMines.Value = i;
                Assert.AreEqual(i, numberOfMines.Value);
            }
        }

        [TestCase(NumberOfMines.MinValue - 1)]
        [TestCase(NumberOfMines.MaxValue + 1)]
        public void TestValueGetterSetter_InvalidValue_ThrowsException(int value)
        {
            var numberOfMines = new NumberOfMines();
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                numberOfMines.Value = value;
            });
        }
    }
}
