﻿using System.IO;
using NIST.CVP.Crypto.ANSIX963;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.KDF_Components.v1_0.ANXIX963.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ANSIX963.IntegrationTests
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
            var shaFactory = new ShaFactory();

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

                        var result = algo.DeriveKey(testCase.Z, testCase.SharedInfo, testGroup.KeyDataLength);

                        Assert.IsTrue(result.Success, result.ErrorMessage);
                        Assert.AreEqual(testCase.KeyData.ToHex(), result.DerivedKey.ToHex(), $"Failed on KeyData {testCase.KeyData.ToHex()}, got {result.DerivedKey.ToHex()}");
                    }
                }
            }
        }
    }
}