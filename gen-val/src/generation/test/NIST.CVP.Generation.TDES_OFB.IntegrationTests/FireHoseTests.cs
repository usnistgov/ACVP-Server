﻿using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.TDES_OFB;
using NIST.CVP.Generation.TDES_OFB.v1_0.Parsers;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace NIST.CVP.Generation.TDES_OFB.IntegrationTests
{
    public class FireHoseTests
    {
        private string _testPath;
        private OfbBlockCipher _algo;
        private MonteCarloTdesOfb _algoMct;
        private MonteCarloKeyMaker _keyMaker;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\tdes-ofb\");
            _algo = new OfbBlockCipher(new TdesEngine());
            _keyMaker = new MonteCarloKeyMaker();
            _algoMct = new MonteCarloTdesOfb(
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
                                testCase.ResultsArray.First().IV.GetDeepCopy(),
                                testCase.ResultsArray.First().Keys.GetDeepCopy(),
                                testCase.ResultsArray.First().PlainText.GetDeepCopy()
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
                                testCase.ResultsArray.First().IV.GetDeepCopy(),
                                testCase.ResultsArray.First().Keys.GetDeepCopy(),
                                testCase.ResultsArray.First().CipherText.GetDeepCopy()
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
                            if (testGroup.TestType.ToLower() == "mmt")
                            {
                                testCase.Key = testCase.Key1.ConcatenateBits(testCase.Key2.ConcatenateBits(testCase.Key3));
                            }
                            var param = new ModeBlockCipherParameters(
                                BlockCipherDirections.Encrypt,
                                testCase.Iv.GetDeepCopy(),
                                testCase.Key.GetDeepCopy(),
                                testCase.PlainText.GetDeepCopy()
                            );
                            var result = _algo.ProcessPayload(param);

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
                            if (testGroup.TestType.ToLower() == "mmt")
                            {
                                //Since MMT files include 3 keys (while KAT files only include 1), we concatenate them into a single key before inputing them into the DEA.
                                testCase.Key = testCase.Key1.ConcatenateBits(testCase.Key2.ConcatenateBits(testCase.Key3));
                            }
                            var param = new ModeBlockCipherParameters(
                                BlockCipherDirections.Decrypt,
                                testCase.Iv.GetDeepCopy(),
                                testCase.Key.GetDeepCopy(),
                                testCase.CipherText.GetDeepCopy()
                            );
                            var result = _algo.ProcessPayload(param);

                            if (testCase.PlainText.ToHex() == result.Result.ToHex())
                                passes++;
                            else
                                fails++;

                            Assert.AreEqual(testCase.PlainText.ToHex(), result.Result.ToHex(),
                                $"Failed on count {count} expected PT {testCase.PlainText.ToHex()}, got {result.Result.ToHex()}");
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
