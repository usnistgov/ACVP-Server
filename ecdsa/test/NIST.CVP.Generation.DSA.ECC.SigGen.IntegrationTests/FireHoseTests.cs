using System.IO;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.DSA.ECC.SigGen.Parsers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;
        private string _testPathComponent;
        IShaFactory _shaFactory;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\siggen\");
            _testPathComponent = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\siggencomponent\");
            _shaFactory = new ShaFactory();
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

                foreach (var iTestGroup in testVector.TestGroups)
                {
                    var testGroup = (TestGroup)iTestGroup;

                    if (testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    var sha = _shaFactory.GetShaInstance(testGroup.HashAlg);
                    var algo = new EccDsa(sha, EntropyProviderTypes.Testable);

                    foreach (var iTestCase in testGroup.Tests)
                    {
                        var testCase = (TestCase)iTestCase;
                        algo.AddEntropy(testCase.K);

                        var result = algo.Sign(testGroup.DomainParameters, testCase.KeyPair, testCase.Message);
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

                foreach (var iTestGroup in testVector.TestGroups)
                {
                    var testGroup = (TestGroup)iTestGroup;

                    if (testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    var sha = _shaFactory.GetShaInstance(testGroup.HashAlg);
                    var algo = new EccDsa(sha, EntropyProviderTypes.Testable);

                    foreach (var iTestCase in testGroup.Tests)
                    {
                        var testCase = (TestCase)iTestCase;
                        algo.AddEntropy(testCase.K);

                        var result = algo.Sign(testGroup.DomainParameters, testCase.KeyPair, testCase.Message, true);
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
