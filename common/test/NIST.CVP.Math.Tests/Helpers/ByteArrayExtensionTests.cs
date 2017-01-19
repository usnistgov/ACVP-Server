using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using NIST.CVP.Math.Helpers;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests.Helpers
{
    [TestFixture]
    public class ByteArrayExtensionTests
    {
        [Test]
        public void ShouldThrowExceptionIfByteArrayBLongerthanByteArrayA()
        {
            var bArrayA = new byte[] {1};
            var bArrayB = new byte[] {1, 2};
            Assert.Throws(typeof(ArgumentException), () => bArrayA.Add(bArrayB));
        }

        [Test]
        [TestCase(new byte[] { 1 }, new byte[] { 1 }, new byte[] { 2 })]
        [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new byte[] { 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55 }, new byte[] { 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55 })]
        [TestCase(new byte[] { 0, 0, 0, 0, 1, 0, 0, 0 }, new byte[] { 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55 }, new byte[] { 0x55, 0x55, 0x55, 0x55, 0x56, 0x55, 0x55, 0x55 })]
        [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 })]
        [TestCase(new byte[] { 0xff, 0, 0, 0, 0, 0, 0, 0 }, new byte[] { 1, 0, 0, 0, 0, 0, 0, 1 }, new byte[] { 0, 1, 0, 0, 0, 0, 0, 1 })]
        [TestCase(new byte[] {  0, 0, 0, 0xff }, new byte[] { 0, 0, 0, 1 }, new byte[] { 0, 0,0, 0 })]
        public void ShouldAddArraysOfEqualLength(byte[]  bArrayA, byte[] bArrayB, byte[] expected)
        {
            var result = bArrayA.Add(bArrayB);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(new byte[] { 1, 0}, new byte[] { 1 }, new byte[] { 2,0})]
        [TestCase(new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 }, new byte[] { 0x55 }, new byte[] { 0x56, 0, 0, 0, 0, 0, 0, 0 })]
        [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }, new byte[] { 0x55 }, new byte[] { 0x55, 0, 0, 0, 0, 0, 0, 1 })]
        public void ShouldAddArraysWhenByteArrayALongerThanByteArrayB(byte[] bArrayA, byte[] bArrayB, byte[] expected)
        {
            var result = bArrayA.Add(bArrayB);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldThrowExceptionIfByteArrayALongerthanPadLength()
        {
            var bArrayA = new byte[] { 1, 2, 3, 4 };
           
            Assert.Throws(typeof(ArgumentException), () => bArrayA.PadArrayToLength(2));
        }

        [Test]
        [TestCase(new byte[] { 1, 0 }, 3, new byte[] { 1, 0, 0 })]
        [TestCase(new byte[] { 0x56, 0, 0, 0}, 4, new byte[] { 0x56, 0, 0, 0 })]
        [TestCase(new byte[] { 0, 3, 0, 0, 0}, 8, new byte[] { 0, 3, 0, 0, 0, 0, 0, 0 })]
        public void ShouldPadArrayByAdding0BytesToMostSignificantEnd(byte[] bArrayA, int padLength, byte[] expected)
        {
            var result = bArrayA.PadArrayToLength(padLength);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(new byte[] {1, 0}, new byte[] {1,1})]
        [TestCase(new byte[] { 1, 0,0,0 }, new byte[] { 1, 1,1,1 })]
        [TestCase(new byte[] { 5, 0, 0, 0 }, new byte[] { 4, 1, 1, 1 })]
        [TestCase(new byte[] { 4, 1, 1, 1 }, new byte[] { 4, 1, 1, 1 })]
        public void ShouldSetOddParityBit(byte[] bArray, byte[] expected)
        {
            var result = bArray.SetOddParityBitInSuppliedBytes();
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase( new byte[] { 1, 1 })]
        [TestCase(new byte[] { 1, 1, 1, 1 })]
        [TestCase(new byte[] { 4, 1, 1, 1 })]
        public void ShouldHaveOddParityBitInAllBytes(byte[] bArray)
        {
            var result = bArray.AllBytesHaveOddParity();
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase(new byte[] { 1, 0 })]
        [TestCase(new byte[] { 1, 0, 0, 0})]
        [TestCase(new byte[] { 5, 0, 0, 0 })]
        public void ShouldNotHaveOddParityBitInAllBytes(byte[] bArray)
        {
            var result = bArray.AllBytesHaveOddParity();
            Assert.IsFalse(result);
        }
    }
}
