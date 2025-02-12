﻿using System.IO;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Generation.AES_ECB.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.AES_ECB.v1_0.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_ECB.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        private EcbBlockCipher _aesEcb;
        private MonteCarloAesEcb _aesEcbMct;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\aes-ecb\");
            _aesEcb = new EcbBlockCipher(new AesEngine());
            _aesEcbMct = new MonteCarloAesEcb(new BlockCipherEngineFactory(), new ModeBlockCipherFactory(), new AesMonteCarloKeyMaker());
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
                            var param = new ModeBlockCipherParameters(BlockCipherDirections.Encrypt,
                                testCase.ResultsArray.First().Key, testCase.ResultsArray.First().PlainText);
                            var result = _aesEcbMct.ProcessMonteCarloTest(param);

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
                            var param = new ModeBlockCipherParameters(BlockCipherDirections.Decrypt,
                                testCase.ResultsArray.First().Key, testCase.ResultsArray.First().CipherText);
                            var result = _aesEcbMct.ProcessMonteCarloTest(param);

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
                            var param = new ModeBlockCipherParameters(BlockCipherDirections.Encrypt, testCase.Key, testCase.PlainText);
                            var result = _aesEcb.ProcessPayload(param);

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
                            var param = new ModeBlockCipherParameters(BlockCipherDirections.Decrypt, testCase.Key, testCase.CipherText);
                            var result = _aesEcb.ProcessPayload(param);

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
