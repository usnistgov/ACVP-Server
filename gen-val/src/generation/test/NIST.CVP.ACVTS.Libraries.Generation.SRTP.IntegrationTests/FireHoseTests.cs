using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.SRTP;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SRTP;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SRTP.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SRTP.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\kdf-components\SRTP\");
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
            var algo = new Srtp();

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

                    foreach (var iTestCase in testGroup.Tests)
                    {
                        count++;

                        var testCase = (TestCase)iTestCase;
                        var result = algo.DeriveKey(
                            testGroup.AesKeyLength,
                            testCase.MasterKey,
                            testCase.MasterSalt,
                            testCase.Kdr,
                            testCase.Index,
                            testCase.SrtcpIndex
                        );

                        Assert.That(result.Success, Is.True, result.ErrorMessage);

                        Assert.That(result.SrtpResult.EncryptionKey.ToHex(), Is.EqualTo(testCase.SrtpKe.ToHex()), $"Failed on SrtpKe {testCase.SrtpKe.ToHex()}, got {result.SrtpResult.EncryptionKey.ToHex()}");
                        Assert.That(result.SrtpResult.AuthenticationKey.ToHex(), Is.EqualTo(testCase.SrtpKa.ToHex()), $"Failed on SrtpKa {testCase.SrtpKa.ToHex()}, got {result.SrtpResult.AuthenticationKey.ToHex()}");
                        Assert.That(result.SrtpResult.SaltingKey.ToHex(), Is.EqualTo(testCase.SrtpKs.ToHex()), $"Failed on SrtpKs {testCase.SrtpKs.ToHex()}, got {result.SrtpResult.SaltingKey.ToHex()}");

                        Assert.That(result.SrtcpResult.EncryptionKey.ToHex(), Is.EqualTo(testCase.SrtcpKe.ToHex()), $"Failed on SrtcpKe {testCase.SrtcpKe.ToHex()}, got {result.SrtcpResult.EncryptionKey.ToHex()}");
                        Assert.That(result.SrtcpResult.AuthenticationKey.ToHex(), Is.EqualTo(testCase.SrtcpKa.ToHex()), $"Failed on SrtcpKa {testCase.SrtcpKa.ToHex()}, got {result.SrtcpResult.AuthenticationKey.ToHex()}");
                        Assert.That(result.SrtcpResult.SaltingKey.ToHex(), Is.EqualTo(testCase.SrtcpKs.ToHex()), $"Failed on SrtcpKs {testCase.SrtcpKs.ToHex()}, got {result.SrtcpResult.SaltingKey.ToHex()}");
                    }
                }
            }
        }
    }
}
