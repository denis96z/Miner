using System;
using System.Windows.Forms;
using NSubstitute;
using NUnit.Framework;

using Miner.Data;
using Miner.Input;

namespace MinerTests.Sources.InputTests
{
    [TestFixture]
    public class InputManagerTests
    {
        [Test]
        public void TestConstructor_ValidArguments_ObjectIsCreated()
        {
            var field = Substitute.For<IField>();
            var control = new Form();
            var inputManager = new InputManager(field, control);
        }

        [Test]
        public void TestContructor_NullArgument_ThrowsException()
        {
            var field = Substitute.For<IField>();
            var control = new Control();

            Assert.Throws<ArgumentNullException>(() =>
                new InputManager(null, control));
            Assert.Throws<ArgumentNullException>(() =>
                new InputManager(field, null));
        }
    }
}
