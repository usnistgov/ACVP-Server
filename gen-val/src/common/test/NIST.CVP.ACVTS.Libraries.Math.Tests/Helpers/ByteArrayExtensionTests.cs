using System;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class ByteArrayExtensionTests
    {
        [Test]
        public void ShouldThrowExceptionIfByteArrayBLongerthanByteArrayA()
        {
            var bArrayA = new byte[] { 1 };
            var bArrayB = new byte[] { 1, 2 };
            Assert.Throws(typeof(ArgumentException), () => bArrayA.Add(bArrayB));
        }

        [Test]
        [TestCase("test1", new byte[] { 1 }, new byte[] { 1 }, new byte[] { 2 })]
        [TestCase("test2", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new byte[] { 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55 }, new byte[] { 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55 })]
        [TestCase("test3", new byte[] { 0, 0, 0, 0, 1, 0, 0, 0 }, new byte[] { 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55 }, new byte[] { 0x55, 0x55, 0x55, 0x55, 0x56, 0x55, 0x55, 0x55 })]
        [TestCase("test4", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 })]
        [TestCase("test5", new byte[] { 0xff, 0, 0, 0, 0, 0, 0, 0 }, new byte[] { 1, 0, 0, 0, 0, 0, 0, 1 }, new byte[] { 0, 1, 0, 0, 0, 0, 0, 1 })]
        [TestCase("test6", new byte[] { 0, 0, 0, 0xff }, new byte[] { 0, 0, 0, 1 }, new byte[] { 0, 0, 0, 0 })]
        public void ShouldAddArraysOfEqualLength(string label, byte[] bArrayA, byte[] bArrayB, byte[] expected)
        {
            var result = bArrayA.Add(bArrayB);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("test1", new byte[] { 1, 0 }, new byte[] { 1 }, new byte[] { 2, 0 })]
        [TestCase("test2", new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 }, new byte[] { 0x55 }, new byte[] { 0x56, 0, 0, 0, 0, 0, 0, 0 })]
        [TestCase("test3", new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }, new byte[] { 0x55 }, new byte[] { 0x55, 0, 0, 0, 0, 0, 0, 1 })]
        public void ShouldAddArraysWhenByteArrayALongerThanByteArrayB(string label, byte[] bArrayA, byte[] bArrayB, byte[] expected)
        {
            var result = bArrayA.Add(bArrayB);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldThrowExceptionIfByteArrayALongerthanPadLength()
        {
            var bArrayA = new byte[] { 1, 2, 3, 4 };

            Assert.Throws(typeof(ArgumentException), () => bArrayA.PadArrayToLength(2));
        }

        [Test]
        [TestCase(new byte[] { 1, 0 }, 3, new byte[] { 1, 0, 0 })]
        [TestCase(new byte[] { 0x56, 0, 0, 0 }, 4, new byte[] { 0x56, 0, 0, 0 })]
        [TestCase(new byte[] { 0, 3, 0, 0, 0 }, 8, new byte[] { 0, 3, 0, 0, 0, 0, 0, 0 })]
        public void ShouldPadArrayByAdding0BytesToMostSignificantEnd(byte[] bArrayA, int padLength, byte[] expected)
        {
            var result = bArrayA.PadArrayToLength(padLength);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("test1", new byte[] { 1, 0 }, new byte[] { 1, 1 })]
        [TestCase("test2", new byte[] { 1, 0, 0, 0 }, new byte[] { 1, 1, 1, 1 })]
        [TestCase("test3", new byte[] { 5, 0, 0, 0 }, new byte[] { 4, 1, 1, 1 })]
        [TestCase("test4", new byte[] { 4, 1, 1, 1 }, new byte[] { 4, 1, 1, 1 })]
        public void ShouldSetOddParityBit(string label, byte[] bArray, byte[] expected)
        {
            var result = bArray.SetOddParityBitInSuppliedBytes();
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("test1", new byte[] { 1, 1 })]
        [TestCase("test2", new byte[] { 1, 1, 1, 1 })]
        [TestCase("test3", new byte[] { 4, 1, 1, 1 })]
        public void ShouldHaveOddParityBitInAllBytes(string label, byte[] bArray)
        {
            var result = bArray.AllBytesHaveOddParity();
            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase("test1", new byte[] { 1, 0 })]
        [TestCase("test2", new byte[] { 1, 0, 0, 0 })]
        [TestCase("test3", new byte[] { 5, 0, 0, 0 })]
        public void ShouldNotHaveOddParityBitInAllBytes(string label, byte[] bArray)
        {
            var result = bArray.AllBytesHaveOddParity();
            Assert.That(result, Is.False);
        }

        [Test]
        [TestCase("test1", new byte[] { 0 }, 1, 0)]
        [TestCase("test2", new byte[] { 2 }, 0, 0)]
        [TestCase("test3", new byte[] { 2 }, 7, 1)]
        [TestCase("test4", new byte[] { 3 }, 7, 1)]
        [TestCase("test5", new byte[] { 4 }, 6, 1)]
        [TestCase("test6", new byte[] { 5 }, 6, 1)]
        [TestCase("test7", new byte[] { 5 }, 6, 1)]
        [TestCase("test8", new byte[] { 255 }, 1, 1)]
        [TestCase("test9", new byte[] { 255 }, 2, 1)]
        [TestCase("test10", new byte[] { 255 }, 3, 1)]
        [TestCase("test11", new byte[] { 255 }, 4, 1)]
        [TestCase("test12", new byte[] { 255 }, 5, 1)]
        [TestCase("test13", new byte[] { 255 }, 6, 1)]
        [TestCase("test14", new byte[] { 255 }, 7, 1)]
        [TestCase("test15", new byte[] { 255 }, 0, 0)]
        [TestCase("test16", new byte[] { 255, 1 }, 8, 0)]
        [TestCase("test17", new byte[] { 255, 2 }, 15, 1)]
        [TestCase("test18", new byte[] { 0, 3 }, 15, 1)]
        [TestCase("test19", new byte[] { 0, 0, 255 }, 17, 1)]
        public void ShouldGetProperKeyBit(string label, byte[] subject, int bitNum, byte expected)
        {
            var result = subject.GetKeyBit(bitNum);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(1, 0x01)]
        [TestCase(16, 0xFF)]
        public void ShouldSetEachByteToValue(int byteArrayLength, byte valueToSet)
        {
            var data = new byte[byteArrayLength];
            var result = data.SetEachByteToValue(valueToSet);

            for (var i = 0; i < data.Length; i++)
            {
                Assert.That(result[i], Is.EqualTo(valueToSet), $"{nameof(i)}: {i}");
            }
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 8)]
        [TestCase(8, 64)]
        public void WhenBitLengthGivenByteArrayOfBytesX_ShouldReturnLengthTimesEightOfX(int byteArrayLength, int expectedBits)
        {
            var bytes = new byte[byteArrayLength];
            Assert.That(bytes.BitLength(), Is.EqualTo(expectedBits));
        }

        [Test]
        public void WhenBitLengthGivenNull_ShouldThrowArgumentNullException()
        {
            byte[] bytes = null;

            Assert.Throws<ArgumentNullException>(() => bytes.BitLength());
        }
    }
}
