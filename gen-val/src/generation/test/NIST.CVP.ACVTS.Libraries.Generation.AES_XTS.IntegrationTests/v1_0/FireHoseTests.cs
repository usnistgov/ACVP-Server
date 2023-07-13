using System.IO;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.XTS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.XTS;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v1_0.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.IntegrationTests.v1_0
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\aes-xts\");
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
            var algo = new XtsBlockCipher(new AesEngine());

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

                    var testGroup = iTestGroup;
                    foreach (var iTestCase in testGroup.Tests)
                    {
                        count++;

                        var testCase = iTestCase;

                        // If we were given an integer value instead of hex
                        testCase.I ??= XtsHelper.GetIFromBigInteger(new BigInteger(testCase.SequenceNumber));

                        // Shorten plaintext and ciphertext to the length the group specifies
                        testCase.PlainText = testCase.PlainText.MSBSubstring(0, testGroup.PayloadLen);
                        testCase.CipherText = testCase.CipherText.MSBSubstring(0, testGroup.PayloadLen);

                        switch (testGroup.Direction.ToLower())
                        {
                            case "encrypt":
                                {
                                    var param = new XtsModeBlockCipherParameters(BlockCipherDirections.Encrypt, testCase.I, testCase.Key, testCase.PlainText, testCase.PlainText.BitLength);
                                    var result = algo.ProcessPayload(param);

                                    Assert.AreEqual(testCase.CipherText.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected CT {testCase.CipherText.ToHex()}, got {result.Result.ToHex()}");
                                    continue;
                                }
                            case "decrypt":
                                {
                                    var param = new XtsModeBlockCipherParameters(BlockCipherDirections.Decrypt, testCase.I, testCase.Key, testCase.CipherText, testCase.CipherText.BitLength);
                                    var result = algo.ProcessPayload(param);

                                    Assert.AreEqual(testCase.PlainText.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected PT {testCase.PlainText.ToHex()}, got {result.Result.ToHex()}");
                                    continue;
                                }
                        }
                    }
                }
            }
        }
    }
}
