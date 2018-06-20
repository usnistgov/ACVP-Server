using System.IO;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_GCM;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Generation.AES_GCM.Parsers;
using NUnit.Framework;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.AES_GCM.IntegrationTests
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
            var algo = new GcmBlockCipher(new AesEngine(), new ModeBlockCipherFactory(), new AES_GCMInternals(new ModeBlockCipherFactory(), new BlockCipherEngineFactory()));

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
                            var param = new AeadModeBlockCipherParameters(
                                BlockCipherDirections.Encrypt,
                                testCase.IV,
                                testCase.Key,
                                testCase.PlainText,
                                testCase.AAD,
                                testCase.Tag.BitLength
                            );

                            var result = algo.ProcessPayload(param);

                            if (!result.Success)
                            {
                                fails++;
                                continue;
                            }

                            if (testCase.CipherText.ToHex() == result.Result.ToHex())
                            {
                                passes++;
                            }
                            else
                            {
                                fails++;
                            }

                            Assert.AreEqual(testCase.CipherText.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected CT {testCase.CipherText.ToHex()}, got {result.Result.ToHex()}");
                            continue;
                        }

                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            var param = new AeadModeBlockCipherParameters(
                                BlockCipherDirections.Decrypt,
                                testCase.IV,
                                testCase.Key,
                                testCase.CipherText,
                                testCase.AAD,
                                testCase.Tag
                            );

                            var result = algo.ProcessPayload(param);

                            if (testCase.TestPassed != null && !testCase.TestPassed.Value)
                            {
                                failureTests++;
                                if (result.Success)
                                {
                                    fails++;
                                    continue;
                                }
                                else
                                {
                                    passes++;
                                    continue;
                                }
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
