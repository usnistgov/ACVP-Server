﻿using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.IKEv2;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv2;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv2.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.IKEv2.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\kdf-components\IKEv2\");
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
                    var algo = new IkeV2(new HmacFactory(new NativeShaFactory()).GetHmacInstance(testGroup.HashAlg));

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
                            testGroup.DerivedKeyingMaterialLength,
                            testGroup.DerivedKeyingMaterialLength
                        );

                        Assert.That(result.Success, Is.True, result.ErrorMessage);
                        Assert.That(result.SKeySeed.ToHex(), Is.EqualTo(testCase.SKeySeed.ToHex()), $"Failed on SKeySeed {testCase.SKeySeed.ToHex()}, got {result.SKeySeed.ToHex()}");
                        Assert.That(result.DKM.ToHex(), Is.EqualTo(testCase.DerivedKeyingMaterial.ToHex()), $"Failed on DerivedKeyingMaterial {testCase.DerivedKeyingMaterial.ToHex()}, got {result.DKM.ToHex()}");
                        Assert.That(result.DKMChildSA.ToHex(), Is.EqualTo(testCase.DerivedKeyingMaterialChild.ToHex()), $"Failed on DerivedKeyingMaterialChild {testCase.DerivedKeyingMaterialChild.ToHex()}, got {result.DKMChildSA.ToHex()}");
                        Assert.That(result.DKMChildSADh.ToHex(), Is.EqualTo(testCase.DerivedKeyingMaterialDh.ToHex()), $"Failed on DerivedKeyingMaterialDh {testCase.DerivedKeyingMaterialDh.ToHex()}, got {result.DKMChildSADh.ToHex()}");
                        Assert.That(result.SKeySeedReKey.ToHex(), Is.EqualTo(testCase.SKeySeedReKey.ToHex()), $"Failed on SKeySeedReKey {testCase.SKeySeedReKey.ToHex()}, got {result.SKeySeedReKey.ToHex()}");
                    }
                }
            }
        }
    }
}
