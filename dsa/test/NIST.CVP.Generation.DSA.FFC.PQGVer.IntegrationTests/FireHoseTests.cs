using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.DSA.FFC.PQGVer.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;
        IShaFactory _shaFactory;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\pqgver\");
            _shaFactory = new ShaFactory();
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
                        var testCase = (TestCase)iTestCase;
                        var sha = _shaFactory.GetShaInstance(testGroup.HashAlg);
                        var algo = gFactory.GetGeneratorValidator(testGroup.GGenMode, sha);

                        var result = algo.Validate(testCase.P, testCase.Q, testCase.G, testCase.Seed, testCase.Index);
                        if (result.Success != testCase.Result)
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

                    foreach (var iTestCase in testGroup.Tests)
                    {
                        var testCase = (TestCase)iTestCase;
                        var sha = _shaFactory.GetShaInstance(testGroup.HashAlg);

                        var algo = pqFactory.GetGeneratorValidator(testGroup.PQGenMode, sha);

                        var result = algo.Validate(testCase.P, testCase.Q, testCase.Seed, testCase.Counter);
                        if (result.Success != testCase.Result)
                        {
                            Assert.Fail($"Could not generate TestCase: {testCase.TestCaseId}");
                        }
                    }
                }
            }
        }
    }
}
