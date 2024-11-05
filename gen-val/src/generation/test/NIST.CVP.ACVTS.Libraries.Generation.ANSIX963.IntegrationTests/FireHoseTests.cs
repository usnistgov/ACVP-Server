using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.ANSIX963;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX963.Parsers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.ANSIX963.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\kdf-components\ANSX\");
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
            var shaFactory = new NativeShaFactory();

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
                    var algo = new AnsiX963(shaFactory.GetShaInstance(testGroup.HashAlg));

                    foreach (var testCase in testGroup.Tests)
                    {
                        count++;

                        var z = new BitString(testCase.SharedSecret, testGroup.FieldSize).PadToModulusMsb(8);
                        var result = algo.DeriveKey(z, testCase.SharedInfo, testGroup.KeyDataLength);

                        Assert.That(result.Success, Is.True, result.ErrorMessage);
                        Assert.That(result.DerivedKey.ToHex(), Is.EqualTo(testCase.KeyData.ToHex()), $"Failed on KeyData {testCase.KeyData.ToHex()}, got {result.DerivedKey.ToHex()}");
                    }
                }
            }
        }
    }
}
