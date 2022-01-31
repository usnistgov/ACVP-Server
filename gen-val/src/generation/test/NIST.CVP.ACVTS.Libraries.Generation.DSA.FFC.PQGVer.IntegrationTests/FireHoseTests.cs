using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.FFC.PQGVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;
        IShaFactory _shaFactory;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\dsa\pqgver\");
            _shaFactory = new NativeShaFactory();
        }

        [Test]
        [TestCase("unverifiable")]
        [TestCase("canonical")]
        public void ShouldRunThroughAllTestFilesAndValidateGVer(string mode)
        {
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            var folderPath = new DirectoryInfo(Path.Combine(_testPath, mode));
            var parser = new LegacyResponseFileParser();
            var gFactory = new GGeneratorValidatorFactory();

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
                    testGroup.SetString("gMode", mode);

                    if (testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    foreach (var iTestCase in testGroup.Tests)
                    {
                        var testCase = iTestCase;
                        var sha = _shaFactory.GetShaInstance(testGroup.HashAlg);
                        var algo = gFactory.GetGeneratorValidator(testGroup.GGenMode, sha);

                        var result = algo.Validate(
                            testCase.P.ToPositiveBigInteger(),
                            testCase.Q.ToPositiveBigInteger(),
                            testCase.G.ToPositiveBigInteger(),
                            testCase.Seed,
                            testCase.Index
                            );
                        if (result.Success != testCase.TestPassed)
                        {
                            Assert.Fail($"Could not generate TestCase: {testCase.TestCaseId}");
                        }
                    }
                }
            }
        }

        [Test]
        [TestCase("probable")]
        [TestCase("provable")]
        public void ShouldRunThroughAllTestFilesAndValidatePQVer(string mode)
        {
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            var folderPath = new DirectoryInfo(Path.Combine(_testPath, mode));
            var parser = new LegacyResponseFileParser();
            var pqFactory = new PQGeneratorValidatorFactory();

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
                    testGroup.SetString("pqMode", mode);

                    if (testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    foreach (var testCase in testGroup.Tests)
                    {
                        var sha = _shaFactory.GetShaInstance(testGroup.HashAlg);

                        var algo = pqFactory.GetGeneratorValidator(testGroup.PQGenMode, sha);

                        var result = algo.Validate(
                            testCase.P.ToPositiveBigInteger(),
                            testCase.Q.ToPositiveBigInteger(),
                            testCase.Seed,
                            testCase.Counter);
                        if (result.Success != testCase.TestPassed)
                        {
                            Assert.Fail($"Could not generate TestCase: {testCase.TestCaseId}");
                        }
                    }
                }
            }
        }
    }
}
