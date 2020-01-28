using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.TDES_OFBI;
using NIST.CVP.Generation.TDES_OFBI.v1_0.Parsers;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace NIST.CVP.Generation.TDES_OFBI.IntegrationTests
{
    public class FireHoseTests
    {
        private string _testPath;
        private OfbiBlockCipher _algo;
        private MonteCarloTdesOfbi _algoMct;
        private MonteCarloKeyMaker _keyMaker;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\tdes-ofbi\");
            _algo = new OfbiBlockCipher(new TdesEngine());
            _keyMaker = new MonteCarloKeyMaker();
            _algoMct = new MonteCarloTdesOfbi(
                new BlockCipherEngineFactory(),
                new ModeBlockCipherFactory(),
                _keyMaker
            );
        }

        [Test]
        public void ShouldParseAndRunCAVSFiles()
        {
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
                        var firstResult = testCase.ResultsArray.First();
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
                            var param = new ModeBlockCipherParameters(
                                BlockCipherDirections.Encrypt,
                                testCase.IV.GetDeepCopy(),
                                testCase.Keys.GetDeepCopy(),
                                testCase.PlainText.GetDeepCopy()
                            );
                            var result = _algo.ProcessPayload(param);

                            if (testCase.CipherText.ToString() == result.Result.ToString())
                            {
                                passes++;
                            }
                            else
                            {
                                fails++;
                            }

                            Assert.AreEqual(testCase.CipherText.ToHex(), result.Result.ToHex(),
                                $"Failed on count {count} expected CT {testCase.CipherText}, got { result.Result}");
                            continue;
                        }

                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            var param = new ModeBlockCipherParameters(
                                BlockCipherDirections.Decrypt,
                                testCase.IV.GetDeepCopy(),
                                testCase.Keys.GetDeepCopy(),
                                testCase.CipherText.GetDeepCopy()
                            );
                            var result = _algo.ProcessPayload(param);

                            if (testCase.PlainText.ToHex() == result.Result.ToHex())
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
                    }

                    Assert.Fail($"{testGroup.Function} did not meet expected function values");
                }
            }

            Assert.IsTrue(mctTestHit, "No MCT tests were run");
            Assert.IsTrue(nonMctTestHit, "No normal (non MCT) tests were run");

        }
    }
}
