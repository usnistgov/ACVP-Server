using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.SNMP;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SNMP;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SNMP.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SNMP.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\kdf-components\SNMP\");
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
            var algo = new Snmp(new NativeFastSha1());

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
                        var result = algo.KeyLocalizationFunction(testGroup.EngineId, testCase.Password);

                        Assert.That(result.Success, Is.True, result.ErrorMessage);
                        Assert.That(result.SharedKey.ToHex(), Is.EqualTo(testCase.SharedKey.ToHex()), $"Failed on SharedKey {testCase.SharedKey.ToHex()}, got {result.SharedKey.ToHex()}");
                    }
                }
            }
        }
    }
}
