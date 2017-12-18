using System;
using NSubstitute;
using Telerik.JustMock;
using NUnit.Framework;

using Arg = NSubstitute.Arg;

using Miner.Data;
using Miner.Sound;

namespace MinerTests.Sources.SoundTests
{
    [TestFixture]
    public class WaveSoundPlayerTests
    {
        [Test]
        public void TestFieldModifiedHandler_EventRaised_HandlerReceivedSignal()
        {
            var fakeField = Substitute.For<IField>();
            var soundPlayer = Mock.Create(() => new WaveSoundPlayer(fakeField));

            bool eventRaised = false;
            Mock.NonPublic.Arrange(soundPlayer,"OnFieldModified")
                .DoInstead(() => eventRaised = true);
            fakeField.Modified += Raise.Event<FieldModified>(Arg.Any<IField>(),
                Arg.Any<FieldModType>());
            Assert.IsTrue(eventRaised);
        }
    }
}
