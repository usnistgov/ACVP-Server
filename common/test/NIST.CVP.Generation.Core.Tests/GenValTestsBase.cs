using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Enums;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Tests.Core;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture]
    public abstract class GenValTestsBase
    {
        public abstract AlgoMode AlgoMode { get; }
        public abstract string Algorithm { get; }
        public abstract string Mode { get; }

        public string DllDropLocation { get; private set; }

        public string TestPath { get; private set; }
        public string JsonSavePath { get; private set; }

        public abstract IRegisterInjections RegistrationsCrypto { get; }

        public abstract IRegisterInjections RegistrationsGenVal { get; }

        public string[] TestVectorFileNames = { @"\expectedResults.json", @"\internalProjection.json", @"\prompt.json"};

        protected abstract void ModifyTestCaseToFail(dynamic testCase);
        protected abstract string GetTestFileFewTestCases(string folderName);
        protected abstract string GetTestFileLotsOfTestCases(string folderName);

        // Set this during a test if you want to save the json from the session
        public bool SaveJson = true;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            TestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\temp_integrationTests\");
            JsonSavePath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\..\..\..\json-files\");

            DllDropLocation =
                Utilities.GetConsistentTestingStartPath(GetType(),
                    @"..\..\..\common\src\NIST.CVP.Generation.GenValApp\");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (!SaveJson)
            {
                Directory.Delete(TestPath, true);
            }
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationFewTestCases()
        {
            var targetFolder = GetTestFolder("Few");
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName, true);
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

            RunGeneration(targetFolder, fileName, false);
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

            var spacing = Mode == "" ? "" : "-";
            var friendlyAlgorithm = Algorithm.Replace("/", "-");
            var newLocation = Path.Combine(JsonSavePath, $"{friendlyAlgorithm}{spacing}{Mode}");
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
            RunGeneration(targetFolder, fileName, true);
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

        protected string GetTestFolder(string name)
        {
            var spacing = Mode == "" ? "" : "-";
            var folderName = $"{Algorithm}{spacing}{Mode}-{GetDateTime()}-{name}";
            var targetFolder = Path.Combine(TestPath, folderName);
            Directory.CreateDirectory(targetFolder);

            return targetFolder;
        }

        protected void RunGeneration(string targetFolder, string fileName, bool overrideRegisteredDependencies)
        {
            // Run test vector generation
            using (var scope = GetContainer(overrideRegisteredDependencies).BeginLifetimeScope())
            {
                var gen = scope.Resolve<IGenerator>();
                var result = gen.Generate(fileName);

                Assert.IsTrue(result.Success, "Generator failed to complete");
            }

            Assert.IsTrue(File.Exists($"{targetFolder}{TestVectorFileNames[0]}"), $"{targetFolder}{TestVectorFileNames[0]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{TestVectorFileNames[1]}"), $"{targetFolder}{TestVectorFileNames[1]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{TestVectorFileNames[2]}"), $"{targetFolder}{TestVectorFileNames[2]}");
        }

        protected void RunValidation(string targetFolder)
        {
            // Run test vector validation
            using (var scope = GetContainer().BeginLifetimeScope())
            {
                var val = scope.Resolve<IValidator>();
                var result = val.Validate(
                    $@"{targetFolder}\{TestVectorFileNames[0]}",
                    $@"{targetFolder}\{TestVectorFileNames[1]}"
                );

                Assert.IsTrue(result.Success, "Validator failed to complete");
            }
            Assert.IsTrue(File.Exists($@"{targetFolder}\validation.json"), $"{targetFolder} validation");
        }

        private IContainer GetContainer(bool overrideRegisteredDependencies = false)
        {
            var builder = new ContainerBuilder();

            RegistrationsCrypto.RegisterTypes(builder, AlgoMode);
            RegistrationsGenVal.RegisterTypes(builder, AlgoMode);

            if (overrideRegisteredDependencies)
            {
                OverrideRegisteredDependencies(builder);
            }

            return builder.Build();
        }

        protected virtual void OverrideRegisteredDependencies(ContainerBuilder builder)
        {

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
            foreach (var testGroup in parsedValidation.ParsedObject.testGroups)
            {
                foreach (var testCase in testGroup.tests)
                {
                    if ((int)testCase.tcId % 2 == 0 || testCase.resultsArray != null)
                    {
                        failedTestCases.Add((int)testCase.tcId);
                        ModifyTestCaseToFail(testCase);
                    }
                }
            }

            // Write the new JSON to the results file
            File.Delete(resultsFile);
            File.WriteAllText(resultsFile, parsedValidation.ParsedObject.ToString());

            return failedTestCases;
        }

        protected string CreateRegistration(string targetFolder, IParameters parameters)
        {
            JsonConverterProvider jcp = new JsonConverterProvider();

            var json = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings
            {
                Converters = jcp.GetJsonConverters().ToList(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var fileName = $@"{targetFolder}\registration.json";
            File.WriteAllText(fileName, json);

            return fileName;
        }
    }
}
