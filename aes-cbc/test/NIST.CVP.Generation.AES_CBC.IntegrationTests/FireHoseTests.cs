using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.AES_CBC.Parsers;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CBC.IntegrationTests
{
    [TestFixture]
    public class FireHoseTests
    {
        private string _testPath;
        private AES_CBC _aesCbc;
        private AES_CBC_MCT _aesCbcMct;
        
        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\");
            _aesCbc = new AES_CBC(new RijndaelFactory(new RijndaelInternals()));
            _aesCbcMct = new AES_CBC_MCT(_aesCbc);
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
                    }
                    else
                    {
                        nonMctTestHit = true;

                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            var result = _aesCbc.BlockEncrypt(
                                testCase.IV,
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
                            var result = _aesCbc.BlockDecrypt(
                                testCase.IV,
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

            Assert.IsTrue(mctTestHit, "No MCT tests were run");
            Assert.IsTrue(nonMctTestHit, "No normal (non MCT) tests were run");
            // Assert.Fail($"Passes {passes}, fails {fails}, count {count}");
        }
    }
}
