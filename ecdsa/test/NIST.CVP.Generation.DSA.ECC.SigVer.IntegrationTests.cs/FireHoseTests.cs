using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.DSA.ECC.SigVer.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
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
                    var algo = new EccDsa(sha);

                    foreach (var iTestCase in testGroup.Tests)
                    {
                        var testCase = (TestCase)iTestCase;

                        var result = algo.Verify(testGroup.DomainParameters, testCase.KeyPair, testCase.Message, testCase.Signature);
                        if (result.Success != testCase.Result)
                        {
                            Assert.Fail($"Could not validate TestCase: {testCase.TestCaseId}, with values: \nR: {testCase.Signature.R}\nS: {testCase.Signature.S}");
                        }
                    }
                }
            }
        }
    }
}
