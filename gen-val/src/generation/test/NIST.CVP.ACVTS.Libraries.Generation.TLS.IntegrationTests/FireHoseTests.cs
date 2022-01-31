using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.TLS;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.TLS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\kdf-components\TLS\");
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
            var tlsFactory = new TlsKdfFactory(new HmacFactory(new NativeShaFactory()));

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

                foreach (var testGroup in testVector.TestGroups)
                {
                    var algo = tlsFactory.GetTlsKdfInstance(testGroup.TlsMode, testGroup.HashAlg);

                    foreach (var testCase in testGroup.Tests)
                    {
                        count++;

                        var result = algo.DeriveKey(
                            testCase.PreMasterSecret,
                            testCase.ClientHelloRandom,
                            testCase.ServerHelloRandom,
                            testCase.ClientRandom,
                            testCase.ServerRandom,
                            testGroup.KeyBlockLength
                        );

                        Assert.IsTrue(result.Success, result.ErrorMessage);
                        Assert.AreEqual(testCase.MasterSecret.ToHex(), result.MasterSecret.ToHex(), $"Failed on MasterSecret {testCase.MasterSecret.ToHex()}, got {result.MasterSecret.ToHex()}");
                        Assert.AreEqual(testCase.KeyBlock.ToHex(), result.DerivedKey.ToHex(), $"Failed on KeyBlock {testCase.KeyBlock.ToHex()}, got {result.DerivedKey.ToHex()}");
                    }
                }
            }
        }
    }
}
