using System.IO;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.ECDSA.v1_0.SigVer.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.IntegrationTests.Fips186_4
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        private string _testPath;
        private IShaFactory _shaFactory;
        private IEccCurveFactory _curveFactory;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\sigver\");
            _shaFactory = new ShaFactory();
            _curveFactory = new EccCurveFactory();
        }

        [Test]
        public void ShouldRunThroughTestFilesAndValidateSigVer()
        {
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            var folderPath = new DirectoryInfo(Path.Combine(_testPath));
            var parser = new LegacyResponseFileParser();

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

                    var sha = _shaFactory.GetShaInstance(testGroup.HashAlg);
                    var algo = new EccDsa(sha);

                    foreach (var testCase in testGroup.Tests)
                    {
                        var domainParams = new EccDomainParameters(_curveFactory.GetCurve(testGroup.Curve));
                        var result = algo.Verify(domainParams, testCase.KeyPair, testCase.Message, testCase.Signature);
                        if (result.Success != testCase.TestPassed)
                        {
                            Assert.Fail($"Could not validate TestCase: {testCase.TestCaseId}, with values: \nR: {testCase.Signature.R}\nS: {testCase.Signature.S}");
                        }
                    }
                }
            }
        }
    }
}
