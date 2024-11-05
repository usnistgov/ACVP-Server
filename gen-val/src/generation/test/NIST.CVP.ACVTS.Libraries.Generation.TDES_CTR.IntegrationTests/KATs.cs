using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTR;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class KATs
    {
        private readonly TdesEngine _engine = new TdesEngine();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();
        private readonly CounterFactory _counterFactory = new CounterFactory();

        private ITestCaseGeneratorAsync<TestGroup, TestCase> _katTestCaseGenerator;

        private readonly string[] _katTypes = { "permutation", "substitutiontable", "variablekey", "variabletext", "inversepermutation" };
        private readonly string _encryptLabel = "encrypt";

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

                _katTestCaseGenerator = new TestCaseGeneratorKat(testType);

                List<TestCase> tests = new List<TestCase>();
                for (int i = 0; i < _katTestCaseGenerator.NumberOfTestCasesToGenerate; i++)
                {
                    tests.Add(_katTestCaseGenerator.GenerateAsync(tg, false).Result.TestCase);
                }

                foreach (var test in tests)
                {
                    var counter = _counterFactory.GetCounter(_engine, CounterTypes.Additive, test.Iv);
                    var algo = _modeFactory.GetCounterCipher(_engine, counter);
                    var result = algo.ProcessPayload(new ModeBlockCipherParameters(
                        BlockCipherDirections.Encrypt, test.Key, test.PlainText
                    ));

                    Assert.That(result.Success, Is.True, nameof(result.Success));
                    Assert.That(result.Result, Is.EqualTo(test.CipherText), test.CipherText.ToHex());
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

                _katTestCaseGenerator = new TestCaseGeneratorKat(testType);

                List<TestCase> tests = new List<TestCase>();
                for (int i = 0; i < _katTestCaseGenerator.NumberOfTestCasesToGenerate; i++)
                {
                    tests.Add(_katTestCaseGenerator.GenerateAsync(tg, false).Result.TestCase);
                }

                foreach (var test in tests)
                {
                    var counter = _counterFactory.GetCounter(_engine, CounterTypes.Additive, test.Iv);
                    var algo = _modeFactory.GetCounterCipher(_engine, counter);
                    var result = algo.ProcessPayload(new ModeBlockCipherParameters(
                        BlockCipherDirections.Decrypt, test.Key, test.CipherText
                    ));
                    Assert.That(result.Result, Is.EqualTo(test.PlainText), $"{testType} - {test.PlainText.ToHex()}");
                }
            }
        }
    }
}
