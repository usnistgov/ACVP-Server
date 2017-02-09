using System;
using System.Collections;

using NIST.CVP.Math.Helpers;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests.Helpers
{
    [TestFixture]
    public class BitArrayExtensionsTests
    {
        [Test]
        public void ShouldShiftBitsLeft()
        {
            var array1 = new BitArray(new bool[] {true, false, false, false});
            var result = array1.BitShiftLeft();
            Assert.AreEqual(new BitArray(new bool[] { false, true, false, false }), result );
        }

        [Test]
        public void ShouldShiftBitsRight()
        {
            var array1 = new BitArray(new bool[] { true, false, false, false });
            var result = array1.BitShiftRight();
            Assert.AreEqual(new BitArray(new bool[] { false, false, false, false }), result);
        }

        //LSB ----------------> MSB
        [Test]
        [TestCase("0", new byte[] { 0 })]
        [TestCase("00000000", new byte[] { 0 })]
        [TestCase("000000000", new byte[] { 0, 0 })]
        [TestCase("1", new byte[] {1})]
        [TestCase("01", new byte[] { 2 })]
        [TestCase("11", new byte[] { 3 })]
        [TestCase("011", new byte[] { 6 })]
        [TestCase("00000001", new byte[] { 128 })]
        [TestCase("000000010", new byte[] { 128,0 })]
        [TestCase("000000011", new byte[] { 128, 1 })]
        public void ShouldBuildProperByteArrayFromBits(string bitRep, byte[] expected)
        {
            var subject = new BitArray(bitRep.Length);
            for (int idx = 0; idx < bitRep.Length; idx++)
            {
                subject[idx] = bitRep[idx] == '1';
            }
            var result = subject.ToBytes();
            Assert.AreEqual(expected,result);
        }
    }
}
