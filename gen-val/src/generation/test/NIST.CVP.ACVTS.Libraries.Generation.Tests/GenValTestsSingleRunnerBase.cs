using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NLog;
using NUnit.Framework;
using Serilog;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests
{
    [TestFixture]
    public abstract class GenValTestsSingleRunnerBase
    {
        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            TestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\temp_integrationTests\");
            JsonSavePath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\..\..\json-files\");
            
            _serviceProvider = EntryPointConfigHelper.GetServiceProviderFromConfigurationBuilder();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (!SaveJson)
            {
                Directory.Delete(TestPath, true);
            }
        }

        public abstract AlgoMode AlgoMode { get; }
        public abstract string Algorithm { get; }
        public abstract string Mode { get; }
        public virtual string Revision { get; set; } = "1.0";
        public virtual IJsonConverterProvider JsonConverterProvider => new JsonConverterProvider();
        private string TestPath { get; set; }
        private string JsonSavePath { get; set; }

        public IRegisterInjections RegistrationsOracle => new RegisterInjections();
        public abstract IRegisterInjections RegistrationsGenVal { get; }

        private readonly string[] _testVectorFileNames = { "expectedResults.json", "internalProjection.json", "prompt.json" };

        protected abstract void ModifyTestCaseToFail(dynamic testCase);
        protected abstract string GetTestFileFewTestCases(string folderName);
        protected abstract string GetTestFileLotsOfTestCases(string folderName);

        // Set this during a test if you want to save the json from the session
        private bool SaveJson = true;

        protected static readonly Logger GenLogger = LogManager.GetLogger("Generator");
        protected static readonly Logger ValLogger = LogManager.GetLogger("Validator");

        private IServiceProvider _serviceProvider;

        private static readonly IFileService FileService = new FileService();

        [Test]
        public async Task ShouldReportAllSuccessfulTestsWithinValidationFewTestCases()
        {
            var targetFolder = GetTestFolder("Few");
            var fileName = GetTestFileFewTestCases(targetFolder);

            LoggingHelper.ConfigureLogging(fileName, "generator", LogLevel.Debug);
            GenLogger.Info($"{Algorithm}-{Mode} Test Vectors");
            await RunGeneration(targetFolder, fileName, true);

            LoggingHelper.ConfigureLogging(fileName, "validator", LogLevel.Debug);
            ValLogger.Info($"{Algorithm}-{Mode} Test Vectors");
            await RunValidation(targetFolder);
            
            // Get object for the validation.json
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse(Path.Combine(targetFolder, "validation.json"));
            
            // Validate result as pass
            Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed), parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public async Task ShouldReportAllSuccessfulTestsWithinValidationLotsOfTests()
        {
            var targetFolder = GetTestFolder("Lots");
            var fileName = GetTestFileLotsOfTestCases(targetFolder);

            LoggingHelper.ConfigureLogging(fileName, "generator", LogLevel.Info);
            GenLogger.Info($"{Algorithm}-{Mode} Test Vectors");
            await RunGeneration(targetFolder, fileName, false);

            LoggingHelper.ConfigureLogging(fileName, "validator", LogLevel.Info);
            ValLogger.Info($"{Algorithm}-{Mode} Test Vectors");
            await RunValidation(targetFolder);

            // Get object for the validation.json
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse(Path.Combine(targetFolder, "validation.json"));

            // Validate result as pass
            Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed), parsedValidation.ParsedObject.disposition.ToString());

            // If the assertion passed, move the files to the good directory
            if (!Directory.Exists(JsonSavePath))
            {
                Directory.CreateDirectory(JsonSavePath);
            }

            var modeSection = "";
            if (!string.IsNullOrEmpty(Mode))
            {
                modeSection = $"-{Mode}";
            }
            var friendlyAlgorithm = Algorithm.Replace("/", "-");
            var newLocation = Path.Combine(JsonSavePath, $"{friendlyAlgorithm}{modeSection}-{Revision}");
            if (Directory.Exists(newLocation))
            {
                Directory.Delete(newLocation, true);
            }

            Directory.Move(targetFolder, newLocation);
        }

        [Test]
        public async Task ShouldReportFailedDispositionOnErrorTests()
        {
            var targetFolder = GetTestFolder("FailedTests");
            var fileName = GetTestFileFewTestCases(targetFolder);

            var expectedFailTestCases = new List<int>();
            await RunGeneration(targetFolder, fileName, true);
            GetFailureTestCases(targetFolder, ref expectedFailTestCases);
            await RunValidation(targetFolder);

            // Get object for the validation.json
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse(Path.Combine(targetFolder, "validation.json"));

            // Validate result as fail
            Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Failed), parsedValidation.ParsedObject.disposition.ToString(), "disposition");
            foreach (var test in parsedValidation.ParsedObject.tests)
            {
                int tcId = test.tcId;
                string result = test.result;
                var expected = test.expected;
                var provided = test.provided;

                // Validate expected TCs are failure
                if (expectedFailTestCases.Contains(tcId))
                {
                    Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Failed), result, tcId.ToString());

                    // There are certain situations where not enough information is provided to generated expected vs provided results
                    if (expected != null || provided != null)
                    {
                        Assert.IsNotNull(expected, "expected must not be null");
                        Assert.IsNotNull(provided, "provided must not be null");
                    }
                }
                // Validate other TCs are success
                else
                {
                    Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed), result, tcId.ToString());
                    Assert.IsNull(expected, "expected must be null");
                    Assert.IsNull(provided, "provided must be null");
                }
            }

            Assert.True(true);
        }

        private string[] GetFileNamesWithPath(string directory, string[] fileNames)
        {
            var numOfFiles = fileNames.Length;
            var fileNamesWithPaths = new string[numOfFiles];

            for (var i = 0; i < numOfFiles; i++)
            {
                fileNamesWithPaths[i] = Path.Combine(directory, fileNames[i]);
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

        protected async Task<GenerateResponse> RunGeneration(string targetFolder, string fileName, bool overrideRegisteredDependencies)
        {
            var registrationJson = FileService.ReadFile(fileName);

            // Run test vector generation
            using var scope = GetContainer(overrideRegisteredDependencies).BeginLifetimeScope();
            try
            {
                var gen = scope.Resolve<IGenerator>();
                var result = await gen.GenerateAsync(new GenerateRequest(registrationJson));

                FileService.WriteFile(Path.Combine(targetFolder, _testVectorFileNames[0]), result.ResultProjection, true);
                FileService.WriteFile(Path.Combine(targetFolder, _testVectorFileNames[1]), result.InternalProjection, true);
                FileService.WriteFile(Path.Combine(targetFolder, _testVectorFileNames[2]), result.PromptProjection, true);

                Assert.IsTrue(result.Success, $"Generator failed to complete with status code: {result.StatusCode}, {EnumHelpers.GetEnumDescriptionFromEnum(result.StatusCode)}, {result.ErrorMessage}");
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);

                Assert.Fail($"Exception running Generator.{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return new GenerateResponse("Test fail");
            }
        }

        protected async Task<ValidateResponse> RunValidation(string targetFolder)
        {
            var resultJson = FileService.ReadFile(Path.Combine(targetFolder, _testVectorFileNames[0]));
            var internalJson = FileService.ReadFile(Path.Combine(targetFolder, _testVectorFileNames[1]));

            // Run test vector validation
            using var scope = GetContainer().BeginLifetimeScope();
            try
            {
                var val = scope.Resolve<IValidator>();
                var result = await val.ValidateAsync(new ValidateRequest(internalJson, resultJson, true));

                FileService.WriteFile(Path.Combine(targetFolder, "validation.json"), result.ValidationResult, true);

                Assert.IsTrue(result.Success,
                    $"Validator failed to complete with status code: {result.StatusCode}, {EnumHelpers.GetEnumDescriptionFromEnum(result.StatusCode)}, {result.ErrorMessage}");
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);

                Assert.Fail($"Exception running Validator.{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return new ValidateResponse("Test fail", StatusCode.ValidatorError);
            }
        }

        private IContainer GetContainer(bool overrideRegisteredDependencies = false)
        {
            var builder = new ContainerBuilder();

            EntryPointConfigHelper.RegisterConfigurationInjections(_serviceProvider, builder);

            RegistrationsOracle.RegisterTypes(builder, AlgoMode);
            RegistrationsGenVal.RegisterTypes(builder, AlgoMode);

            if (overrideRegisteredDependencies)
            {
                OverrideRegisteredDependencies(builder);
            }

            return builder.Build();
        }

        private IGenValInvoker GetGenValInvoker(bool overrideRegisteredDependencies = false)
        {
            return new GenValInvoker(_serviceProvider);
        }

        protected virtual void OverrideRegisteredDependencies(ContainerBuilder builder)
        {

        }

        private void GetFailureTestCases(string targetFolder, ref List<int> failureTcIds)
        {
            var files = GetFileNamesWithPath(targetFolder, _testVectorFileNames);

            // Modify testResults in order to contain some tests that will fail
            var expectedFailTestCases = DoBadThingsToResultsFile(files[0]);
            Assert.That(expectedFailTestCases.Count > 0);
            failureTcIds.AddRange(expectedFailTestCases);
        }

        private List<int> DoBadThingsToResultsFile(string resultsFile)
        {
            // Parse file
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse(resultsFile);
            Assert.That(parsedValidation != null);
            Assert.That(parsedValidation.Success);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetFolder"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected string CreateRegistration(string targetFolder, IParameters parameters)
        {
            var json = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings
            {
                Converters = JsonConverterProvider.GetJsonConverters().ToList(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var fileName = Path.Combine(targetFolder, "registration.json");
            File.WriteAllText(fileName, json);

            return fileName;
        }
    }
}
