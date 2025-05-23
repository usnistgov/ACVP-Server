﻿using NIST.CVP.ACVTS.Libraries.Crypto.AES;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Tests.Aes
{
    [TestFixture, FastCryptoTest]
    public class RijndaelKeyScheduleTests
    {
        [Test]
        [TestCase(1)]
        [TestCase(64)]
        [TestCase(127)]
        public void ShouldBeInvalidIfInvalidKeySize(int keySize)
        {
            var subject = new RijndaelKeySchedule(keySize, 128, new byte[4, 4]);
            Assert.That(subject.IsValid, Is.False);
        }

        [Test]
        [TestCase(1)]
        [TestCase(64)]
        [TestCase(127)]
        public void ShouldBeInvalidIfInvalidBlockSize(int blockSize)
        {
            var subject = new RijndaelKeySchedule(128, blockSize, new byte[4, 4]);
            Assert.That(subject.IsValid, Is.False);
        }

        [Test]
        [TestCase(1)]
        [TestCase(64)]
        [TestCase(127)]
        public void ShouldHaveProperErrorMessageIfInvalidKeySize(int keySize)
        {
            var subject = new RijndaelKeySchedule(keySize, 128, new byte[4, 4]);
            Assert.That(!subject.IsValid);
            Assert.That(subject.ErrorMessage, Is.EqualTo($"Invalid key size: {keySize}"));
        }

        [Test]
        [TestCase(1)]
        [TestCase(64)]
        [TestCase(127)]
        public void ShouldHaveProperErrorMessageIfInvalidBlockSize(int blockSize)
        {
            var subject = new RijndaelKeySchedule(128, blockSize, new byte[4, 4]);
            Assert.That(!subject.IsValid);
            Assert.That(subject.ErrorMessage, Is.EqualTo($"Invalid block size: {blockSize}"));
        }


        [Test]
        [TestCase(128, 128)]
        [TestCase(128, 192)]
        [TestCase(128, 256)]
        [TestCase(192, 128)]
        [TestCase(192, 192)]
        [TestCase(192, 256)]
        [TestCase(256, 128)]
        [TestCase(256, 192)]
        [TestCase(256, 256)]
        public void ShouldBeValidIfValidKeySizeAndBlockSize(int keySize, int blockSize)
        {

            var subject = new RijndaelKeySchedule(keySize, blockSize, new byte[4, 8]);
            Assert.That(subject.IsValid, Is.True);
        }

        [Test]
        [TestCase(128, 128, 10)]
        [TestCase(128, 192, 12)]
        [TestCase(128, 256, 14)]
        [TestCase(192, 128, 12)]
        [TestCase(192, 192, 12)]
        [TestCase(192, 256, 14)]
        [TestCase(256, 128, 14)]
        [TestCase(256, 192, 14)]
        [TestCase(256, 256, 14)]
        public void ShouldHaveProperNumberOfRoundsForKeyAndBlockSize(int keySize, int blockSize, int expectedRounds)
        {
            var subject = new RijndaelKeySchedule(keySize, blockSize, new byte[4, 8]);
            Assert.That(subject.Rounds, Is.EqualTo(expectedRounds));
        }
    }
}
