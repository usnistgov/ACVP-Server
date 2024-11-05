using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.SSH;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using TestCase = NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH.TestCase;

namespace NIST.CVP.ACVTS.Libraries.Generation.SSH.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\kdf-components\SSH\");
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
            var sshFactory = new SshFactory(new NativeShaFactory());

            var count = 0;
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
                    var algo = sshFactory.GetSshInstance(testGroup.HashAlg, testGroup.Cipher);

                    foreach (var iTestCase in testGroup.Tests)
                    {
                        count++;

                        var testCase = (TestCase)iTestCase;
                        var result = algo.DeriveKey(
                            testCase.K,
                            testCase.H,
                            testCase.SessionId
                        );

                        Assert.That(result.Success, Is.True, result.ErrorMessage);

                        Assert.That(result.ClientToServer.InitialIv.ToHex(), Is.EqualTo(testCase.InitialIvClient.ToHex()), $"Failed on InitialIvClient {testCase.InitialIvClient.ToHex()}, got {result.ClientToServer.InitialIv.ToHex()}");
                        Assert.That(result.ClientToServer.EncryptionKey.ToHex(), Is.EqualTo(testCase.EncryptionKeyClient.ToHex()), $"Failed on EncryptionKeyClient {testCase.EncryptionKeyClient.ToHex()}, got {result.ClientToServer.EncryptionKey.ToHex()}");
                        Assert.That(result.ClientToServer.IntegrityKey.ToHex(), Is.EqualTo(testCase.IntegrityKeyClient.ToHex()), $"Failed on IntegrityKeyClient {testCase.IntegrityKeyClient.ToHex()}, got {result.ClientToServer.IntegrityKey.ToHex()}");

                        Assert.That(result.ServerToClient.InitialIv.ToHex(), Is.EqualTo(testCase.InitialIvServer.ToHex()), $"Failed on InitialIvServer {testCase.InitialIvServer.ToHex()}, got {result.ServerToClient.InitialIv.ToHex()}");
                        Assert.That(result.ServerToClient.EncryptionKey.ToHex(), Is.EqualTo(testCase.EncryptionKeyServer.ToHex()), $"Failed on EncryptionKeyServer {testCase.EncryptionKeyServer.ToHex()}, got {result.ServerToClient.EncryptionKey.ToHex()}");
                        Assert.That(result.ServerToClient.IntegrityKey.ToHex(), Is.EqualTo(testCase.IntegrityKeyServer.ToHex()), $"Failed on IntegrityKeyServer {testCase.IntegrityKeyServer.ToHex()}, got {result.ServerToClient.IntegrityKey.ToHex()}");
                    }
                }
            }
        }
    }
}
