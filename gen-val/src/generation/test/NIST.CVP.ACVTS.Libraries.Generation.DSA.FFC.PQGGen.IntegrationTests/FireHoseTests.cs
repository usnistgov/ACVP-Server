﻿using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgGen;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgGen.Parsers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.FFC.PQGGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;
        IShaFactory _shaFactory;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\dsa\pqggen\");
            _shaFactory = new NativeShaFactory();
        }

        [Test]
        //[TestCase("unverifiable")]            Impossible to actually test this 
        [TestCase("canonical")]
        public void ShouldRunThroughAllTestFilesAndValidateGGen(string mode)
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

                        var result = algo.Generate(testCase.P, testCase.Q, testCase.Seed, testCase.Index);
                        if (result.G != testCase.G)
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
        public void ShouldRunThroughAllTestFilesAndValidatePQGen(string mode)
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

                        var algo = pqFactory.GetGeneratorValidator(testGroup.PQGenMode, sha, EntropyProviderTypes.Testable);
                        var seed = new BitString(testCase.Seed.Seed);
                        algo.AddEntropy(seed);

                        var result = algo.Generate(testGroup.L, testGroup.N, seed.BitLength);
                        if (result.P != testCase.P || result.Q != testCase.Q || result.Seed.GetFullSeed() != testCase.Seed.GetFullSeed())
                        {
                            Assert.Fail($"Could not generate TestCase: {testCase.TestCaseId}");
                        }
                    }
                }
            }
        }
    }
}
