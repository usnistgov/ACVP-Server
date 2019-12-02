using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NIST.CVP.Crypto.SSH;
using NIST.CVP.Generation.KDF_Components.v1_0.SSH;
using NIST.CVP.Generation.KDF_Components.v1_0.SSH.Parsers;
using TestCase = NIST.CVP.Generation.KDF_Components.v1_0.SSH.TestCase;

namespace NIST.CVP.Generation.SSH.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\SSH\");
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
            var sshFactory = new SshFactory();

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

                        Assert.IsTrue(result.Success, result.ErrorMessage);
                        
                        Assert.AreEqual(testCase.InitialIvClient.ToHex(), result.ClientToServer.InitialIv.ToHex(), $"Failed on InitialIvClient {testCase.InitialIvClient.ToHex()}, got {result.ClientToServer.InitialIv.ToHex()}");
                        Assert.AreEqual(testCase.EncryptionKeyClient.ToHex(), result.ClientToServer.EncryptionKey.ToHex(), $"Failed on EncryptionKeyClient {testCase.EncryptionKeyClient.ToHex()}, got {result.ClientToServer.EncryptionKey.ToHex()}");
                        Assert.AreEqual(testCase.IntegrityKeyClient.ToHex(), result.ClientToServer.IntegrityKey.ToHex(), $"Failed on IntegrityKeyClient {testCase.IntegrityKeyClient.ToHex()}, got {result.ClientToServer.IntegrityKey.ToHex()}");
                        
                        Assert.AreEqual(testCase.InitialIvServer.ToHex(), result.ServerToClient.InitialIv.ToHex(), $"Failed on InitialIvServer {testCase.InitialIvServer.ToHex()}, got {result.ServerToClient.InitialIv.ToHex()}");
                        Assert.AreEqual(testCase.EncryptionKeyServer.ToHex(), result.ServerToClient.EncryptionKey.ToHex(), $"Failed on EncryptionKeyServer {testCase.EncryptionKeyServer.ToHex()}, got {result.ServerToClient.EncryptionKey.ToHex()}");
                        Assert.AreEqual(testCase.IntegrityKeyServer.ToHex(), result.ServerToClient.IntegrityKey.ToHex(), $"Failed on IntegrityKeyServer {testCase.IntegrityKeyServer.ToHex()}, got {result.ServerToClient.IntegrityKey.ToHex()}");
                    }
                }
            }
        }
    }
}
