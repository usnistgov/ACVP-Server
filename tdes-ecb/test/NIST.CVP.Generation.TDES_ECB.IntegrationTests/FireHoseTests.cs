using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.TDES;
using NIST.CVP.Generation.TDES_ECB.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Generation.TDES_ECB;
using NUnit.Framework;


namespace NIST.CVP.Generation.TDES_EBC.IntegrationTests
{
    
    [TestFixture]
    public class FireHoseTests
    {
        private string _testPath;
        private TdesEcb _tdesEbc;
        //private AES_ECB_MCT _aesEcbMct;
        
        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\");
            _tdesEbc = new TdesEcb();
            //_aesCbcMct = new AES_CBC_MCT(_aesCbc);
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

                var testGroup = (TestGroup) iTestGroup;
                foreach (var iTestCase in testGroup.Tests)
                {
                    count++;

                    var testCase = (TestCase) iTestCase;

                    if (testGroup.TestType.ToLower() == "mct")
                    {
                        mctTestHit = true;//REMOVE WHEN MCT IS IMPLEMENTED

                        continue; //REMOVE WHEN MCT IS IMPLEMENTED

                        /*
                        mctTestHit = true;

                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            var result = _aesCbcMct.MCTEncrypt(
                                testCase.ResultsArray.First().IV,
                                testCase.ResultsArray.First().Key,
                                testCase.ResultsArray.First().PlainText
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
                            var result = _aesCbcMct.MCTDecrypt(
                                testCase.ResultsArray.First().IV,
                                testCase.ResultsArray.First().Key,
                                testCase.ResultsArray.First().CipherText
                            );

                            Assert.IsTrue(testCase.ResultsArray.Count > 0, $"{nameof(testCase)} MCT decrypt count should be gt 0");
                            for (int i = 0; i < testCase.ResultsArray.Count; i++)
                            {
                                Assert.AreEqual(testCase.ResultsArray[i].PlainText, result.Response[i].PlainText, $"PlainText mismatch on index {i}");
                            }
                            continue;
                        }
                        */
                    }

                    else
                    {
                        nonMctTestHit = true;
                        
                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            if(testGroup.TestType.ToLower() == "mmt")
                            {
                                testCase.Key = testCase.Key.ConcatenateBits(testCase.Key2.ConcatenateBits(testCase.Key3));
                            }
                            var result = _tdesEbc.BlockEncrypt(
                                testCase.Key,
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
                            if (testGroup.TestType.ToLower() == "mmt")
                            {
                                //Since MMT files include 3 keys (while KAT files only include 1), we concatenate them into a single key before inputing them into the DEA.
                                testCase.Key = testCase.Key.ConcatenateBits(testCase.Key2.ConcatenateBits(testCase.Key3));
                            }
                            var result = _tdesEbc.BlockDecrypt(
                                testCase.Key,
                                testCase.CipherText
                            );

                            if (testCase.PlainText.ToHex() == result.PlainText.ToHex())
                                passes++;
                            else
                                fails++;

                            Assert.AreEqual(testCase.PlainText.ToHex(), result.PlainText.ToHex(),
                                $"Failed on count {count} expected PT {testCase.PlainText.ToHex()}, got {result.PlainText.ToHex()}");
                            continue;
                        }
                    }

                    Assert.Fail($"{testGroup.Function} did not meet expected function values");
                }
            }

         //   Assert.IsTrue(mctTestHit, "No MCT tests were run");
            Assert.IsTrue(nonMctTestHit, "No normal (non MCT) tests were run");
            // Assert.Fail($"Passes {passes}, fails {fails}, count {count}");
        }
    }
    
}
