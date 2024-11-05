using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES.KATs;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CFB.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CFB.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class KATs
    {
        private readonly TdesEngine _engine = new TdesEngine();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();

        private ITestCaseGeneratorAsync<TestGroup, TestCase> _katTestCaseGenerator;

        private readonly string[] _katTypes = KatData.GetLabels();

        [Test]
        [TestCase("encrypt", AlgoMode.TDES_CFB1_v1_0)]
        [TestCase("decrypt", AlgoMode.TDES_CFB1_v1_0)]
        [TestCase("encrypt", AlgoMode.TDES_CFB8_v1_0)]
        [TestCase("decrypt", AlgoMode.TDES_CFB8_v1_0)]
        [TestCase("encrypt", AlgoMode.TDES_CFB64_v1_0)]
        [TestCase("decrypt", AlgoMode.TDES_CFB64_v1_0)]
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
                        GetDirection(direction), test.Iv, test.Keys, payload
                    ));

                    Assert.That(result.Success, Is.True, nameof(result.Success));
                    Assert.That(result.Result.ToHex(), Is.EqualTo(expectedResult.ToHex()));
                }
            }
        }

        private BlockCipherModesOfOperation GetModeOfOperation(AlgoMode mode)
        {
            switch (mode)
            {
                case AlgoMode.TDES_CFB1_v1_0:
                    return BlockCipherModesOfOperation.CfbBit;
                case AlgoMode.TDES_CFB8_v1_0:
                    return BlockCipherModesOfOperation.CfbByte;
                case AlgoMode.TDES_CFB64_v1_0:
                    return BlockCipherModesOfOperation.CfbBlock;

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
