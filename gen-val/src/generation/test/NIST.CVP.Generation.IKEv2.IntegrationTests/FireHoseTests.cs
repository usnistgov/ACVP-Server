using System.IO;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.IKEv2;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.KDF_Components.v1_0.IKEv2;
using NIST.CVP.Generation.KDF_Components.v1_0.IKEv2.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.IKEv2.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\IKEv2\");
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
                    var algo = new IkeV2(new HmacFactory(new ShaFactory()).GetHmacInstance(testGroup.HashAlg));
                    
                    foreach (var testCase in testGroup.Tests)
                    {
                        count++;

                        var result = algo.GenerateIke(
                            testCase.NInit,
                            testCase.NResp,
                            testCase.Gir,
                            testCase.GirNew,
                            testCase.SpiInit,
                            testCase.SpiResp,
                            testGroup.DerivedKeyingMaterialLength
                        );

                        Assert.IsTrue(result.Success, result.ErrorMessage);
                        Assert.AreEqual(testCase.SKeySeed.ToHex(), result.SKeySeed.ToHex(), $"Failed on SKeySeed {testCase.SKeySeed.ToHex()}, got {result.SKeySeed.ToHex()}");
                        Assert.AreEqual(testCase.DerivedKeyingMaterial.ToHex(), result.DKM.ToHex(), $"Failed on DerivedKeyingMaterial {testCase.DerivedKeyingMaterial.ToHex()}, got {result.DKM.ToHex()}");
                        Assert.AreEqual(testCase.DerivedKeyingMaterialChild.ToHex(), result.DKMChildSA.ToHex(), $"Failed on DerivedKeyingMaterialChild {testCase.DerivedKeyingMaterialChild.ToHex()}, got {result.DKMChildSA.ToHex()}");
                        Assert.AreEqual(testCase.DerivedKeyingMaterialDh.ToHex(), result.DKMChildSADh.ToHex(), $"Failed on DerivedKeyingMaterialDh {testCase.DerivedKeyingMaterialDh.ToHex()}, got {result.DKMChildSADh.ToHex()}");
                        Assert.AreEqual(testCase.SKeySeedReKey.ToHex(), result.SKeySeedReKey.ToHex(), $"Failed on SKeySeedReKey {testCase.SKeySeedReKey.ToHex()}, got {result.SKeySeedReKey.ToHex()}");
                    }
                }
            }
        }
    }
}
