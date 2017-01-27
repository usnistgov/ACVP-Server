using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture]
    public class TDESKeysTests
    {

        [Test]
        public void ShouldMakeThreeIdenticalKeysIfOnly8BytesSupplied()
        {
            byte[] keyBytes = {0,1,2,3,4,5,6,7};
            var subject = new TDESKeys(new BitString(keyBytes));
            foreach (var bytes in subject.Keys)
            {
                Assert.AreEqual(keyBytes, bytes);
            }
            
        }

        [Test]
        public void ShouldMakeKey1AndKey3IndenticalIf16BytesSupplied()
        {
            byte[] keyBytes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.AreEqual(subject.Keys[0], subject.Keys[2]);
        }

        [Test]
        public void ShouldMakeThreeDifferentKeysIf24BytesSupplied()
        {
            byte[] keyBytes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.AreNotEqual(subject.Keys[0], subject.Keys[1]);
            Assert.AreNotEqual(subject.Keys[0], subject.Keys[2]);
            Assert.AreNotEqual(subject.Keys[1], subject.Keys[2]);
        }

        [Test]
        [TestCase(new byte[] {0,1,2,3,4,5,6,7}, KeyOptionValues.OneKey)]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 }, KeyOptionValues.OneKey)]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 }, KeyOptionValues.OneKey)]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 2, 21, 22, 23, 24, 25, 26, 27 }, KeyOptionValues.TwoKey)]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 2, 21, 22, 23, 24, 25, 26, 27, 0, 1, 2, 3, 4, 5, 6, 7 }, KeyOptionValues.TwoKey)]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 2, 21, 22, 23, 24, 25, 26, 27, 3, 31, 32, 33, 34, 35, 36, 37 }, KeyOptionValues.ThreeKey)]
        public void ShouldHaveProperKeyOptionSetBasedOnKeyEquality(byte[] keyBytes, KeyOptionValues expected)
        {
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.AreEqual(expected, subject.KeyOption);
        }

        [Test]
        public void ShouldHaveDifferentKey1andKey2if16BytesSupplied()
        {
            byte[] keyBytes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.AreNotEqual(subject.Keys[0], subject.Keys[1]);
        }

        [Test]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(24)]
        public void ShouldMakeThreeKeys(int numBytes)
        {
            byte[] keyBytes = new byte[numBytes];
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.AreEqual(3, subject.Keys.Count);

        }

        [Test]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(24)]
        public void ShouldHaveKeys(int numBytes)
        {
            byte[] keyBytes = new byte[numBytes];
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.IsNotNull(subject.Keys);

        }

        [Test]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(24)]
        public void ShouldHaveThreeKeysAsBitStrings(int numBytes)
        {
            byte[] keyBytes = new byte[numBytes];
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.AreEqual(3, subject.KeysAsBitStrings.Count);
        }

        [Test]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(24)]
        public void ShouldHaveKeysAsBitStrings(int numBytes)
        {
            byte[] keyBytes = new byte[numBytes];
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.IsNotNull(subject.KeysAsBitStrings.Count);
        }
    }
}
