using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES.KATs;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.TDES_CFBP.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_CFBP.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class KATs
    {
        private readonly TdesEngine _engine = new TdesEngine();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();

        private ITestCaseGeneratorAsync<TestGroup, TestCase> _katTestCaseGenerator;

        private readonly string[] _katTypes = KatData.GetLabels();

        [Test]
        [TestCase("encrypt", AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase("decrypt", AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase("encrypt", AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase("decrypt", AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase("encrypt", AlgoMode.TDES_CFBP64_v1_0)]
        [TestCase("decrypt", AlgoMode.TDES_CFBP64_v1_0)]
        public void ShouldPerformAllKATsCorrectly(string direction, AlgoMode mode)
        {
            foreach (var testType in _katTypes)
            {
                var tg = new TestGroup
                {
                    TestType = testType,
                    Function = direction,
                    AlgoMode = mode
                };

                _katTestCaseGenerator = new TestCaseGeneratorKat(testType, mode);

                var tests = new List<TestCase>();
                for (var i = 0; i < _katTestCaseGenerator.NumberOfTestCasesToGenerate; i++)
                {
                    tests.Add(_katTestCaseGenerator.GenerateAsync(tg, false).Result.TestCase);
                }

                foreach (var test in tests)
                {
                    var payload = direction == "encrypt" ? test.PlainText : test.CipherText;
                    var expectedResult = direction == "encrypt" ? test.CipherText : test.PlainText;

                    var algo = _modeFactory.GetStandardCipher(_engine, GetModeOfOperation(mode));
                    var result = algo.ProcessPayload(new ModeBlockCipherParameters(
                        GetDirection(direction), test.IV, test.Keys, payload
                    ));

                    Assert.IsTrue(result.Success, nameof(result.Success));
                    Assert.AreEqual(expectedResult.ToHex(), result.Result.ToHex());
                }
            }
        }

        private BlockCipherModesOfOperation GetModeOfOperation(AlgoMode mode)
        {
            switch (mode)
            {
                case AlgoMode.TDES_CFBP1_v1_0:
                    return BlockCipherModesOfOperation.CfbpBit;
                case AlgoMode.TDES_CFBP8_v1_0:
                    return BlockCipherModesOfOperation.CfbpByte;
                case AlgoMode.TDES_CFBP64_v1_0:
                    return BlockCipherModesOfOperation.CfbpBlock;

                default:
                    throw new Exception();
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
