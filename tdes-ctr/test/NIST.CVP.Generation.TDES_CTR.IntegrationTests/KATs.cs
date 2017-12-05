using NIST.CVP.Crypto.TDES_CTR;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CTR.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class KATs
    {
        private readonly ITdesCtr _subject = new TdesCtr();
        private readonly IKnownAnswerTestFactory _katFactory = new KnownAnswerTestFactory();

        private readonly string[] _katTypes = { "permutation", "substitutiontable", "variablekey", "variabletext", "inversepermutation"};
        private readonly string _encryptLabel = "encrypt";
        private readonly string _decryptLabel = "decrypt";

        [Test]
        public void ShouldPerformAllEncryptKATsCorrectly()
        {
            foreach (var testType in _katTypes)
            {
                var tests = _katFactory.GetKATTestCases(testType, _encryptLabel);
                foreach (var test in tests)
                {
                    var result = _subject.EncryptBlock(test.Key, test.PlainText, test.Iv);

                    Assert.IsTrue(result.Success, nameof(result.Success));
                    Assert.AreEqual(test.CipherText, result.CipherText, test.CipherText.ToHex());
                }
            }
        }

        [Test]
        public void ShouldPerformAllDecryptKATsCorrectly()
        {
            foreach (var testType in _katTypes)
            {
                var tests = _katFactory.GetKATTestCases(testType, _decryptLabel);
                foreach (var test in tests)
                {
                    var result = _subject.DecryptBlock(test.Key, test.CipherText, test.Iv);

                    Assert.IsTrue(result.Success, nameof(result.Success));
                    Assert.AreEqual(test.PlainText, result.PlainText, test.PlainText.ToHex());
                }
            }
        }
    }
}
