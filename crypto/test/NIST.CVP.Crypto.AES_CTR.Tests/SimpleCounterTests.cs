using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_CTR.Tests
{
    [TestFixture, FastCryptoTest]
    public class SimpleCounterTests
    {
        [Test]
        public void ShouldWrapTheCounterWhenAtMaxValue()
        {
            var initialValue = BitString.Ones(128);
            var subject = new SimpleCounter(initialValue);

            var firstResult = subject.GetNextIV();
            var secondResult = subject.GetNextIV();
            var thirdResult = subject.GetNextIV();

            Assert.AreEqual(initialValue, firstResult);
            Assert.AreEqual(BitString.Zeroes(128), secondResult);
            Assert.AreEqual(BitString.ConcatenateBits(BitString.Zeroes(127), BitString.One()), thirdResult);
        }

        [Test]
        public void ShouldIncreaseByOneEachCall()
        {
            var subject = new SimpleCounter();

            var prevResult = subject.GetNextIV().ToPositiveBigInteger();
            for (var i = 0; i < 1000; i++)
            {
                var curResult = subject.GetNextIV().ToPositiveBigInteger();
                Assert.AreEqual(prevResult + 1, curResult);

                prevResult = curResult;
            }
        }

        [Test]
        [TestCase("00")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")]
        [TestCase("abcdef123456")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")]
        public void ShouldAlwaysOfferExactly128Bits(string hex)
        {
            var firstValue = new BitString(hex);
            var subject = new SimpleCounter(firstValue);

            for (var i = 0; i < 1000; i++)
            {
                Assert.AreEqual(128, subject.GetNextIV().BitLength);
            }
        }
    }
}
