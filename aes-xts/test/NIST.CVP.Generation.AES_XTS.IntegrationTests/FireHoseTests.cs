using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Generation.AES_XTS.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_XTS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\");
        }

        [Test]
        public void ShouldRunThroughAllTestFilesAndValidate()
        {
            if (!Directory.Exists(_testPath))

            {
                Assert.Fail("Test File Directory does not exist");
            }
            var testDir = new DirectoryInfo(_testPath);
            var parser = new LegacyResponseFileParser();
            var algo = new Crypto.AES_XTS.AesXts();

            int count = 0;
            foreach (var testFilePath in testDir.EnumerateFiles())
            {
                var parseResult = parser.Parse(testFilePath.FullName);
                if (!parseResult.Success)
                {
                    Assert.Fail($"Could not parse: {testFilePath.FullName}");
                }
                var testVector = parseResult.ParsedObject;

                if (testVector.TestGroups.Count == 0)
                {
                    Assert.Fail("No TestGroups were parsed.");
                }

                foreach (var iTestGroup in testVector.TestGroups)
                {

                    var testGroup = (TestGroup)iTestGroup;
                    foreach (var iTestCase in testGroup.Tests)
                    {
                        count++;

                        var testCase = (TestCase)iTestCase;

                        // If we were given an integer value instead of hex
                        if (testCase.I == null)
                        {
                            testCase.I = algo.GetIFromInteger(testCase.SequenceNumber);
                        }

                        // Shorten plaintext and ciphertext to the length the group specifies
                        testCase.PlainText = testCase.PlainText.MSBSubstring(0, testGroup.PtLen);
                        testCase.CipherText = testCase.CipherText.MSBSubstring(0, testGroup.PtLen);

                        if (testGroup.Direction.ToLower() == "encrypt")
                        {
                            var result = algo.Encrypt(
                                testCase.Key,
                                testCase.PlainText,
                                testCase.I
                            );

                            Assert.AreEqual(testCase.CipherText.ToHex(), result.CipherText.ToHex(), $"Failed on count {count} expected CT {testCase.CipherText.ToHex()}, got {result.CipherText.ToHex()}");
                            continue;
                        }

                        if (testGroup.Direction.ToLower() == "decrypt")
                        {
                            var result = algo.Decrypt(
                                testCase.Key,
                                testCase.CipherText,
                                testCase.I
                            );

                            Assert.AreEqual(testCase.PlainText.ToHex(), result.PlainText.ToHex(), $"Failed on count {count} expected PT {testCase.PlainText.ToHex()}, got {result.PlainText.ToHex()}");
                            continue;
                        }
                    }
                }
            }
        }
    }
}
