using System;
using System.Windows.Forms;
using NSubstitute;
using NUnit.Framework;

using Miner.View;
using Miner.Data;

namespace MinerTests.ViewTests
{
    [TestFixture]
    class ControlFieldViewTests
    {
        [Test]
        public void TestConstructor_ValidArguments_ObjectCreated()
        {
            var field = Substitute.For<IField>();
            var control = new Form();

            field.Width.Returns(callInfo =>
            {
                return 1;
            });
            field.Height.Returns(callInfo =>
            {
                return 1;
            });

            var instance = new ControlFieldView(field, control);
        }
    }
}
