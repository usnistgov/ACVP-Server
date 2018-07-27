using System.IO;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Symmetric.Engines;
using NUnit.Framework;
using NIST.CVP.Generation.AES_CCM.Parsers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NLog;

namespace NIST.CVP.Generation.AES_CCM.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTests
    {
        private string _testPath;
        private IAeadModeBlockCipher _subject;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            LoggingHelper.ConfigureLogging("FireHose", "aesCcm");
        }

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\");
            _subject = new CcmBlockCipher(new AesEngine(), new ModeBlockCipherFactory(), new AES_CCMInternals());
        }

        [Test]
        public void ShouldRunThroughAllTestFilesAndValidate()
        {
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            LegacyResponseFileParser parser = new LegacyResponseFileParser();
            var parsedFiles = parser.Parse(_testPath);
            if (!parsedFiles.Success)
            {
                Assert.Fail("Failed parsing test files");
            }

            if (parsedFiles.ParsedObject.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            int count = 0;
            int testPasses = 0;
            int fails = 0;
            int failureTests = 0;
            
            var testVector = parsedFiles.ParsedObject;

            if (testVector.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            foreach (var iTestGroup in testVector.TestGroups)
            {

                var testGroup = iTestGroup;
                foreach (var iTestCase in testGroup.Tests)
                {
                    count++;

                    var testCase = iTestCase;

                    if (testGroup.Function.ToLower() == "encrypt")
                    {
                        var param = new AeadModeBlockCipherParameters(
                            BlockCipherDirections.Encrypt,
                            testCase.IV,
                            testCase.Key,
                            testCase.PlainText,
                            testCase.AAD,
                            testGroup.TagLength
                        );
                        var result = _subject.ProcessPayload(param);

                        if (!result.Success)
                        {
                            fails++;
                            continue;
                        }

                        testPasses++;
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
                            testGroup.TagLength
                        );
                        var result = _subject.ProcessPayload(param);

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
                                testPasses++;
                                continue;
                            }
                        }

                        testPasses++;
                        
                        //ThisLogger.Debug(testCase.CipherText.ToHex());
                        Assert.AreEqual(testCase.PlainText.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected PT {testCase.PlainText.ToHex()}, got {result.Result.ToHex()}");
                        continue;
                    }

                    if (fails > 0)
                        Assert.Fail("Unexpected failures were encountered.");

                    Assert.Fail($"{testGroup.Function} did not meet expected function values");
                }
            }

            Assert.IsTrue(failureTests > 0, "No failure test conditions checked");
            Assert.IsTrue(testPasses > 0, "No tests were run");
            //Assert.Fail($"Passes {passes}, fails {fails}, count {count}.  Failure tests {failureTests}");
        }

        private static Logger ThisLogger
        {
            get { return LogManager.GetLogger("FireHose"); }
        }
    }
}
