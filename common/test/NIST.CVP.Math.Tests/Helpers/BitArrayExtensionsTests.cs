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
    }
}
