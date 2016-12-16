using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections;

namespace NIST.CVP.Math.Tests
{
    [TestFixture]
    public class Random800_90Tests
    {
        /// <summary>
        /// Test implementation of <see cref="Random800_90"/>.  
        /// Used to ensure while block within GetDifferentBitStringOfSameSize is hit.
        /// </summary>
        private class TestRandom800_90 : Random800_90
        {
            private readonly BitString _currentBitString;

            public bool HasReturnedCurrentBitString { get; private set; } = false;

            public TestRandom800_90(BitString currentBitString)
            {
                _currentBitString = currentBitString;
            }

            /// <summary>
            /// Returns the <see cref="_currentBitString"/> the first time invoked, and a different <see cref="BitString"/> the second time.
            /// </summary>
            /// <param name="length">The length of the bitstring</param>
            /// <returns>BitString</returns>
            public override BitString GetRandomBitString(int length)
            {
                if (!HasReturnedCurrentBitString)
                {
                    HasReturnedCurrentBitString = true;
                    return _currentBitString;
                }

                // XORable bit array of length
                BitString bs = new BitString(length);
                bs.Bits.SetAll(true);

                // Get the XOR of the current bit string after it is returned as is a single time.
                return bs.XOR(_currentBitString);
            }
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void ShouldReturnZeroLngthBitStringForZeroOrLessLengths(int length)
        {
            var subject = new Random800_90();
            var result = subject.GetRandomBitString(length);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.BitLength);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(16)]
        public void ShouldReturnSelectedLength(int length)
        {
            var subject = new Random800_90();
            var result = subject.GetRandomBitString(length);
            Assume.That(result != null);
            Assert.AreEqual(length, result.BitLength);
        }

        [Test]
        public void ShouldReturnNullWhenCurrentIsNull()
        {
            var subject = new Random800_90();
            var result = subject.GetDifferentBitStringOfSameSize(null);

            Assert.IsNull(result);
        }

        [Test]
        public void ShouldReturnNullWhenCurrentIsZeroLength()
        {
            var subject = new Random800_90();
            var zeroLengthBitString = new BitString(0);
            var result = subject.GetDifferentBitStringOfSameSize(zeroLengthBitString);

            Assert.IsNull(result);
        }

        [Test]
        [TestCase(new bool[] { true })]
        [TestCase(new bool[] { true, false, true })]
        [TestCase(new bool[] { true, true, true })]
        [TestCase(new bool[] { false, false, true, true })]
        public void ShouldReturnDifferentBitStringWhenInvoked(bool[] bits)
        {
            var bs = new BitString(new BitArray(bits));
            TestRandom800_90 sut = new TestRandom800_90(bs);
            var result = sut.GetDifferentBitStringOfSameSize(bs);

            Assert.AreNotEqual(bs, result);
        }

        [Test]
        [TestCase(0, 1, 100)]
        public void ShouldReturnNothingOutsideOfRange(int min, int max, int iterations)
        {
            var subject = new Random800_90();

            for (int i = 0; i < iterations; i++)
            {
                var result = subject.GetRandomInt(min, max);
                Assert.IsTrue(result >= 0 && result <= 1);
            }
        }

    }
}
