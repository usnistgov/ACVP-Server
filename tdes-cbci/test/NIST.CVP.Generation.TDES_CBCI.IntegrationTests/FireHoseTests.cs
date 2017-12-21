using NIST.CVP.Crypto.TDES_CBCI;
using NIST.CVP.Generation.TDES_CBCI.Parsers;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using NIST.CVP.Crypto.TDES;

namespace NIST.CVP.Generation.TDES_CBCI.IntegrationTests
{
    public class FireHoseTests
    {


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


            var algoMct = new TdesCbciMCT(new MonteCarloKeyMaker());
            var algo = new TdesCbci();

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

                            var result = algoMct.MCTEncrypt(
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
                            var result = algoMct.MCTDecrypt(
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
                            if (testGroup.TestType.ToLower() == "inversepermutation")
                            {
                                var result = algo.BlockEncrypt(
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

                                Assert.AreEqual(testCase.CipherText.ToString(), result.CipherText.ToString(), 
                                    $"Failed on count {count} expected CT {testCase.CipherText}, got { result.CipherText}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "multiblockmessage")
                            {
                                var result = algo.BlockEncrypt(
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
                                var result = algo.BlockEncrypt(
                                    testCase.Keys,
                                    testCase.IV1,
                                    testCase.PlainText
                                );

                                if (testCase.CipherText.ToString() == result.CipherText.ToString() )
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.CipherText.ToString(), result.CipherText.ToString(), 
                                    $"Failed on count {count} expected CT {testCase.CipherText}, got { result.CipherText}");
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
                                var result = algo.BlockDecrypt(
                                    testCase.Keys,
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

                                Assert.AreEqual(testCase.PlainText.ToString(), result.PlainText.ToString(), 
                                    $"Failed on count {count} expected CT {testCase.PlainText}, got {result.PlainText}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "multiblockmessage")
                            {
                                var result = algo.BlockDecrypt(
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
                                var result = algo.BlockDecrypt(
                                    testCase.Keys,
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

                                Assert.AreEqual(testCase.PlainText.ToString(), result.PlainText.ToString(), 
                                    $"Failed on count {count} expected CT {testCase.PlainText}, got { result.PlainText}");
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
