using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\kdf\");
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
            var algoFactory = new KdfFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()), new HmacFactory(new NativeShaFactory()), new KmacFactory(new cSHAKEWrapper()));

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
                    var algo = algoFactory.GetKdfInstance(testGroup.KdfMode, testGroup.MacMode, testGroup.CounterLocation, testGroup.CounterLength);

                    foreach (var iTestCase in testGroup.Tests)
                    {
                        count++;

                        var testCase = (TestCase)iTestCase;
                        var result = algo.DeriveKey(
                            testCase.KeyIn,
                            testCase.FixedData,
                            testCase.L,
                            testCase.IV,
                            testCase.BreakLocation
                        );

                        Assert.That(result.Success, Is.True, result.ErrorMessage);
                        Assert.That(result.DerivedKey.ToHex(), Is.EqualTo(testCase.KeyOut.ToHex()), $"Failed on count {count} {testCase.KeyOut.ToHex()}, got {result.DerivedKey.ToHex()}");
                    }
                }
            }
        }
    }
}
