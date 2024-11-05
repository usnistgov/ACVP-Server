using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Tests.Tdes
{
    [TestFixture, FastCryptoTest]
    public class TDESKeysTests
    {

        [Test]
        public void ShouldMakeThreeIdenticalKeysIfOnly8BytesSupplied()
        {
            byte[] keyBytes = { 0, 1, 2, 3, 4, 5, 6, 7 };
            var subject = new TDESKeys(new BitString(keyBytes));
            foreach (var bytes in subject.Keys)
            {
                Assert.That(bytes, Is.EqualTo(keyBytes));
            }

        }

        [Test]
        public void ShouldMakeKey1AndKey3IndenticalIf16BytesSupplied()
        {
            byte[] keyBytes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.That(subject.Keys[2], Is.EqualTo(subject.Keys[0]));
        }

        [Test]
        public void ShouldMakeThreeDifferentKeysIf24BytesSupplied()
        {
            byte[] keyBytes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.That(subject.Keys[1], Is.Not.EqualTo(subject.Keys[0]));
            Assert.That(subject.Keys[2], Is.Not.EqualTo(subject.Keys[0]));
            Assert.That(subject.Keys[2], Is.Not.EqualTo(subject.Keys[1]));
        }

        [Test]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }, KeyOptionValues.OneKey)]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 }, KeyOptionValues.OneKey)]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 }, KeyOptionValues.OneKey)]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 2, 21, 22, 23, 24, 25, 26, 27 }, KeyOptionValues.TwoKey)]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 2, 21, 22, 23, 24, 25, 26, 27, 0, 1, 2, 3, 4, 5, 6, 7 }, KeyOptionValues.TwoKey)]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 2, 21, 22, 23, 24, 25, 26, 27, 3, 31, 32, 33, 34, 35, 36, 37 }, KeyOptionValues.ThreeKey)]
        public void ShouldHaveProperKeyOptionSetBasedOnKeyEquality(byte[] keyBytes, KeyOptionValues expected)
        {
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.That(subject.KeyOption, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldHaveDifferentKey1andKey2if16BytesSupplied()
        {
            byte[] keyBytes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.That(subject.Keys[1], Is.Not.EqualTo(subject.Keys[0]));
        }

        [Test]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(24)]
        public void ShouldMakeThreeKeys(int numBytes)
        {
            byte[] keyBytes = new byte[numBytes];
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.That(subject.Keys.Count, Is.EqualTo(3));

        }

        [Test]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(24)]
        public void ShouldHaveKeys(int numBytes)
        {
            byte[] keyBytes = new byte[numBytes];
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.That(subject.Keys, Is.Not.Null);

        }

        [Test]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(24)]
        public void ShouldHaveThreeKeysAsBitStrings(int numBytes)
        {
            byte[] keyBytes = new byte[numBytes];
            var subject = new TDESKeys(new BitString(keyBytes));
            Assert.That(subject.KeysAsBitStrings.Count, Is.EqualTo(3));
        }

        [Test]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(24)]
        public void ShouldHaveKeysAsBitStrings(int numBytes)
        {
            byte[] keyBytes = new byte[numBytes];
            var subject = new TDESKeys(new BitString(keyBytes));
            //Assert.That(subject.KeysAsBitStrings.Count, Is.Not.Null);
            Assert.That(subject.KeysAsBitStrings.Count, Is.GreaterThan(0));
        }
    }
}
