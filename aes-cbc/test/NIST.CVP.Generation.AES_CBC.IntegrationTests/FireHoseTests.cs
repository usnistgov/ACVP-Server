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
        string _testPath;

        AES_CBC algo = new AES_CBC(new RijndaelFactory(new RijndaelInternals()));

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\");
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
            foreach (var iTestGroup in parsedTestVectorSet.ParsedObject.TestGroups)
            {

                var testGroup = (TestGroup) iTestGroup;
                foreach (var iTestCase in testGroup.Tests)
                {
                    count++;

                    var testCase = (TestCase) iTestCase;

                    if (testGroup.Function.ToLower() == "encrypt")
                    {
                        var result = algo.BlockEncrypt(
                            testCase.IV, 
                            testCase.Key, 
                            testCase.PlainText
                        );

                        if (testCase.CipherText.ToHex() == result.CipherText.ToHex())
                            passes++;
                        else
                            fails++;

                        Assert.AreEqual(testCase.CipherText.ToHex(), result.CipherText.ToHex(), $"Failed on count {count} expected CT {testCase.CipherText.ToHex()}, got {result.CipherText.ToHex()}");
                        continue;
                    }

                    if (testGroup.Function.ToLower() == "decrypt")
                    {
                        var result = algo.BlockDecrypt(
                            testCase.IV,
                            testCase.Key,
                            testCase.CipherText
                        );

                        if (testCase.PlainText.ToHex() == result.PlainText.ToHex())
                            passes++;
                        else
                            fails++;

                        Assert.AreEqual(testCase.PlainText.ToHex(), result.PlainText.ToHex(), $"Failed on count {count} expected PT {testCase.PlainText.ToHex()}, got {result.PlainText.ToHex()}");
                        continue;
                    }

                    Assert.Fail($"{testGroup.Function} did not meet expected function values");
                }
            }
            
            // Assert.Fail($"Passes {passes}, fails {fails}, count {count}");
        }
    }
}
