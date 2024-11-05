using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.TPM;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TPMv1_2.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.TPM.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\kdf-components\TPM\");
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
            var tpmFactory = new TpmFactory(new HmacFactory(new NativeShaFactory()));

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
                    var algo = tpmFactory.GetTpm();

                    foreach (var testCase in testGroup.Tests)
                    {
                        count++;

                        var result = algo.DeriveKey(
                            testCase.Auth,
                            testCase.NonceEven,
                            testCase.NonceOdd
                        );

                        Assert.That(result.Success, Is.True, result.ErrorMessage);
                        Assert.That(result.SKey.ToHex(), Is.EqualTo(testCase.SKey.ToHex()), $"Failed on SKey {testCase.SKey.ToHex()}, got {result.SKey.ToHex()}");
                    }
                }
            }
        }
    }
}
