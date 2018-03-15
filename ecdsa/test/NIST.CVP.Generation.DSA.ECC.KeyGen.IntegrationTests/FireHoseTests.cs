using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.DSA.ECC.KeyGen.Parsers;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\keygen\");
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
            var algo = new EccDsa(EntropyProviderTypes.Testable);
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
                        algo.AddEntropy(testCase.KeyPair.PrivateD);

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
