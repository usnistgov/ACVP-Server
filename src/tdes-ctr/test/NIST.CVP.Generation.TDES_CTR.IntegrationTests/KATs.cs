using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_CTR.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class KATs
    {
        private readonly TdesEngine _engine = new TdesEngine();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();
        private readonly CounterFactory _counterFactory = new CounterFactory();

        private ITestCaseGeneratorAsync<TestGroup, TestCase> _katTestCaseGenerator;

        private readonly string[] _katTypes = { "permutation", "substitutiontable", "variablekey", "variabletext", "inversepermutation"};
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
                    Assert.AreEqual(test.PlainText, result.Result, $"{testType} - {test.PlainText.ToHex()}");
                }
            }
        }
    }
}
