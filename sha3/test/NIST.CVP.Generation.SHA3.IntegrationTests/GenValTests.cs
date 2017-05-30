using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using SHA3;

namespace NIST.CVP.Generation.SHA3.IntegrationTests
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
            SHA3_Val.AutofacConfig.OverrideRegistrations = null;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            //Directory.Delete(_testPath, true);
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
            var fileName = GetTestFileFewTestCasesSHA3(targetFolder);
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
            var fileName = GetTestFileFewTestCasesSHA3(targetFolder);

            var result = Program.Main(new string[] {fileName});
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldCreateTestVectors()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA3(targetFolder);

            RunGeneration(targetFolder, fileName);

            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), "testResults");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), "testResults");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), "testResults");
        }

        [Test]
        public void ValShouldReturn1OnFailedRun()
        {
            SHA3_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA3(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = SHA3_Val.Program.Main(GetFileNamesWithPath(targetFolder, _testVectorFileNames));

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ValShouldReturn1OnException()
        {
            SHA3_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA3(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = SHA3_Val.Program.Main(GetFileNamesWithPath(targetFolder, _testVectorFileNames));

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldCreateValidationFile()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA3(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            Assert.IsTrue(File.Exists($@"{targetFolder}\validation.json"), "validation");
        }

        [Test]
        public void SpeedTests()
        {
            var targetFolder = Path.Combine(_testPath, "ParallelTest");
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
            var fileName = GetTestFileTests(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationFewTestCasesSHA3()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA3(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationManyTestsSHA3()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileManyTestCasesSHA3(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationFewTestsSHAKE()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHAKE(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationManyTestsSHAKE()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileManyTestCasesSHAKE(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportFailedDispositionOnErrorTests()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesSHA3(targetFolder);

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
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"),
                $@"{targetFolder}\{_testVectorFileNames[0]} file");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"),
                $@"{targetFolder}\{_testVectorFileNames[1]} file");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"),
                $@"{targetFolder}\{_testVectorFileNames[2]} file");
            Assert.IsTrue(result == 0);
        }

        private void RunValidation(string targetFolder)
        {
            var result = SHA3_Val.Program.Main(GetFileNamesWithPath(targetFolder, _testVectorFileNames));
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
                    failedTestCases.Add((int) testCase.tcId);

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

        private string GetTestFileTests(string targetFolder)
        {
            //RemoveMCTTestGroupFactories();

            var parameters = new Parameters
            {
                Algorithm = "SHA3",
                DigestSizes = new[] {384, 512},
                BitOrientedInput = true,
                IncludeNull = true,
                IsSample = false
            };

            return CreateRegistration(targetFolder, parameters);
        }

        private string GetTestFileFewTestCasesSHA3(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = "SHA3",
                DigestSizes = new [] {512},
                BitOrientedInput = false,
                IncludeNull = false,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        private string GetTestFileManyTestCasesSHA3(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = "SHA3",
                DigestSizes = new [] {224, 256, 384, 512},
                BitOrientedInput = true,
                IncludeNull = true,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        private string GetTestFileFewTestCasesSHAKE(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = "SHAKE",
                DigestSizes = new[] { 128 },
                BitOrientedInput = false,
                BitOrientedOutput = false,
                IncludeNull = false,
                MinOutputLength = 256,
                MaxOutputLength = 512,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        private string GetTestFileManyTestCasesSHAKE(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = "SHAKE",
                DigestSizes = new[] { 128, 256 },
                BitOrientedInput = true,
                BitOrientedOutput = true,
                IncludeNull = true,
                MinOutputLength = 16,
                MaxOutputLength = 65536,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        private void RemoveMCTTestGroupFactories()
        {
            AutofacConfig.OverrideRegistrations += builder =>
            {
                builder.RegisterType<NullMCTTestGroupFactory<Parameters, TestGroup>>().AsImplementedInterfaces();
            };
        }

        private static string CreateRegistration(string targetFolder, Parameters parameters)
        {
            var json = JsonConvert.SerializeObject(parameters, Formatting.Indented);
            string fileName = $@"{targetFolder}\registration.json";
            File.WriteAllText(fileName, json);
            return fileName;
        }
    }
}
