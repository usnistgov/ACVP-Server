using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Core;
using NIST.CVP.Crypto.TDES_CFB;
using NIST.CVP.Generation.TDES_CFB.Parsers;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using Algo = NIST.CVP.Crypto.TDES_CFB.Algo;

namespace NIST.CVP.Generation.TDES_CFB.IntegrationTests
{
    public class FireHoseTests
    {
        //private string _testPath;
        //private TdesCfb _algo;
        //private TDES_CFB_MCT _algoMct;
        //private MonteCarloKeyMaker _keyMaker;

        //[SetUp]
        //public void Setup()
        //{
        //    _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\");
        //    //_algo = new TdesCfb();
        //    //_keyMaker = new MonteCarloKeyMaker();
        //    //var modeFactory = new ModeFactory();
        //    //_modeOfOperation = modeFactory.GetMode(algo);
        //    _algoMct = new TDES_CFB_MCT(_algo, _keyMaker, _modeOfOperation);
        //}

        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void ShouldParseAndRunCAVSFiles(Algo algo)
        {
            var _testPath = Utilities.GetConsistentTestingStartPath(GetType(), $@"..\..\TestFiles\LegacyParserFiles\{algo.GetDescription()}");
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

            
            var modeMCT = ModeFactoryMCT.GetMode(algo);
            var mode = ModeFactory.GetMode(algo);

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

                        var firstResult = testCase.ResultsArray.First();
                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            
                            var result = modeMCT.MCTEncrypt(
                                firstResult.Key1.ConcatenateBits(firstResult.Key2).ConcatenateBits(firstResult.Key3),
                                firstResult.IV,
                                firstResult.PlainText
                            );

                            Assert.IsTrue(testCase.ResultsArray.Count > 0, $"{nameof(testCase)} MCT encrypt count should be gt 0");
                            for (int i = 0; i < testCase.ResultsArray.Count; i++)
                            {
                                Assert.AreEqual(testCase.ResultsArray[i].CipherText, result.Response[i].CipherText, $"CipherText mismatch on index {i}");
                            }
                            continue;
                        }
                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            var result = modeMCT.MCTDecrypt(
                                firstResult.Key1.ConcatenateBits(firstResult.Key2).ConcatenateBits(firstResult.Key3),
                                firstResult.IV,
                                firstResult.CipherText
                            );

                            Assert.IsTrue(testCase.ResultsArray.Count > 0, $"{nameof(testCase)} MCT decrypt count should be gt 0");
                            Assert.IsTrue(testCase.ResultsArray.Count == result.Response.Count, "Result and response arrays must be of the same size.");
                            for (int i = 0; i < testCase.ResultsArray.Count; i++)
                            {
                                Assert.AreEqual(testCase.ResultsArray[i].PlainText, result.Response[i].PlainText, $"PlainText mismatch on index {i}");
                            }
                            continue;
                        }
                    }

                    else
                    {
                        nonMctTestHit = true;

                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            //if (testGroup.TestType.ToLower() == "mmt")
                            //{
                            //    testCase.Key = testCase.Key1.ConcatenateBits(testCase.Key2.ConcatenateBits(testCase.Key3));
                            //}
                            var result = mode.BlockEncrypt(
                                testCase.Key1.ConcatenateBits(testCase.Key2.ConcatenateBits(testCase.Key3)),
                                testCase.Iv,
                                testCase.PlainText
                            );

                            if (testCase.CipherText.ToHex() == result.CipherText.ToHex())
                                passes++;
                            else
                                fails++;

                            Assert.AreEqual(testCase.CipherText.ToHex(), result.CipherText.ToHex(),
                                $"Failed on count {count} expected CT {testCase.CipherText.ToHex()}, got {result.CipherText.ToHex()}");
                            continue;
                        }

                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            //if (testGroup.TestType.ToLower() == "mmt")
                            //{
                            //    //Since MMT files include 3 keys (while KAT files only include 1), we concatenate them into a single key before inputing them into the DEA.
                            //    testCase.Key = testCase.Key1.ConcatenateBits(testCase.Key2.ConcatenateBits(testCase.Key3));
                            //}
                            var result = mode.BlockDecrypt(
                                testCase.Key1.ConcatenateBits(testCase.Key2.ConcatenateBits(testCase.Key3)),
                                testCase.Iv,
                                testCase.CipherText
                            );

                            if (testCase.PlainText.ToHex() == result.PlainText.ToHex())
                                passes++;
                            else
                                fails++;

                            Assert.AreEqual(testCase.PlainText.ToHex(), result.PlainText.ToHex(),
                                $"Failed on {testGroup.Function}-{testGroup.TestType}-{testGroup.KeyingOption} count {count} expected PT {testCase.PlainText.ToHex()}, got {result.PlainText.ToHex()}");
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
