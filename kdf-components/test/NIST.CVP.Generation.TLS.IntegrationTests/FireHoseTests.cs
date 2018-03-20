using NUnit.Framework;
using System.IO;
using NIST.CVP.Crypto.TLS;
using NIST.CVP.Generation.TLS.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.TLS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\TLS\");
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
            var tlsFactory = new TlsKdfFactory();

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
