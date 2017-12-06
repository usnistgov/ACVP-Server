using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES_CFBP;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.TDES_CFBP.Parsers;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFBP.IntegrationTests
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
        [TestCase(Algo.TDES_CFBP1)]
        [TestCase(Algo.TDES_CFBP8)]
        [TestCase(Algo.TDES_CFBP64)]
        public void ShouldParseAndRunCAVSFiles(Algo algo)
        {
            var _testPath = Utilities.GetConsistentTestingStartPath(GetType(), $@"..\..\TestFiles\LegacyParserFiles\{EnumHelpers.GetEnumDescriptionFromEnum(algo)}");
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
                                firstResult.Keys,
                                firstResult.IV1,
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
                                firstResult.Keys,
                                firstResult.IV1,
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
                            if (testGroup.TestType.ToLower() == "inversepermutation")
                            {
                                var result = mode.BlockEncrypt(
                                    testCase.Keys,
                                    testCase.IV1,
                                    testCase.PlainText1,
                                    testCase.PlainText2,
                                    testCase.PlainText3
                                );

                                if (testCase.CipherText1.ToString() == result.CipherTexts[0].ToString() &&
                                    testCase.CipherText2.ToString() == result.CipherTexts[1].ToString() &&
                                    testCase.CipherText3.ToString() == result.CipherTexts[2].ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.CipherText1.ToString(), result.CipherTexts[0].ToString(), $"Failed on count {count} expected CT {testCase.CipherText1}, got { result.CipherTexts[0]}");
                                Assert.AreEqual(testCase.CipherText2.ToString(), result.CipherTexts[1].ToString(), $"Failed on count {count} expected CT {testCase.CipherText2}, got { result.CipherTexts[1]}");
                                Assert.AreEqual(testCase.CipherText3.ToString(), result.CipherTexts[2].ToString(), $"Failed on count {count} expected CT {testCase.CipherText3}, got { result.CipherTexts[2]}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "multiblockmessage")
                            {
                                var result = mode.BlockEncrypt(
                                    testCase.Keys,
                                    testCase.IV1,
                                    testCase.PlainText
                                );

                                if (testCase.CipherText.ToString() == result.CipherText.ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.CipherText.ToString(), result.CipherText.ToString(), $"Failed on count {count} expected CT {testCase.CipherText}, got { result.CipherText}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "permutation" ||
                                     testGroup.TestType.ToLower() == "substitutiontable" ||
                                     testGroup.TestType.ToLower() == "variablekey" ||
                                     testGroup.TestType.ToLower() == "variabletext")
                            {
                                var result = mode.BlockEncrypt(
                                    testCase.Keys,
                                    testCase.IV1,
                                    testCase.PlainText,
                                    true
                                );

                                if (testCase.CipherText1.ToString() == result.CipherTexts[0].ToString() &&
                                    testCase.CipherText2.ToString() == result.CipherTexts[1].ToString() &&
                                    testCase.CipherText3.ToString() == result.CipherTexts[2].ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.CipherText1.ToString(), result.CipherTexts[0].ToString(), $"Failed on count {count} expected CT {testCase.CipherText1}, got { result.CipherTexts[0]}");
                                Assert.AreEqual(testCase.CipherText2.ToString(), result.CipherTexts[1].ToString(), $"Failed on count {count} expected CT {testCase.CipherText2}, got { result.CipherTexts[1]}");
                                Assert.AreEqual(testCase.CipherText3.ToString(), result.CipherTexts[2].ToString(), $"Failed on count {count} expected CT {testCase.CipherText3}, got { result.CipherTexts[2]}");
                                continue;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }

                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            //if (testGroup.TestType.ToLower() == "mmt")
                            //{
                            //    //Since MMT files include 3 keys (while KAT files only include 1), we concatenate them into a single key before inputing them into the DEA.
                            //    testCase.Key = testCase.Key1.ConcatenateBits(testCase.Key2.ConcatenateBits(testCase.Key3));
                            //}
                            if (testGroup.TestType.ToLower() == "inversepermutation")
                            {
                                var result = mode.BlockDecrypt(
                                    testCase.Keys,
                                    testCase.IV1,
                                    testCase.CipherText1,
                                    testCase.CipherText2,
                                    testCase.CipherText3
                                );

                                if (testCase.PlainText1.ToString() == result.PlainTexts[0].ToString() &&
                                    testCase.PlainText2.ToString() == result.PlainTexts[1].ToString() &&
                                    testCase.PlainText3.ToString() == result.PlainTexts[2].ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.PlainText1.ToString(), result.PlainTexts[0].ToString(), $"Failed on count {count} expected CT {testCase.PlainText1}, got {result.PlainTexts[0]}");
                                Assert.AreEqual(testCase.PlainText2.ToString(), result.PlainTexts[1].ToString(), $"Failed on count {count} expected CT {testCase.PlainText2}, got {result.PlainTexts[1]}");
                                Assert.AreEqual(testCase.PlainText3.ToString(), result.PlainTexts[2].ToString(), $"Failed on count {count} expected CT {testCase.PlainText3}, got {result.PlainTexts[2]}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "multiblockmessage")
                            {
                                var result = mode.BlockDecrypt(
                                    testCase.Key1.ConcatenateBits(testCase.Key2.ConcatenateBits(testCase.Key3)),
                                    testCase.IV1,
                                    testCase.CipherText
                                );

                                if (testCase.PlainText.ToString() == result.PlainText.ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.PlainText.ToString(), result.PlainText.ToString(), $"Failed on count {count} expected CT {testCase.PlainText}, got {result.PlainText}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "permutation" ||
                                     testGroup.TestType.ToLower() == "substitutiontable" ||
                                     testGroup.TestType.ToLower() == "variablekey" ||
                                     testGroup.TestType.ToLower() == "variabletext")
                            {
                                var result = mode.BlockDecrypt(
                                    testCase.Keys,
                                    testCase.IV1,
                                    testCase.CipherText,
                                    true
                                );

                                if (testCase.PlainText1.ToString() == result.PlainTexts[0].ToString() &&
                                    testCase.PlainText2.ToString() == result.PlainTexts[1].ToString() &&
                                    testCase.PlainText3.ToString() == result.PlainTexts[2].ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.PlainText1.ToString(), result.PlainTexts[0].ToString(), $"Failed on count {count} expected CT {testCase.PlainText1}, got { result.PlainTexts[0]}");
                                Assert.AreEqual(testCase.PlainText2.ToString(), result.PlainTexts[1].ToString(), $"Failed on count {count} expected CT {testCase.PlainText2}, got { result.PlainTexts[1]}");
                                Assert.AreEqual(testCase.PlainText3.ToString(), result.PlainTexts[2].ToString(), $"Failed on count {count} expected CT {testCase.PlainText3}, got { result.PlainTexts[2]}");
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
            // Assert.Fail($"Passes {passes}, fails {fails}, count {count}");
        }
    }
}
