using System;
using NSubstitute;
using NUnit.Framework;

using Miner.Data;
using Miner.Sound;

namespace MinerTests.SoundTests
{
    [TestFixture]
    public class WaveSoundPlayerTests
    {
        public void TestFieldModifiedHandler_EventRaised_EventHandled()
        {
            var fakeField = Substitute.For<IField>();
            var mockSoundPlayer = Substitute.For<WaveSoundPlayer>(fakeField);
        }
    }
}
