using System.IO;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_GCM;
using NIST.CVP.Generation.AES_XPN.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_XPN.IntegrationTests
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
            var algo = new AES_GCM(
                new AES_GCMInternals(
                    new RijndaelFactory(
                        new RijndaelInternals()
                    )
                ), 
                new RijndaelFactory(
                    new RijndaelInternals()
                )
            );

            int count = 0;
            int passes = 0;
            int fails = 0;
            int failureTests = 0;
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

                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            var ivXorSalt = testCase.IV.XOR(testCase.Salt);

                            var result = algo.BlockEncrypt(
                                testCase.Key,
                                testCase.PlainText,
                                ivXorSalt,
                                testCase.AAD,
                                testCase.Tag.BitLength
                            );

                            if (!result.Success)
                            {
                                fails++;
                                continue;
                            }

                            if (testCase.CipherText.ToHex() == result.CipherText.ToHex())
                            {
                                passes++;
                            }
                            else
                            {
                                fails++;
                            }

                            Assert.AreEqual(testCase.CipherText.ToHex(), result.CipherText.ToHex(), $"Failed on count {count} expected CT {testCase.CipherText.ToHex()}, got {result.CipherText.ToHex()}");
                            continue;
                        }

                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            var ivXorSalt = testCase.IV.XOR(testCase.Salt);

                            var result = algo.BlockDecrypt(
                                testCase.Key,
                                testCase.CipherText,
                                ivXorSalt,
                                testCase.AAD,
                                testCase.Tag
                            );

                            if (testCase.TestPassed != null && !testCase.TestPassed.Value)
                            {
                                failureTests++;
                                if (result.Success)
                                {
                                    fails++;
                                }
                                else
                                {
                                    passes++;
                                }
                                continue;
                            }

                            if (testCase.PlainText.ToHex() == result.Result.ToHex())
                            {
                                passes++;
                            }
                            else
                            {
                                fails++;
                            }

                            Assert.AreEqual(testCase.PlainText.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected PT {testCase.PlainText.ToHex()}, got {result.Result.ToHex()}");
                            continue;
                        }

                        if (fails > 0)
                            Assert.Fail("Unexpected failures were encountered.");

                        Assert.Fail($"{testGroup.Function} did not meet expected function values");
                    }
                }
            }
            //Assert.Fail($"Passes {passes}, fails {fails}, count {count}.  Failure tests {failureTests}");
        }
       
    }
}
