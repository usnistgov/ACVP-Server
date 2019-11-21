using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NIST.CVP.Crypto.SRTP;
using NIST.CVP.Generation.KDF_Components.v1_0.SRTP;
using NIST.CVP.Generation.KDF_Components.v1_0.SRTP.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SRTP.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\SRTP\");
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

                        Assert.IsTrue(result.Success, result.ErrorMessage);
                        
                        Assert.AreEqual(testCase.SrtpKe.ToHex(), result.SrtpResult.EncryptionKey.ToHex(), $"Failed on SrtpKe {testCase.SrtpKe.ToHex()}, got {result.SrtpResult.EncryptionKey.ToHex()}");
                        Assert.AreEqual(testCase.SrtpKa.ToHex(), result.SrtpResult.AuthenticationKey.ToHex(), $"Failed on SrtpKa {testCase.SrtpKa.ToHex()}, got {result.SrtpResult.AuthenticationKey.ToHex()}");
                        Assert.AreEqual(testCase.SrtpKs.ToHex(), result.SrtpResult.SaltingKey.ToHex(), $"Failed on SrtpKs {testCase.SrtpKs.ToHex()}, got {result.SrtpResult.SaltingKey.ToHex()}");
                        
                        Assert.AreEqual(testCase.SrtcpKe.ToHex(), result.SrtcpResult.EncryptionKey.ToHex(), $"Failed on SrtcpKe {testCase.SrtcpKe.ToHex()}, got {result.SrtcpResult.EncryptionKey.ToHex()}");
                        Assert.AreEqual(testCase.SrtcpKa.ToHex(), result.SrtcpResult.AuthenticationKey.ToHex(), $"Failed on SrtcpKa {testCase.SrtcpKa.ToHex()}, got {result.SrtcpResult.AuthenticationKey.ToHex()}");
                        Assert.AreEqual(testCase.SrtcpKs.ToHex(), result.SrtcpResult.SaltingKey.ToHex(), $"Failed on SrtcpKs {testCase.SrtcpKs.ToHex()}, got {result.SrtcpResult.SaltingKey.ToHex()}");
                    }
                }
            }
        }
    }
}
