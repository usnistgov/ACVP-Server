using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.CTR.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.CTR.Tests
{
    [TestFixture, FastCryptoTest]
    public class TestableCounterTests
    {
        [Test]
        public void ShouldAlwaysGetCorrectCounter()
        {
            var ivs = new List<BitString>
            {
                BitString.Zeroes(120).ConcatenateBits(new BitString("00")),
                BitString.Zeroes(120).ConcatenateBits(new BitString("04")),
                BitString.Zeroes(120).ConcatenateBits(new BitString("08")),
                BitString.Zeroes(120).ConcatenateBits(new BitString("0C")),
                BitString.Zeroes(120).ConcatenateBits(new BitString("10")),
                BitString.Zeroes(120).ConcatenateBits(new BitString("14")),
                BitString.Zeroes(120).ConcatenateBits(new BitString("18")),
                BitString.Zeroes(120).ConcatenateBits(new BitString("1C")),
                BitString.Zeroes(120).ConcatenateBits(new BitString("20"))
            };

            var subject = new TestableCounter(Cipher.AES, ivs);

            foreach (var iv in ivs)
            {
                var result = subject.GetNextIV();
                Assert.AreEqual(iv, result);
            }
        }

        [Test]
        public void ShouldThrowExceptionWhenThereAreNoMoreCounters()
        {
            var ivs = new List<BitString>
            {
                BitString.Zeroes(128)
            };

            var subject = new TestableCounter(Cipher.AES, ivs);
            var firstResult = subject.GetNextIV();

            Assert.Throws(Is.TypeOf<Exception>(), () => subject.GetNextIV());
        }

        [Test]
        [TestCase(Cipher.AES, 128)]
        [TestCase(Cipher.TDES, 64)]
        public void ShouldGetCorrectBlockSize(Cipher cipher, int blockSize)
        {
            var ivs = new List<BitString>
            {
                BitString.Zero()
            };

            var subject = new TestableCounter(cipher, ivs);
            var result = subject.GetNextIV();
            Assert.AreEqual(blockSize, result.BitLength);
        }

        [Test]
        [TestCase("00")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")]
        [TestCase("abcdef123456")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")]
        public void ShouldAlwaysOfferExactly128Bits(string hex)
        {
            var ivs = new List<BitString>
            {
                new BitString(hex),
                new BitString(hex),
                new BitString(hex)
            };

            var subject = new TestableCounter(Cipher.AES, ivs);

            foreach (var iv in ivs)
            {
                var result = subject.GetNextIV();
                Assert.AreEqual(128, result.BitLength);
            }
        }
    }
}
