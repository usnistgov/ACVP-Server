using System.IO;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyGen.Parsers;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.ECC.KeyGen.IntegrationTests.Fips186_4
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\ecdsa\KeyGen\");
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

                    var tests = testGroup.Tests.Shuffle().Take(2);
                    foreach (var testCase in tests)
                    {
                        var entropyProvider = new TestableEntropyProvider();
                        entropyProvider.AddEntropy(testCase.KeyPair.PrivateD);

                        var algo = new EccDsa(entropyProvider);

                        var domainParams = new EccDomainParameters(curveFactory.GetCurve(testGroup.Curve));
                        var result = algo.GenerateKeyPair(domainParams);
                        if (!result.Success)
                        {
                            Assert.Fail($"Could not regenerate TestCase: {testCase.TestCaseId}");
                        }
                        else
                        {
                            Assert.AreEqual(testCase.KeyPair.PublicQ.X, result.KeyPair.PublicQ.X, "x");
                            Assert.AreEqual(testCase.KeyPair.PublicQ.Y, result.KeyPair.PublicQ.Y, "y");
                        }
                    }
                }
            }
        }
    }
}
