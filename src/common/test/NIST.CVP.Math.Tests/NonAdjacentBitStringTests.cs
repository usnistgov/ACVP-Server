using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests
{
    [TestFixture, UnitTest]
    public class NonAdjacentBitStringTests
    {
        [Test]
        [TestCase("03")]
        [TestCase("1234982735987192871abef09d")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFF")]
        [TestCase("AAAAAAAAAAAAAAAAAAAAAAAAAA")]
        [TestCase("0004501507AC42D00B9F22B32862183F60D92DA08081")]
        public void NonAdjacentBitStringsShouldNotHaveTwoValuesNextToEachOther(string hex)
        {
            var bs = new BitString(hex);
            var bigInt = bs.ToPositiveBigInteger();

            var subject = new NonAdjacentBitString(bigInt);

            for (var i = 0; i < subject.BitLength - 1; i++)
            {   
                // If the current bit is not 0, the next bit should not be -1 or 1
                if (subject.Bits[i] != 0)
                {
                    Assert.AreNotEqual(1, System.Math.Abs(subject.Bits[i + 1]), $"Adjacent bit found at {i}");
                }
            }
        }

        [Test]
        [TestCase("03")]
        [TestCase("1234982735987192871abef09d")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFF")]
        [TestCase("AAAAAAAAAAAAAAAAAAAAAAAAAA")]
        [TestCase("0004501507AC42D00B9F22B32862183F60D92DA08081")]
        public void NonAdjacentBitStringShouldHaveTheCorrectIntegerValue(string hex)
        {
            var bs = new BitString(hex);
            var bigInt = bs.ToPositiveBigInteger();

            var subject = new NonAdjacentBitString(bigInt);

            BigInteger result = 0;

            for (var i = subject.BitLength - 1; i >= 0; i--)
            {
                result += subject.Bits[i] * (BigInteger.One << i);
            }

            Assert.AreEqual(bigInt, result);
        }
    }
}
