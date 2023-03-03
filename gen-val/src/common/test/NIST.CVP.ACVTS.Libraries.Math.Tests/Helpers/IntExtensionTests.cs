using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class IntExtensionTests
    {
        [Test]
        [TestCase(1, 1, 1)]
        [TestCase(1, 100, 1)]
        [TestCase(200, 4, 50)]
        [TestCase(300, 7, 43)]
        [TestCase(104714, 198, 529)]
        [TestCase(981724, 8176, 121)]
        public void ShouldCeilingDivideProperly(int numerator, int denominator, int expectedResult)
        {
            Assert.AreEqual(expectedResult, numerator.CeilingDivide(denominator));
        }

        [Test]
        [TestCase(1, 2, 2)]
        [TestCase(3, 2, 4)]
        [TestCase(4, 4, 4)]
        [TestCase(4, 10, 10)]
        [TestCase(1, 1024, 1024)]
        [TestCase(1024, 32, 1024)]
        [TestCase(1018, 32, 1024)]
        [TestCase(1024, 1024, 1024)]
        [TestCase(1025, 1024, 2048)]
        public void ShouldValueToModCorrectly(int value, int modulo, int expectedValue)
        {
            var result = value.ValueToMod(modulo);

            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        [TestCase(0x00, new byte[] { 0, 0, 0, 0 })]
        [TestCase(0x01, new byte[] { 0, 0, 0, 1 })]
        [TestCase(0xAAFF, new byte[] { 0, 0, 170, 255 })]
        [TestCase(0xFFFF, new byte[] { 0, 0, 255, 255 })]
        public void ShouldGetBytesCorrectly(int value, byte[] expectedBytes)
        {
            var result = value.GetBytes();

            Assert.IsTrue(result.SequenceEqual(expectedBytes));
        }

        [Test]
        [TestCase(0x00, new byte[] { 0, 0 })]
        [TestCase(0x01, new byte[] { 0, 1 })]
        [TestCase(0xAAFF, new byte[] { 170, 255 })]
        [TestCase(0xFFFF, new byte[] { 255, 255 })]
        public void ShouldGet16BitsCorrectly(int value, byte[] expectedBytes)
        {
            var result = value.Get16Bits();

            Assert.IsTrue(result.SequenceEqual(expectedBytes));
        }

        [Test]
        [TestCase(0x1FFFF)]
        public void Get16BitsShouldThrowGt65535(int value)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => value.Get16Bits());
        }

        [Test]
        [TestCase(0x00, new byte[] { 0 })]
        [TestCase(0x01, new byte[] { 1 })]
        [TestCase(0xAF, new byte[] { 175 })]
        [TestCase(0xFF, new byte[] { 255 })]
        public void ShouldGet8BitsCorrectly(int value, byte[] expectedBytes)
        {
            var result = value.Get8Bits();

            Assert.IsTrue(result.SequenceEqual(expectedBytes));
        }

        [Test]
        [TestCase(0x1FF)]
        public void Get8BitsShouldThrowGt255(int value)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => value.Get8Bits());
        }
    }
}
