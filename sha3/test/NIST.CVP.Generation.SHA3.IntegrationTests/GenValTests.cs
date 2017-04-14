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
using NUnit.Framework;
using SHA3;

namespace NIST.CVP.Generation.SHA3.IntegrationTests
{
    [TestFixture]
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
            var fileName = GetTestFileFewTestCases(targetFolder);
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
            var fileName = GetTestFileFewTestCases(targetFolder);

            var result = Program.Main(new string[] {fileName});
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldCreateTestVectors()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder);

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
            var fileName = GetTestFileFewTestCases(targetFolder);

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
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = SHA3_Val.Program.Main(GetFileNamesWithPath(targetFolder, _testVectorFileNames));

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldCreateValidationFile()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            Assert.IsTrue(File.Exists($@"{targetFolder}\validation.json"), "validation");
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationFewTestCases()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationManyTests()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileManyTestCases(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportFailedDispositionOnErrorTests()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder);

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

                    if (testCase.digest != null)
                    {
                        var bs = new BitString(testCase.digest.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);
                        testCase.digest = bs.ToHex();
                    }

                    if (testCase.message != null)
                    {
                        var bs = new BitString(testCase.message.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);
                        testCase.message = bs.ToHex();
                    }

                    if (testCase.resultsArray != null)
                    {
                        var bsMessage = new BitString(testCase.resultsArray[0].message.ToString());
                        bsMessage = rand.GetDifferentBitStringOfSameSize(bsMessage);
                        testCase.resultsArray[0].message = bsMessage.ToHex();

                        var bsDigest = new BitString(testCase.resultsArray[0].digest.ToString());
                        bsDigest = rand.GetDifferentBitStringOfSameSize(bsDigest);
                        testCase.resultsArray[0].digest = bsDigest.ToHex();
                    }
                }
            }

            File.Delete(resultsFile);
            File.WriteAllText(resultsFile, parsedValidation.ParsedObject.ToString());

            return failedTestCases;
        }

        private string GetTestFileFewTestCases(string targetFolder)
        {
            RemoveMCTTestGroupFactories();

            var parameters = new Parameters
            {
                Algorithm = "SHA3",
                Functions = new []
                {
                    new Function
                    {
                        Mode = "sha3",
                        DigestSizes = new [] {224, 256, 384}
                    }
                },
                BitOrientedInput = true,
                IncludeNull = false,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        private string GetTestFileManyTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = "SHA3",
                Functions = new[]
                {
                    new Function
                    {
                        Mode = "sha3",
                        DigestSizes = new [] {224, 256, 384, 512}
                    },
                    new Function
                    {
                        Mode = "shake",
                        DigestSizes = new [] {128, 256}
                    }
                },
                BitOrientedInput = true,
                BitOrientedOutput = true,
                IncludeNull = true,
                MinOutputLength = 16,
                MaxOutputLength = 65536,
                IsSample = false
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
