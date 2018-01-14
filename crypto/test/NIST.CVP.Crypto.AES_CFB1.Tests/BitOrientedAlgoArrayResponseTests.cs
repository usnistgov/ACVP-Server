using System.Collections.Generic;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_CFB1.Tests
{
    [TestFixture,  FastCryptoTest]
    public class BitOrientedAlgoArrayResponseTests
    {
        [Test]
        public void ShouldGetDerivedFromBase()
        {
            AlgoArrayResponse original = new AlgoArrayResponse();
            original.PlainText = new BitString(1);
            original.CipherText = new BitString(2);
            original.IV = new BitString(3);
            original.Key = new BitString(4);

            var subject = BitOrientedAlgoArrayResponse.GetDerivedFromBase(original);

            Assert.AreEqual(original.PlainText.Bits, subject.PlainText.Bits, nameof(original.PlainText));
            Assert.AreEqual(original.CipherText.Bits, subject.CipherText.Bits, nameof(original.CipherText));
            Assert.AreEqual(original.IV.Bits, subject.IV.Bits, nameof(original.IV));
            Assert.AreEqual(original.Key.Bits, subject.Key.Bits, nameof(original.Key));
        }

        [Test]
        public void ShouldGetDerivedFromBaseCollection()
        {
            AlgoArrayResponse original = new AlgoArrayResponse();
            original.PlainText = new BitString(1);
            original.CipherText = new BitString(2);
            original.IV = new BitString(3);
            original.Key = new BitString(4);

            List<AlgoArrayResponse> originals = new List<AlgoArrayResponse>();
            originals.Add(original);

            var subject = BitOrientedAlgoArrayResponse.GetDerivedFromBase(originals);

            Assert.AreEqual(original.PlainText.Bits, subject[0].PlainText.Bits, nameof(original.PlainText));
            Assert.AreEqual(original.CipherText.Bits, subject[0].CipherText.Bits, nameof(original.CipherText));
            Assert.AreEqual(original.IV.Bits, subject[0].IV.Bits, nameof(original.IV));
            Assert.AreEqual(original.Key.Bits, subject[0].Key.Bits, nameof(original.Key));
        }
    }
}
