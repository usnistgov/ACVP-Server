using System.IO;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.TDES_OFBI;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_OFBI.v1_0.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_OFBI.IntegrationTests
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

                            Assert.That(testCase.ResultsArray.Count > 0, Is.True, $"{nameof(testCase)} MCT encrypt count should be gt 0");
                            for (int i = 0; i < testCase.ResultsArray.Count; i++)
                            {
                                Assert.That(result.Response[i].IV, Is.EqualTo(testCase.ResultsArray[i].IV), $"IV mismatch on index {i}");
                                Assert.That(result.Response[i].Keys, Is.EqualTo(testCase.ResultsArray[i].Keys), $"Key mismatch on index {i}");
                                Assert.That(result.Response[i].PlainText, Is.EqualTo(testCase.ResultsArray[i].PlainText), $"PlainText mismatch on index {i}");
                                Assert.That(result.Response[i].CipherText, Is.EqualTo(testCase.ResultsArray[i].CipherText), $"CipherText mismatch on index {i}");
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

                            Assert.That(testCase.ResultsArray.Count > 0, Is.True, $"{nameof(testCase)} MCT decrypt count should be gt 0");
                            Assert.That(testCase.ResultsArray.Count == result.Response.Count, Is.True, "Result and response arrays must be of the same size.");
                            for (int i = 0; i < testCase.ResultsArray.Count; i++)
                            {
                                Assert.That(result.Response[i].IV, Is.EqualTo(testCase.ResultsArray[i].IV), $"IV mismatch on index {i}");
                                Assert.That(result.Response[i].Keys, Is.EqualTo(testCase.ResultsArray[i].Keys), $"Key mismatch on index {i}");
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

                            Assert.That(result.Result.ToHex(), Is.EqualTo(testCase.CipherText.ToHex()),
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

                            Assert.That(result.Result.ToString(), Is.EqualTo(testCase.PlainText.ToString()),
                                $"Failed on count {count} expected CT {testCase.PlainText}, got {result.Result}");
                            continue;
                        }
                    }

                    Assert.Fail($"{testGroup.Function} did not meet expected function values");
                }
            }

            Assert.That(mctTestHit, Is.True, "No MCT tests were run");
            Assert.That(nonMctTestHit, Is.True, "No normal (non MCT) tests were run");

        }
    }
}
