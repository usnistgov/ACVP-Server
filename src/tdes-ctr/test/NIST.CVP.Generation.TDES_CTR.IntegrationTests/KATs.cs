using NIST.CVP.Crypto.TDES_CTR;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CTR.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class KATs
    {
        private readonly ITdesCtr _subject = new TdesCtr();
        private ITestCaseGenerator<TestGroup, TestCase> _katTestCaseGenerator;

        private readonly string[] _katTypes = { "permutation", "substitutiontable", "variablekey", "variabletext", "inversepermutation"};
        private readonly string _encryptLabel = "encrypt";
        private readonly string _decryptLabel = "decrypt";

        [Test]
        public void ShouldPerformAllEncryptKATsCorrectly()
        {
            foreach (var testType in _katTypes)
            {
                TestGroup tg = new TestGroup()
                {
                    TestType = testType,
                    Direction = _encryptLabel
                };

                _katTestCaseGenerator = new TestCaseGeneratorKnownAnswer(tg);

                List<TestCase> tests = new List<TestCase>();
                for (int i = 0; i < _katTestCaseGenerator.NumberOfTestCasesToGenerate; i++)
                {
                    tests.Add((TestCase)_katTestCaseGenerator.Generate(tg, false).TestCase);
                }

                foreach (var test in tests)
                {
                    var result = _subject.EncryptBlock(test.Key, test.PlainText, test.Iv);

                    Assert.IsTrue(result.Success, nameof(result.Success));
                    Assert.AreEqual(test.CipherText, result.Result, test.CipherText.ToHex());
                }
            }
        }

        [Test]
        public void ShouldPerformAllDecryptKATsCorrectly()
        {
            foreach (var testType in _katTypes)
            {
                TestGroup tg = new TestGroup()
                {
                    TestType = testType,
                    Direction = _encryptLabel
                };

                _katTestCaseGenerator = new TestCaseGeneratorKnownAnswer(tg);

                List<TestCase> tests = new List<TestCase>();
                for (int i = 0; i < _katTestCaseGenerator.NumberOfTestCasesToGenerate; i++)
                {
                    tests.Add((TestCase)_katTestCaseGenerator.Generate(tg, false).TestCase);
                }

                foreach (var test in tests)
                {
                    var result = _subject.DecryptBlock(test.Key, test.CipherText, test.Iv);

                    Assert.IsTrue(result.Success, nameof(result.Success));
                    Assert.AreEqual(test.PlainText, result.Result, test.PlainText.ToHex());
                }
            }
        }
    }
}
