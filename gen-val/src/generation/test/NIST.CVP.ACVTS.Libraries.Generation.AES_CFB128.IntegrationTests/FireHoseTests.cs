using System.IO;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.ShiftRegister;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CFB128.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CFB128.v1_0.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CFB128.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        private string _testPath;
        private IModeBlockCipher<SymmetricCipherResult> _algo;
        private MonteCarloAesCfb _mct;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\aes-cfb128\");
            var engine = new AesEngine();
            _algo = new CfbBlockCipher(engine, new ShiftRegisterStrategyFullBlock(engine));
            _mct = new MonteCarloAesCfb(
                new BlockCipherEngineFactory(),
                new ModeBlockCipherFactory(),
                new AesMonteCarloKeyMaker(),
                128,
                BlockCipherModesOfOperation.CfbBlock
            );
        }

        [Test]
        public void ShouldParseAndRunCAVSFiles()
        {
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            LegacyResponseFileParser parser = new LegacyResponseFileParser();
            var parsedTestVectorSet = parser.Parse(_testPath);

            if (!parsedTestVectorSet.Success)
            {
                Assert.Fail("Failed parsing test files");
            }

            if (parsedTestVectorSet.ParsedObject.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            int count = 0;
            int passes = 0;
            int fails = 0;
            bool mctTestHit = false;
            bool nonMctTestHit = false;
            foreach (var iTestGroup in parsedTestVectorSet.ParsedObject.TestGroups)
            {

                var testGroup = (TestGroup)iTestGroup;
                foreach (var iTestCase in testGroup.Tests)
                {
                    count++;

                    var testCase = (TestCase)iTestCase;

                    if (testGroup.TestType.ToLower() == "mct")
                    {
                        mctTestHit = true;

                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            var result = _mct.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                                BlockCipherDirections.Encrypt,
                                testCase.ResultsArray.First().IV.GetDeepCopy(),
                                testCase.ResultsArray.First().Key.GetDeepCopy(),
                                testCase.ResultsArray.First().PlainText.GetDeepCopy()
                            ));

                            Assert.That(testCase.ResultsArray.Count > 0, Is.True, $"{nameof(testCase)} MCT encrypt count should be gt 0");
                            for (int i = 0; i < testCase.ResultsArray.Count; i++)
                            {
                                Assert.That(result.Response[i].IV, Is.EqualTo(testCase.ResultsArray[i].IV), $"IV mismatch on index {i}");
                                Assert.That(result.Response[i].Key, Is.EqualTo(testCase.ResultsArray[i].Key), $"Key mismatch on index {i}");
                                Assert.That(result.Response[i].PlainText, Is.EqualTo(testCase.ResultsArray[i].PlainText), $"PlainText mismatch on index {i}");
                                Assert.That(result.Response[i].CipherText, Is.EqualTo(testCase.ResultsArray[i].CipherText), $"CipherText mismatch on index {i}");
                            }
                            continue;
                        }
                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            var result = _mct.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                                BlockCipherDirections.Decrypt,
                                testCase.ResultsArray.First().IV.GetDeepCopy(),
                                testCase.ResultsArray.First().Key.GetDeepCopy(),
                                testCase.ResultsArray.First().CipherText.GetDeepCopy()
                            ));

                            Assert.That(testCase.ResultsArray.Count > 0, Is.True, $"{nameof(testCase)} MCT decrypt count should be gt 0");
                            for (int i = 0; i < testCase.ResultsArray.Count; i++)
                            {
                                Assert.That(result.Response[i].IV, Is.EqualTo(testCase.ResultsArray[i].IV), $"IV mismatch on index {i}");
                                Assert.That(result.Response[i].Key, Is.EqualTo(testCase.ResultsArray[i].Key), $"Key mismatch on index {i}");
                                Assert.That(result.Response[i].PlainText, Is.EqualTo(testCase.ResultsArray[i].PlainText), $"PlainText mismatch on index {i}");
                                Assert.That(result.Response[i].CipherText, Is.EqualTo(testCase.ResultsArray[i].CipherText), $"CipherText mismatch on index {i}");
                            }
                            continue;
                        }
                    }
                    else
                    {
                        nonMctTestHit = true;

                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            var result = _algo.ProcessPayload(new ModeBlockCipherParameters(
                                BlockCipherDirections.Encrypt,
                                testCase.IV.GetDeepCopy(),
                                testCase.Key.GetDeepCopy(),
                                testCase.PlainText.GetDeepCopy()
                            ));

                            if (testCase.CipherText.ToHex() == result.Result.ToHex())
                                passes++;
                            else
                                fails++;

                            Assert.That(result.Result.ToHex(), Is.EqualTo(testCase.CipherText.ToHex()),
                                $"Failed on count {count} expected CT {testCase.CipherText.ToHex()}, got {result.Result.ToHex()}");
                            continue;
                        }

                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            var result = _algo.ProcessPayload(new ModeBlockCipherParameters(
                                BlockCipherDirections.Decrypt,
                                testCase.IV.GetDeepCopy(),
                                testCase.Key.GetDeepCopy(),
                                testCase.CipherText.GetDeepCopy()
                            ));

                            if (testCase.PlainText.ToHex() == result.Result.ToHex())
                                passes++;
                            else
                                fails++;

                            Assert.That(result.Result.ToHex(), Is.EqualTo(testCase.PlainText.ToHex()),
                                $"Failed on count {count} expected PT {testCase.PlainText.ToHex()}, got {result.Result.ToHex()}");
                            continue;
                        }
                    }

                    Assert.Fail($"{testGroup.Function} did not meet expected function values");
                }
            }

            Assert.That(mctTestHit, Is.True, "No MCT tests were run");
            Assert.That(nonMctTestHit, Is.True, "No normal (non MCT) tests were run");

            // Assert.Fail($"Passes {passes}, fails {fails}, count {count}");
        }
    }
}
