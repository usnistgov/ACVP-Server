using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace NIST.CVP.Tests.Core
{
    [TestFixture]
    public abstract class GenValTestsBase
    {
        public abstract string Algorithm { get; }
        public abstract string Mode { get; }
        public string TestPath { get; private set; }
        public string JsonSavePath { get; private set; }
        public string[] AdditionalParameters { get; protected set; } = { };

        public delegate int Executable(string[] paths);

        public abstract Executable Generator { get; }
        public abstract Executable Validator { get; }

        public string[] TestVectorFileNames = { @"\testResults.json", @"\prompt.json", @"\answer.json" };

        protected abstract void OverrideRegistrationGenFakeFailure();
        protected abstract void OverrideRegistrationValFakeException();
        protected abstract void OverrideRegistrationValFakeFailure();
        protected abstract void ModifyTestCaseToFail(dynamic testCase);
        protected abstract string GetTestFileMinimalTestCases(string folderName);
        protected abstract string GetTestFileFewTestCases(string folderName);
        protected abstract string GetTestFileLotsOfTestCases(string folderName);

        public abstract void SetUp();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\temp_integrationTests\");
            JsonSavePath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\..\..\..\json-files\");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Directory.Delete(TestPath, true);
        }

        [Test]
        public void GenShouldReturn1OnNoArgumentsSupplied()
        {
            var result = Generator.Invoke(GetParameters(new string[] { }));
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldReturn1OnInvalidFileName()
        {
            var result = Generator.Invoke(GetParameters(new[] { $"{Guid.NewGuid()}.json" }));
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldReturn1OnAFailedRun()
        {
            OverrideRegistrationGenFakeFailure();

            var targetFolder = GetTestFolder("GenShouldFail");
            var fileName = GetTestFileMinimalTestCases(targetFolder);

            var result = Generator.Invoke(GetParameters(new string[] {fileName}));
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldCreateTestVectors()
        {
            var targetFolder = GetTestFolder("GenMinimal");
            var fileName = GetTestFileMinimalTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);

            Assert.IsTrue(File.Exists($"{targetFolder}{TestVectorFileNames[0]}"), "testResults");
            Assert.IsTrue(File.Exists($"{targetFolder}{TestVectorFileNames[1]}"), "prompt");
            Assert.IsTrue(File.Exists($"{targetFolder}{TestVectorFileNames[2]}"), "answer");
        }

        [Test]
        public void ValShouldReturn1OnAFailedRun()
        {
            OverrideRegistrationValFakeFailure();

            var targetFolder = GetTestFolder("ValShouldFail");
            var fileName = GetTestFileMinimalTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);
            var result = Validator.Invoke(GetParameters(GetFileNamesWithPath(targetFolder, TestVectorFileNames)));

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ValShouldReturn1OnException()
        {
            OverrideRegistrationValFakeException();

            var targetFolder = GetTestFolder("ValShouldException");
            var fileName = GetTestFileMinimalTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);
            var result = Validator.Invoke(GetParameters(GetFileNamesWithPath(targetFolder, TestVectorFileNames)));

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldCreateValidationFile()
        {
            var targetFolder = GetTestFolder("ValMinimal");
            var fileName = GetTestFileMinimalTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);
            RunValidation(targetFolder);

            Assert.IsTrue(File.Exists($@"{targetFolder}\validation.json"), "validation");
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationFewTestCases()
        {
            var targetFolder = GetTestFolder("Few");
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);
            RunValidation(targetFolder);

            // Get object for the validation.json
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            // Validate result as pass
            Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed), parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationLotsOfTests()
        {
            var targetFolder = GetTestFolder("Lots");
            var fileName = GetTestFileLotsOfTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);
            RunValidation(targetFolder);

            // Get object for the validation.json
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            // Validate result as pass
            Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed), parsedValidation.ParsedObject.disposition.ToString());

            // If the assertion passed, move the files to the good directory
            if (!Directory.Exists(JsonSavePath))
            {
                Directory.CreateDirectory(JsonSavePath);
            }

            var newLocation = Path.Combine(JsonSavePath, $"{Algorithm}-{Mode}");
            if (Directory.Exists(newLocation))
            {
                Directory.Delete(newLocation, true);
            }

            Directory.Move(targetFolder, newLocation);
        }

        [Test]
        public void ShouldReportFailedDispositionOnErrorTests()
        {
            var targetFolder = GetTestFolder("FailedTests");
            var fileName = GetTestFileFewTestCases(targetFolder);

            var expectedFailTestCases = new List<int>();
            RunGeneration(targetFolder, fileName);
            GetFailureTestCases(targetFolder, ref expectedFailTestCases);
            RunValidation(targetFolder);

            // Get object for the validation.json
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            // Validate result as fail
            Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Failed), parsedValidation.ParsedObject.disposition.ToString(), "disposition");
            foreach (var test in parsedValidation.ParsedObject.tests)
            {
                int tcId = test.tcId;
                string result = test.result;
                // Validate expected TCs are failure
                if (expectedFailTestCases.Contains(tcId))
                {
                    Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Failed), result, tcId.ToString());
                }
                // Validate other TCs are success
                else
                {
                    Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed), result, tcId.ToString());
                }
            }
        }

        private string[] GetParameters(string[] parameters)
        {
            // Copy the parameters over to a new array with the AdditionalParameters coming first
            var fullParameters = new string[parameters.Length + AdditionalParameters.Length];
            Array.Copy(AdditionalParameters, fullParameters, AdditionalParameters.Length);
            Array.Copy(parameters, 0, fullParameters, AdditionalParameters.Length, parameters.Length);

            return fullParameters;
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

        private string GetDateTime()
        {
            var result = "";
            var now = DateTime.Now;
            result += now.ToString("yyMMdd-HHmmss");
            result += now.Millisecond;
            return result;
        }

        private string GetTestFolder(string name)
        {
            var folderName = $"{Algorithm}-{Mode}-{GetDateTime()}-{name}";
            var targetFolder = Path.Combine(TestPath, folderName);
            Directory.CreateDirectory(targetFolder);

            return targetFolder;
        }

        private void RunGeneration(string targetFolder, string fileName)
        {
            // Run test vector generation
            var result = Generator.Invoke(GetParameters(new[] { fileName }));
            Assert.IsTrue(File.Exists($"{targetFolder}{TestVectorFileNames[0]}"), $"{targetFolder}{TestVectorFileNames[0]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{TestVectorFileNames[1]}"), $"{targetFolder}{TestVectorFileNames[1]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{TestVectorFileNames[2]}"), $"{targetFolder}{TestVectorFileNames[2]}");
            Assert.IsTrue(result == 0);
        }

        private void RunValidation(string targetFolder)
        {
            // Run test vector validation
            var result = Validator.Invoke(GetParameters(GetFileNamesWithPath(targetFolder, TestVectorFileNames)));
            Assert.IsTrue(File.Exists($@"{targetFolder}\validation.json"), $"{targetFolder} validation");
            Assert.IsTrue(result == 0);
        }

        private void GetFailureTestCases(string targetFolder, ref List<int> failureTcIds)
        {
            var files = GetFileNamesWithPath(targetFolder, TestVectorFileNames);

            // Modify testResults in order to contain some tests that will fail
            var expectedFailTestCases = DoBadThingsToResultsFile(files[0]);
            Assume.That(expectedFailTestCases.Count > 0);
            failureTcIds.AddRange(expectedFailTestCases);
        }

        private List<int> DoBadThingsToResultsFile(string resultsFile)
        {
            // Parse file
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse(resultsFile);
            Assume.That(parsedValidation != null);
            Assume.That(parsedValidation.Success);

            var failedTestCases = new List<int>();
            foreach (var testCase in parsedValidation.ParsedObject.testResults)
            {
                if ((int)testCase.tcId % 2 == 0)
                {
                    failedTestCases.Add((int)testCase.tcId);
                    ModifyTestCaseToFail(testCase);
                }
            }

            // Write the new JSON to the results file
            File.Delete(resultsFile);
            File.WriteAllText(resultsFile, parsedValidation.ParsedObject.ToString());

            return failedTestCases;
        }

        protected string CreateRegistration(string targetFolder, IParameters parameters)
        {
            var json = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new BitstringConverter(),
                    new BigIntegerConverter(),
                    new DomainConverter()
                },
                Formatting = Formatting.Indented
            });
            var fileName = $@"{targetFolder}\registration.json";
            File.WriteAllText(fileName, json);

            return fileName;
        }
    }
}
