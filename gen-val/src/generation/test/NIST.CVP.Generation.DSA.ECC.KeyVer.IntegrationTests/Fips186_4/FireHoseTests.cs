using System.IO;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.ECDSA.v1_0.KeyVer.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.IntegrationTests.Fips186_4
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\ECDSA\KeyVer\");
        }

        [Test]
        public void ShouldRunThroughTestFilesAndValidateKeyGen()
        {
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            var folderPath = new DirectoryInfo(Path.Combine(_testPath));
            var parser = new LegacyResponseFileParser();
            var algo = new EccDsa();
            var curveFactory = new EccCurveFactory();

            foreach (var testFilePath in folderPath.EnumerateFiles())
            {
                var parseResult = parser.Parse(testFilePath.FullName);
                if (!parseResult.Success)
                {
                    Assert.Fail($"Could not parse: {testFilePath.FullName}");
                }

                var testVector = parseResult.ParsedObject;
                if (testVector.TestGroups.Count == 0)
                {
                    Assert.Fail("No TestGroups parsed");
                }

                foreach (var testGroup in testVector.TestGroups)
                {
                    if (testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    foreach (var testCase in testGroup.Tests)
                    {
                        var domainParams = new EccDomainParameters(curveFactory.GetCurve(testGroup.Curve));
                        var result = algo.ValidateKeyPair(domainParams, testCase.KeyPair);
                        if (result.Success != testCase.TestPassed)
                        {
                            Assert.Fail($"Incorrect response for TestCase: {testCase.TestCaseId}");
                        }
                    }
                }
            }
        }
    }
}
