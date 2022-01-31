using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigGen.Parsers;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.ECC.SigGen.IntegrationTests.Fips186_4
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
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\ecdsa\SigGen\");
            _testPathComponent = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\ecdsa\SigGenComponent\");
            _shaFactory = new NativeShaFactory();
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

                    foreach (var testCase in testGroup.Tests)
                    {
                        var testableEntropy = new TestableEntropyProvider();
                        testableEntropy.AddEntropy(testCase.K);

                        var algo = new EccDsa(sha, new RandomNonceProvider(testableEntropy), EntropyProviderTypes.Testable);

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

                    foreach (var testCase in testGroup.Tests)
                    {
                        var testableEntropy = new TestableEntropyProvider();
                        testableEntropy.AddEntropy(testCase.K);

                        var algo = new EccDsa(sha, new RandomNonceProvider(testableEntropy), EntropyProviderTypes.Testable);

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
