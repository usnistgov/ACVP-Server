using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NIST.CVP.Generation.DSA.FFC.SigVer.Parsers;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;
        IShaFactory _shaFactory;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\sigver\");
            _shaFactory = new ShaFactory();
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

                foreach (var iTestGroup in testVector.TestGroups)
                {
                    var testGroup = (TestGroup)iTestGroup;

                    if (testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    var sha = _shaFactory.GetShaInstance(testGroup.HashAlg);
                    var algo = new FfcDsa(sha);

                    foreach (var iTestCase in testGroup.Tests)
                    {
                        var testCase = (TestCase)iTestCase;

                        var result = algo.Verify(testGroup.DomainParams, testCase.Key, testCase.Message, testCase.Signature);
                        if (result.Success == testCase.FailureTest)
                        {
                            Assert.Fail($"Could not validate TestCase: {testCase.TestCaseId}");
                        }
                    }
                }
            }
        }
    }
}
