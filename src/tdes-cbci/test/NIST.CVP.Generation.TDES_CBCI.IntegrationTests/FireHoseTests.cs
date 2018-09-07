using NIST.CVP.Generation.TDES_CBCI.Parsers;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_CBC;
using MonteCarloKeyMaker = NIST.CVP.Crypto.TDES_CBCI.MonteCarloKeyMaker;

namespace NIST.CVP.Generation.TDES_CBCI.IntegrationTests
{
    public class FireHoseTests
    {
        private string _testPath;
        private CbciBlockCipher _algo;
        private MonteCarloTdesCbci _algoMct;
        private MonteCarloKeyMaker _keyMaker;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\");
            _algo = new CbciBlockCipher(new TdesEngine());
            _keyMaker = new MonteCarloKeyMaker();
            _algoMct = new MonteCarloTdesCbci(
                new BlockCipherEngineFactory(),
                new ModeBlockCipherFactory(),
                _keyMaker
            );
        }

        [Test]
        public void ShouldParseAndRunCAVSFiles()
        {
            var _testPath = Utilities.GetConsistentTestingStartPath(GetType(), $@"..\..\TestFiles\LegacyParserFiles\");
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            var parser = new LegacyResponseFileParser();
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
            foreach (var testGroup in parsedTestVectorSet.ParsedObject.TestGroups)
            {
                foreach (var testCase in testGroup.Tests)
                {
                    count++;
                    if (testGroup.TestType.ToLower() == "mct")
                    {
                        mctTestHit = true;
                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            var param = new ModeBlockCipherParameters(
                                BlockCipherDirections.Encrypt,
                                testCase.ResultsArray.First().IV,
                                testCase.ResultsArray.First().Keys,
                                testCase.ResultsArray.First().PlainText
                            );
                            var result = _algoMct.ProcessMonteCarloTest(param);

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
                                testCase.ResultsArray.First().IV,
                                testCase.ResultsArray.First().Keys,
                                testCase.ResultsArray.First().CipherText
                            );
                            var result = _algoMct.ProcessMonteCarloTest(param);

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
                            if (testGroup.TestType.ToLower() == "inversepermutation")
                            {
                                var param = new ModeBlockCipherParameters(
                                    BlockCipherDirections.Encrypt,
                                    testCase.Iv.GetDeepCopy(),
                                    testCase.Keys.GetDeepCopy(),
                                    testCase.PlainText.GetDeepCopy()
                                );
                                var result = _algo.ProcessPayload(param);

                                if (testCase.CipherText.ToString() == result.Result.ToString() &&
                                    testCase.Iv.ToString() == result.IVs[0].ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.CipherText.ToString(), result.Result.ToString(), 
                                    $"Failed on count {count} expected CT {testCase.CipherText}, got { result.Result}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "multiblockmessage")
                            {
                                var param = new ModeBlockCipherParameters(
                                    BlockCipherDirections.Encrypt,
                                    testCase.Iv.GetDeepCopy(),
                                    testCase.Keys.GetDeepCopy(),
                                    testCase.PlainText.GetDeepCopy()
                                );
                                var result = _algo.ProcessPayload(param);

                                if (testCase.CipherText.ToString() == result.Result.ToString() && 
                                    testCase.Iv.ToString() == result.IVs[0].ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.CipherText.ToString(), result.Result.ToString(), $"Failed on count {count} expected CT {testCase.CipherText}, got { result.Result}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "permutation" ||
                                     testGroup.TestType.ToLower() == "substitutiontable" ||
                                     testGroup.TestType.ToLower() == "variablekey" ||
                                     testGroup.TestType.ToLower() == "variabletext")
                            {
                                var param = new ModeBlockCipherParameters(
                                    BlockCipherDirections.Encrypt,
                                    testCase.Iv.GetDeepCopy(),
                                    testCase.Keys.GetDeepCopy(),
                                    testCase.PlainText.GetDeepCopy()
                                );
                                var result = _algo.ProcessPayload(param);

                                if (testCase.CipherText.ToString() == result.Result.ToString() &&
                                    testCase.Iv.ToString() == result.IVs[0].ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.CipherText.ToString(), result.Result.ToString(), 
                                    $"Failed on count {count} expected CT {testCase.CipherText}, got { result.Result}");
                                continue;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }

                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            if (testGroup.TestType.ToLower() == "inversepermutation")
                            {
                                var param = new ModeBlockCipherParameters(
                                    BlockCipherDirections.Decrypt,
                                    testCase.Iv.GetDeepCopy(),
                                    testCase.Keys.GetDeepCopy(),
                                    testCase.CipherText.GetDeepCopy()
                                );
                                var result = _algo.ProcessPayload(param);

                                if (testCase.PlainText.ToString() == result.Result.ToString() &&
                                    testCase.Iv.ToString() == result.IVs[0].ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.PlainText.ToString(), result.Result.ToString(), 
                                    $"Failed on count {count} expected CT {testCase.PlainText}, got {result.Result}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "multiblockmessage")
                            {
                                var param = new ModeBlockCipherParameters(
                                    BlockCipherDirections.Decrypt,
                                    testCase.Iv.GetDeepCopy(),
                                    testCase.Keys.GetDeepCopy(),
                                    testCase.CipherText.GetDeepCopy()
                                );
                                var result = _algo.ProcessPayload(param);

                                if (testCase.PlainText.ToString() == result.Result.ToString() &&
                                    testCase.Iv.ToString() == result.IVs[0].ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.PlainText.ToString(), result.Result.ToString(), $"Failed on count {count} expected CT {testCase.PlainText}, got {result.Result}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "permutation" ||
                                     testGroup.TestType.ToLower() == "substitutiontable" ||
                                     testGroup.TestType.ToLower() == "variablekey" ||
                                     testGroup.TestType.ToLower() == "variabletext")
                            {
                                var param = new ModeBlockCipherParameters(
                                    BlockCipherDirections.Decrypt,
                                    testCase.Iv.GetDeepCopy(),
                                    testCase.Keys.GetDeepCopy(),
                                    testCase.CipherText.GetDeepCopy()
                                );
                                var result = _algo.ProcessPayload(param);

                                if (testCase.PlainText.ToString() == result.Result.ToString() &&
                                    testCase.Iv.ToString() == result.IVs[0].ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.PlainText.ToString(), result.Result.ToString(), 
                                    $"Failed on count {count} expected CT {testCase.PlainText}, got { result.Result}");
                                continue;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                    }

                    Assert.Fail($"{testGroup.Function} did not meet expected function values");
                }
            }

            Assert.IsTrue(mctTestHit, "No MCT tests were run");
            Assert.IsTrue(nonMctTestHit, "No normal (non MCT) tests were run");

        }
    }
}
