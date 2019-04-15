using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.IO;
using NIST.CVP.Generation.ECDSA.v1_0.SigGen.Parsers;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        private string _testPath;
        private string _testPathComponent;
        private IShaFactory _shaFactory;
        private IEccCurveFactory _curveFactory;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\siggen\");
            _testPathComponent = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\siggencomponent\");
            _shaFactory = new ShaFactory();
            _curveFactory = new EccCurveFactory();
        }

        [Test]
        public void ShouldRunThroughTestFilesAndValidateSigGen()
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
                    var algo = new EccDsa(sha, EntropyProviderTypes.Testable);

                    foreach (var testCase in testGroup.Tests)
                    {
                        algo.AddEntropy(testCase.K);

                        var domainParams = new EccDomainParameters(_curveFactory.GetCurve(testGroup.Curve));
                        var result = algo.Sign(domainParams, testCase.KeyPair, testCase.Message);
                        if (result.Signature.R != testCase.Signature.R || result.Signature.S != testCase.Signature.S)
                        {
                            Assert.Fail($"Could not validate TestCase: {testCase.TestCaseId}, with values: \nR: {testCase.Signature.R}\nS: {testCase.Signature.S}");
                        }
                    }
                }
            }
        }

        [Test]
        [Ignore("Need to generate special file in CAVS for this")]
        public void ShouldRunThroughTestFilesAndValidateSigGenComponent()
        {
            if (!Directory.Exists(_testPathComponent))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            var folderPath = new DirectoryInfo(Path.Combine(_testPathComponent));
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
                    var algo = new EccDsa(sha, EntropyProviderTypes.Testable);

                    foreach (var testCase in testGroup.Tests)
                    {
                        algo.AddEntropy(testCase.K);

                        var domainParams = new EccDomainParameters(_curveFactory.GetCurve(testGroup.Curve));
                        var result = algo.Sign(domainParams, testCase.KeyPair, testCase.Message, true);
                        if (result.Signature.R != testCase.Signature.R || result.Signature.S != testCase.Signature.S)
                        {
                            Assert.Fail($"Could not validate TestCase: {testCase.TestCaseId}, with values: \nR: {testCase.Signature.R}\nS: {testCase.Signature.S}");
                        }
                    }
                }
            }
        }
    }
}
