using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.TPM;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.IO;
using NIST.CVP.Generation.KDF_Components.v1_0.TPMv1_2.Parsers;

namespace NIST.CVP.Generation.TPM.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\TPM\");
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
            var tpmFactory = new TpmFactory(new HmacFactory(new ShaFactory()));

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

                        Assert.IsTrue(result.Success, result.ErrorMessage);
                        Assert.AreEqual(testCase.SKey.ToHex(), result.SKey.ToHex(), $"Failed on SKey {testCase.SKey.ToHex()}, got {result.SKey.ToHex()}");
                    }
                }
            }
        }
    }
}
