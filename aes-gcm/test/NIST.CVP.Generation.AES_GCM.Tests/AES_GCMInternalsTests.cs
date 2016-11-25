using NIST.CVP.Generation.AES;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture]
    public class AES_GCMInternalsTests
    {
        AES_GCMInternals _sut = new AES_GCMInternals(
            new RijndaelFactory(
                new RijndaelInternals()
            )
        );

        #region GHash
        [Test]
        [TestCase(128, false)]
        [TestCase(127, true)]
        [TestCase(129, true)]
        [TestCase(0, true)]
        public void GHashShouldReturnNullWhenHBitLengthNot128(int bitLength, bool shouldBeNull)
        {
            BitString h = new BitString(bitLength);
            BitString x = new BitString(128);

            var result = _sut.GHash(h, x);

            Assert.IsTrue(shouldBeNull ? result == null : result != null);
        }

        [Test]
        [TestCase(128, false)]
        [TestCase(256, false)]
        [TestCase(0, true)]
        [TestCase(127, true)]
        [TestCase(129, true)]
        public void GHashShouldReturnNullWhenXBitLengthNot0WhenModulod128(int bitLength, bool shouldBeNull)
        {
            BitString h = new BitString(128);
            BitString x = new BitString(bitLength);

            var result = _sut.GHash(h, x);

            Assert.IsTrue(shouldBeNull ? result == null : result != null);
        }
        #endregion GHash

        #region GCTR
        public void GCTRShouldReturnNullWhenIcbBitLengthNot128()
        {
            BitString k = new BitString(128);
            BitString icb = new BitString(127);
            BitString x = new BitString(128);
            Key key = new Key();

            var result = _sut.GCTR(k, icb, x, key);

            Assert.IsNull(result);
        }

        [Test]
        public void GCTRShouldReturnZeroLengthBitStringWhenXIsZeroLength()
        {
            BitString k = new BitString(128);
            BitString icb = new BitString(128);
            BitString x = new BitString(0);
            Key key = new Key();

            BitString expectation = new BitString(0);

            var result = _sut.GCTR(k, icb, x, key);

            Assert.AreEqual(expectation, result);
        }
        #endregion GCTR

        #region inc_s
        [Test]
        public void inc_sShouldReturnNullWhenBitStringLengthLtS()
        {
            BitString bs = new BitString(128);
            int s = 129;

            var result = _sut.inc_s(s, bs);

            Assert.IsNull(result);
        }
        #endregion inc_s

        #region LSB
        [Test]
        [TestCase(new bool[] { false, false, false, false, true, true, true, true }, 
            4,
            new bool[] { false, false, false, false }
        )]
        [TestCase(new bool[] { false, true, false, false, true, true, true, true },
            3,
            new bool[] { false, true, false }
        )]
        public void ShouldReturnLeastSignificantBits(bool[] bitsInLSb, int numberOfBits, bool[] expectedBits)
        {
            BitString bs = new BitString(new BitArray(bitsInLSb));
            BitString expectation = new BitString(new BitArray(expectedBits));
            var result = _sut.LSB(numberOfBits, bs);

            Assert.AreEqual(expectation, result);
        }
        #endregion LSB

        #region MSB
        [Test]
        [TestCase(new bool[] { false, false, false, false, true, true, true, true },
            4,
            new bool[] { true, true, true, true }
        )]
        [TestCase(new bool[] { false, true, false, false, true, false, true, true },
            3,
            new bool[] { false, true, true }
        )]
        public void ShouldReturnMostSignificantBits(bool[] bitsInLSb, int numberOfBits, bool[] expectedBits)
        {
            BitString bs = new BitString(new BitArray(bitsInLSb));
            BitString expectation = new BitString(new BitArray(expectedBits));
            var result = _sut.MSB(numberOfBits, bs);

            Assert.AreEqual(expectation, result);
        }
        #endregion MSB

        #region BlockProduct
        [Test]
        [TestCase(0, 0, true)]
        [TestCase(128, 127, true)]
        [TestCase(127, 128, true)]
        [TestCase(128, 128, false)]
        public void BlockProductShouldReturnNullIfXOrYBitLengthNot128(int xLength, int yLength, bool shouldBeNull)
        {
            BitString x = new BitString(xLength);
            BitString y = new BitString(yLength);

            var result = _sut.BlockProduct(x, y);

            Assert.IsTrue(shouldBeNull ? result == null : result != null);
        }

        [Test]
        public void ShouldReturnZeroBitStringWhenXZeroBitStringsProvided()
        {
            BigInteger expectation = new BigInteger(0);

            BitString x = new BitString(128);
            BitString y = x.XOR(x);
            var result = _sut.BlockProduct(x, y);

            Assert.AreEqual(expectation, result.ToBigInteger());
        }
        #endregion BlockProduct
    }
}
