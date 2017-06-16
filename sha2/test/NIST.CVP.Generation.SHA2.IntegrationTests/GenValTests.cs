using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using SHA2;

namespace NIST.CVP.Generation.SHA2.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests
    {
        private string _testPath;
        private readonly string[] _testVectorFileNames = new string[]
        {
            @"\testResults.json",
            @"\prompt.json",
            @"\answer.json"
        };

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\temp_IntegrationTests\");
            AutofacConfig.OverrideRegistrations = null;
            SHA2_Val.AutofacConfig.OverrideRegistrations = null;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Directory.Delete(_testPath, true);
        }

        [Test]
        public void GenShouldReturn1OnInvalidFileName()
        {
            var result = Program.Main(new string[] {$"{Guid.NewGuid()}.json"});
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldReturn1OnFailedRun()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA2(targetFolder);
            var result = Program.Main(new string[] {fileName});
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldReturn1OnException()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionParameterParser<Parameters>>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA2(targetFolder);

            var result = Program.Main(new string[] {fileName});
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldCreateTestVectors()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA2(targetFolder);

            RunGeneration(targetFolder, fileName);

            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), "testResults");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), "testResults");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), "testResults");
        }

        [Test]
        public void ValShouldReturn1OnFailedRun()
        {
            SHA2_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA2(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = SHA2_Val.Program.Main(GetFileNamesWithPath(targetFolder, _testVectorFileNames));

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ValShouldReturn1OnException()
        {
            SHA2_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA1(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = SHA2_Val.Program.Main(GetFileNamesWithPath(targetFolder, _testVectorFileNames));

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldCreateValidationFile()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA2(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            Assert.IsTrue(File.Exists($@"{targetFolder}\validation.json"), "validation");
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationFewSHA1TestCases()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA1(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationFewSHA2TestCases()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA2(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationManySHA2Tests()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileManyTestCasesSHA2(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        //[Test]
        //public void ShouldParallelizeTestsSHA()
        //{
        //    var targetFolder = Path.Combine(_testPath, "ParallelTest");
        //    if (!Directory.Exists(targetFolder))
        //    {
        //        Directory.CreateDirectory(targetFolder);
        //    }
        //    var fileName = GetTestFileParallelTestCasesSHA(targetFolder);

        //    RunGenerationAndValidation(targetFolder, fileName);

        //    var dp = new DynamicParser();
        //    var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

        //    Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        //}

        [Test]
        public void ShouldReportFailedDispositionOnErrorTests()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA2(targetFolder);

            var expectedFailTestCases = new List<int>();
            RunGenerationAndValidationWithExpectedFailures(targetFolder, fileName, ref expectedFailTestCases);

            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            Assert.AreEqual("failed", parsedValidation.ParsedObject.disposition.ToString(), "disposition");
            foreach (var test in parsedValidation.ParsedObject.tests)
            {
                int tcId = test.tcId;
                string result = test.result;

                if (expectedFailTestCases.Contains(tcId))
                {
                    Assert.AreEqual("failed", result, tcId.ToString());
                }
                else
                {
                    Assert.AreEqual("passed", result, tcId.ToString());
                }
            }
        }

        private string[] GetFileNamesWithPath(string directory, string[] fileNames)
        {
            var numOfFiles = fileNames.Length;
            var fileNamesWithPaths = new string[numOfFiles];

            for (var i = 0; i < numOfFiles; i++)
            {
                fileNamesWithPaths[i] = $"{directory}{fileNames[i]}";
            }

            return fileNamesWithPaths;
        }

        private string GetTestFolder()
        {
            var targetFolder = Path.Combine(_testPath, Guid.NewGuid().ToString());
            Directory.CreateDirectory(targetFolder);
            return targetFolder;
        }

        private void RunGenerationAndValidation(string targetFolder, string fileName)
        {
            RunGeneration(targetFolder, fileName);
            RunValidation(targetFolder);
        }

        private void RunGenerationAndValidationWithExpectedFailures(string targetFolder, string fileName,
            ref List<int> failureTcIds)
        {
            RunGeneration(targetFolder, fileName);
            GetFailureTestCases(targetFolder, ref failureTcIds);
            RunValidation(targetFolder);
        }

        private void RunGeneration(string targetFolder, string fileName)
        {
            var result = Program.Main(new string[] {fileName});
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), $@"{targetFolder}\{_testVectorFileNames[0]} file");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), $@"{targetFolder}\{_testVectorFileNames[1]} file");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), $@"{targetFolder}\{_testVectorFileNames[2]} file");
            Assert.IsTrue(result == 0);
        }

        private void RunValidation(string targetFolder)
        {
            var result = SHA2_Val.Program.Main(GetFileNamesWithPath(targetFolder, _testVectorFileNames));
            Assert.IsTrue(File.Exists($@"{targetFolder}\validation.json"), $@"{targetFolder}\validation file");
            Assert.IsTrue(result == 0);
        }

        private void GetFailureTestCases(string targetFolder, ref List<int> failureTcIds)
        {
            var files = GetFileNamesWithPath(targetFolder, _testVectorFileNames);
            var expectedFailTestCases = DoBadThingsToResultsFile(files[0]);
            Assume.That(expectedFailTestCases.Count > 0);
            failureTcIds.AddRange(expectedFailTestCases);
        }

        private List<int> DoBadThingsToResultsFile(string resultsFile)
        {
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse(resultsFile);
            Assume.That(parsedValidation != null);
            Assume.That(parsedValidation.Success);

            var failedTestCases = new List<int>();
            var rand = new Random800_90();
            foreach (var testCase in parsedValidation.ParsedObject.testResults)
            {
                if ((int) testCase.tcId % 2 == 0)
                {
                    failedTestCases.Add((int)testCase.tcId);

                    if (testCase.hashFail != null)
                    {
                        testCase.hashFail = false;
                    }

                    if (testCase.md != null)
                    {
                        var bs = new BitString(testCase.md.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);
                        testCase.md = bs.ToHex();
                    }

                    if (testCase.msg != null)
                    {
                        var bs = new BitString(testCase.msg.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);
                        testCase.msg = bs.ToHex();
                    }

                    if (testCase.resultsArray != null)
                    {
                        var bsMessage = new BitString(testCase.resultsArray[0].msg.ToString());
                        bsMessage = rand.GetDifferentBitStringOfSameSize(bsMessage);
                        testCase.resultsArray[0].msg = bsMessage.ToHex();

                        var bsDigest = new BitString(testCase.resultsArray[0].md.ToString());
                        bsDigest = rand.GetDifferentBitStringOfSameSize(bsDigest);
                        testCase.resultsArray[0].md = bsDigest.ToHex();
                    }
                }
            }

            File.Delete(resultsFile);
            File.WriteAllText(resultsFile, parsedValidation.ParsedObject.ToString());

            return failedTestCases;
        }

        private string GetTestFileFewTestCasesSHA1(string targetFolder)
        {
            RemoveMCTTestGroupFactories();

            var parameters = new Parameters
            {
                Algorithm = "SHA1",
                DigestSizes = new[] {"160"},
                BitOriented = true,
                IncludeNull = true,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        private string GetTestFileFewTestCasesSHA2(string targetFolder)
        {
            RemoveMCTTestGroupFactories();

            var parameters = new Parameters
            {
                Algorithm = "SHA2",
                DigestSizes = new[] {"224"},
                BitOriented = false,
                IncludeNull = false,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        private string GetTestFileManyTestCasesSHA2(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = "SHA2",
                DigestSizes = new[] { "224", "256", "384", "512", "512/224", "512/256" },
                BitOriented = true,
                IncludeNull = true,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        //private string GetTestFileParallelTestCasesSHA(string targetFolder)
        //{
        //    RemoveAFTTestGroupFactories();
        //    //RemoveMCTTestGroupFactories();

        //    var parameters = new Parameters
        //    {
        //        Algorithm = "SHA2",
        //        DigestSizes = new[] {"384", "512"},
        //        BitOriented = true,
        //        IncludeNull = true,
        //        IsSample = false
        //    };

        //    return CreateRegistration(targetFolder, parameters);
        //}

        /// <summary>
        /// Can be used to exclude MCT tests
        /// </summary>
        public class FakeTestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
        {
            public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
            {
                return new List<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGeneratorAlgorithmFunctionalTest()
                };
            }
        }

        private void RemoveMCTTestGroupFactories()
        {
            AutofacConfig.OverrideRegistrations += builder =>
            {
                builder.RegisterType<FakeTestGroupGeneratorFactory>().AsImplementedInterfaces();
            };
        }

        //private void RemoveAFTTestGroupFactories()
        //{
        //    AutofacConfig.OverrideRegistrations += builder =>
        //    {
        //        builder.RegisterType<NullAFTTestGroupFactory<Parameters, TestGroup>>().AsImplementedInterfaces();
        //    };
        //}

        private static string CreateRegistration(string targetFolder, Parameters parameters)
        {
            var json = JsonConvert.SerializeObject(parameters, Formatting.Indented);
            string fileName = $@"{targetFolder}\registration.json";
            File.WriteAllText(fileName, json);
            return fileName;
        }
    }
}
