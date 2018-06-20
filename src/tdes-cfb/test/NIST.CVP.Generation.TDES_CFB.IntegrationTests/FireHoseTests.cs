using System;
using System.IO;
using System.Linq;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Generation.TDES_CFB.Parsers;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFB.IntegrationTests
{
    public class FireHoseTests
    {
        private TdesMonteCarloFactory _mctFactory;
        private readonly BlockCipherEngineFactory _engineFactory = new BlockCipherEngineFactory();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mctFactory = new TdesMonteCarloFactory(_engineFactory, _modeFactory);
        }

        [Test]
        [TestCase(BlockCipherModesOfOperation.CfbBit)]
        [TestCase(BlockCipherModesOfOperation.CfbByte)]
        [TestCase(BlockCipherModesOfOperation.CfbBlock)]
        public void ShouldParseAndRunCAVSFiles(BlockCipherModesOfOperation mode)
        {
            var mct = _mctFactory.GetInstance(mode);
            var engine = _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes);
            var algo = _modeFactory.GetStandardCipher(engine, mode);

            var testPath = string.Empty;
            switch (mode)
            {
                case BlockCipherModesOfOperation.CfbBit:
                    testPath = Utilities.GetConsistentTestingStartPath(
                        GetType(), 
                        $@"..\..\TestFiles\LegacyParserFiles\tdes-cfb1"
                    );
                    break;
                case BlockCipherModesOfOperation.CfbByte:
                    testPath = Utilities.GetConsistentTestingStartPath(
                        GetType(),
                        $@"..\..\TestFiles\LegacyParserFiles\tdes-cfb8"
                    );
                    break;
                case BlockCipherModesOfOperation.CfbBlock:
                    testPath = Utilities.GetConsistentTestingStartPath(
                        GetType(),
                        $@"..\..\TestFiles\LegacyParserFiles\tdes-cfb64"
                    );
                    break;
                default:
                    throw new ArgumentException(nameof(mode));
            }

            
            if (!Directory.Exists(testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            LegacyResponseFileParser parser = new LegacyResponseFileParser();
            var parsedTestVectorSet = parser.Parse(testPath);

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
            foreach (var testGroup in parsedTestVectorSet.ParsedObject.TestGroups)
            {
                foreach (var testCase in testGroup.Tests)
                {
                    count++;

                    if (testGroup.TestType.ToLower() == "mct")
                    {
                        mctTestHit = true;

                        var firstResult = testCase.ResultsArray.First();
                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            var param = new ModeBlockCipherParameters(
                                BlockCipherDirections.Encrypt,
                                firstResult.IV.GetDeepCopy(),
                                firstResult.Keys.GetDeepCopy(),
                                firstResult.PlainText.GetDeepCopy()
                            );
                            
                            var result = mct.ProcessMonteCarloTest(param);

                            Assert.IsTrue(testCase.ResultsArray.Count > 0, $"{nameof(testCase)} MCT encrypt count should be gt 0");
                            for (int i = 0; i < testCase.ResultsArray.Count; i++)
                            {
                                Assert.AreEqual(testCase.ResultsArray[i].IV, result.Response[i].IV, $"IV mismatch on index {i}");
                                Assert.AreEqual(testCase.ResultsArray[i].Keys, result.Response[i].Keys, $"Key mismatch on index {i}");
                                Assert.AreEqual(testCase.ResultsArray[i].PlainText, result.Response[i].PlainText, $"PlainText mismatch on index {i}");
                                Assert.AreEqual(testCase.ResultsArray[i].CipherText, result.Response[i].CipherText, $"CipherText mismatch on index {i}");
                            }
                            continue;
                        }
                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            var param = new ModeBlockCipherParameters(
                                BlockCipherDirections.Decrypt,
                                firstResult.IV.GetDeepCopy(),
                                firstResult.Keys.GetDeepCopy(),
                                firstResult.CipherText.GetDeepCopy()
                            );

                            var result = mct.ProcessMonteCarloTest(param);

                            Assert.IsTrue(testCase.ResultsArray.Count > 0, $"{nameof(testCase)} MCT decrypt count should be gt 0");
                            Assert.IsTrue(testCase.ResultsArray.Count == result.Response.Count, "Result and response arrays must be of the same size.");
                            for (int i = 0; i < testCase.ResultsArray.Count; i++)
                            {
                                Assert.AreEqual(testCase.ResultsArray[i].IV, result.Response[i].IV, $"IV mismatch on index {i}");
                                Assert.AreEqual(testCase.ResultsArray[i].Keys, result.Response[i].Keys, $"Key mismatch on index {i}");
                                Assert.AreEqual(testCase.ResultsArray[i].PlainText, result.Response[i].PlainText, $"PlainText mismatch on index {i}");
                                Assert.AreEqual(testCase.ResultsArray[i].CipherText, result.Response[i].CipherText, $"CipherText mismatch on index {i}");
                            }
                            continue;
                        }
                    }

                    else
                    {
                        nonMctTestHit = true;

                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            var param = new ModeBlockCipherParameters(
                                BlockCipherDirections.Encrypt,
                                testCase.Iv.GetDeepCopy(),
                                testCase.Keys.GetDeepCopy(),
                                testCase.PlainText.GetDeepCopy()
                            );

                            var result = algo.ProcessPayload(param);

                            if (testCase.CipherText.ToHex() == result.Result.ToHex())
                                passes++;
                            else
                                fails++;

                            Assert.AreEqual(testCase.CipherText.ToHex(), result.Result.ToHex(),
                                $"Failed on count {count} expected CT {testCase.CipherText.ToHex()}, got {result.Result.ToHex()}");
                            continue;
                        }

                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            var param = new ModeBlockCipherParameters(
                                BlockCipherDirections.Decrypt,
                                testCase.Iv.GetDeepCopy(),
                                testCase.Keys.GetDeepCopy(),
                                testCase.CipherText.GetDeepCopy()
                            );

                            var result = algo.ProcessPayload(param);

                            if (testCase.PlainText.ToHex() == result.Result.ToHex())
                                passes++;
                            else
                                fails++;

                            Assert.AreEqual(testCase.PlainText.ToHex(), result.Result.ToHex(),
                                $"Failed on {testGroup.Function}-{testGroup.TestType}-{testGroup.KeyingOption} count {count} expected PT {testCase.PlainText.ToHex()}, got {result.Result.ToHex()}");
                            continue;
                        }
                    }

                    Assert.Fail($"{testGroup.Function} did not meet expected function values");
                }
            }

            Assert.IsTrue(mctTestHit, "No MCT tests were run");
            Assert.IsTrue(nonMctTestHit, "No normal (non MCT) tests were run");
            // Assert.Fail($"Passes {passes}, fails {fails}, count {count}");
        }
    }
}
