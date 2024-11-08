﻿using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTR.Fakes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.CTR.Tests
{
    [TestFixture, FastCryptoTest]
    public class TestableCounterTests
    {
        private readonly AesEngine _aesEngine = new AesEngine();

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

            var subject = new TestableCounter(_aesEngine, ivs);

            foreach (var iv in ivs)
            {
                var result = subject.GetNextIV();
                Assert.That(result, Is.EqualTo(iv));
            }
        }

        [Test]
        public void ShouldThrowExceptionWhenThereAreNoMoreCounters()
        {
            var ivs = new List<BitString>
            {
                BitString.Zeroes(128)
            };

            var subject = new TestableCounter(_aesEngine, ivs);
            var firstResult = subject.GetNextIV();

            Assert.Throws(Is.TypeOf<Exception>(), () => subject.GetNextIV());
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

            var subject = new TestableCounter(_aesEngine, ivs);

            foreach (var iv in ivs)
            {
                var result = subject.GetNextIV();
                Assert.That(result.BitLength, Is.EqualTo(128));
            }
        }
    }
}
