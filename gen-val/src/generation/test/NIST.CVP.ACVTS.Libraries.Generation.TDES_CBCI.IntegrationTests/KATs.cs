using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES.KATs;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CBCI.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CBCI.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class KATs
    {
        private readonly TdesEngine _engine = new TdesEngine();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();

        private ITestCaseGeneratorAsync<TestGroup, TestCase> _katTestCaseGenerator;

        private readonly string[] _katTypes = KatData.GetLabels();

        [Test]
        [TestCase("encrypt")]
        [TestCase("decrypt")]
        public void ShouldPerformAllKATsCorrectly(string direction)
        {
            foreach (var testType in _katTypes)
            {
                var tg = new TestGroup
                {
                    TestType = testType,
                    Function = direction,
                };

                _katTestCaseGenerator = new TestCaseGeneratorKat(testType);

                var tests = new List<TestCase>();
                for (var i = 0; i < _katTestCaseGenerator.NumberOfTestCasesToGenerate; i++)
                {
                    tests.Add(_katTestCaseGenerator.GenerateAsync(tg, false).Result.TestCase);
                }

                foreach (var test in tests)
                {
                    var payload = direction == "encrypt" ? test.PlainText : test.CipherText;
                    var expectedResult = direction == "encrypt" ? test.CipherText : test.PlainText;

                    var algo = _modeFactory.GetStandardCipher(_engine, BlockCipherModesOfOperation.Cbci);
                    var result = algo.ProcessPayload(new ModeBlockCipherParameters(
                        GetDirection(direction), test.Iv, test.Keys, payload
                    ));

                    Assert.IsTrue(result.Success, nameof(result.Success));
                    Assert.AreEqual(expectedResult.ToHex(), result.Result.ToHex(), $"{testType} - {result.Result.ToHex()}");
                }
            }
        }

        private BlockCipherDirections GetDirection(string direction)
        {
            switch (direction)
            {
                case "encrypt":
                    return BlockCipherDirections.Encrypt;
                case "decrypt":
                    return BlockCipherDirections.Decrypt;

                default:
                    throw new Exception();
            }
        }
    }
}
