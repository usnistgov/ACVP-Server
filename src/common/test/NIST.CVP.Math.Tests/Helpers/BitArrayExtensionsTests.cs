using System;
using System.Collections;
using System.Linq;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests.Helpers
{
    [TestFixture, UnitTest]
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
        [Test]
        [TestCase("100111", 0, 1, "1")]
        [TestCase("100111", 0, 2, "10")]
        [TestCase("100111", 0, 3, "100")]
        [TestCase("100111", 0, 4, "1001")]
        [TestCase("100111", 0, 5, "10011")]
        [TestCase("100111", 0, 6, "100111")]

        [TestCase("100111", 1, 2, "00")]
        [TestCase("100111", 1, 3, "001")]
        [TestCase("100111", 1, 4, "0011")]
        [TestCase("100111", 1, 5, "00111")]

        [TestCase("100111", 3, 1, "1")]
        [TestCase("100111", 3, 2, "11")]
        [TestCase("100111", 3, 3, "111")]

        [TestCase("100111", 5, 1, "1")]
        public void ShouldCreateCorrectSubarray(string inArrayStr, int startIndex, int length, string outArrayStr)
        {
            //might be overkill to have this much checking on a unit test
            var inArray = new BitArray(inArrayStr.Select(x => x == '1' || x == '0' ? //make sure only 0 or 1 are in the string
                                          x == '1' :  
                                          throw new InvalidCastException()).ToArray());
            var outArray = new BitArray(outArrayStr.Select(x => x == '1' || x == '0' ?
                                          x == '1' :
                                          throw new InvalidCastException()).ToArray());
            var subArray = inArray.SubArray(startIndex, length);
            Assert.AreEqual(subArray, outArray);
        }
    }
}
