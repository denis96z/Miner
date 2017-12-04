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
            for (int i = NumberOfMines.MIN_VALUE; i <= NumberOfMines.MAX_VALUE; i++)
            {
                var numberOfMines = new NumberOfMines(i);
            }
        }

        [TestCase(NumberOfMines.MIN_VALUE - 1)]
        [TestCase(NumberOfMines.MAX_VALUE + 1)]
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
            for (int i = NumberOfMines.MIN_VALUE; i <= NumberOfMines.MAX_VALUE; i++)
            {
                numberOfMines.Value = i;
                Assert.AreEqual(i, numberOfMines.Value);
            }
        }

        [TestCase(NumberOfMines.MIN_VALUE - 1)]
        [TestCase(NumberOfMines.MAX_VALUE + 1)]
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
