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
using RSA_KeyGen;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests
    {
        private string _testPath;
        private readonly string[] _testVectorFileNames =
        {
            @"\testResults.json",
            @"\prompt.json",
            @"\answer.json"
        };

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\temp_integrationTests\");
            AutofacConfig.OverrideRegistrations = null;
            RSA_KeyGen_Val.AutofacConfig.OverrideRegistrations = null;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Directory.Delete(_testPath, true);
        }

        [Test]
        public void GenShouldReturn1OnNoArgumentsSupplied()
        {
            var result = Program.Main(new string[] {});
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldReturn1OnInvalidFileName()
        {
            var result = Program.Main(new [] { $"{Guid.NewGuid()}.json" });

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldReturn1OnFailedRun()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder("GenFailed");
            var fileName = GetTestFileFewTestCases(targetFolder);

            var result = Program.Main(new string[] {fileName});
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldCreateTestVectors()
        {
            var targetFolder = GetTestFolder("Gen");
            var fileName = GetCoverageExample(targetFolder);

            RunGeneration(targetFolder, fileName);

            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), "testResults");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), "prompt");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), "answer");
        }

        [Test]
        public void ValShouldReturn1OnFailedRun()
        {
            RSA_KeyGen_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder("ValFailed");
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = RSA_KeyGen_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames)
            );

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ValShouldReturn1OnException()
        {
            RSA_KeyGen_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder("ValException");
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = RSA_KeyGen_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames)
            );

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldCreateValidationFile()
        {
            var targetFolder = GetTestFolder("Val");
            var fileName = GetCoverageExample(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            Assert.IsTrue(File.Exists($@"{targetFolder}\validation.json"), "validation");
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationClientGeneratedValues()
        {
            var targetFolder = GetTestFolder("Client");
            var fileName = GetTestFileClientGeneratedTests(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            // Quick startup mode (skip generation)
            //var targetFolder = ShortCutValidation("client-test");

            // Get object for the validation.json
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            // Validate result as pass
            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationFewTestCases()
        {
            var targetFolder = GetTestFolder("Few");
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            // Get object for the validation.json
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            // Validate result as pass
            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationLotsOfTests()
        {
            var targetFolder = GetTestFolder("Lots");
            var fileName = GetTestFileLotsOfTestCases(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            // Get object for the validation.json
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            // Validate result as pass
            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationNotSample()
        {
            var targetFolder = GetTestFolder("NotSample");
            var fileName = GetTestFileSingleTestCaseNoSample(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            // Get object for the validation.json
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            // Validate result as pass
            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportFailedDispositionOnErrorTests()
        {
            var targetFolder = GetTestFolder("FailedTests");
            var fileName = GetTestFileFewTestCases(targetFolder);

            var expectedFailTestCases = new List<int>();
            RunGenerationAndValidationWithExpectedFailures(targetFolder, fileName, ref expectedFailTestCases);

            // Get object for the validation.json
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            // Validate result as fail
            Assert.AreEqual("failed", parsedValidation.ParsedObject.disposition.ToString(), "disposition");
            foreach (var test in parsedValidation.ParsedObject.tests)
            {
                int tcId = test.tcId;
                string result = test.result;
                // Validate expected TCs are failure
                if (expectedFailTestCases.Contains(tcId))
                {
                    Assert.AreEqual("failed", result, tcId.ToString());
                }
                // Validate other TCs are success
                else
                {
                    Assert.AreEqual("passed", result, tcId.ToString());
                }
            }
        }

        [Test]
        public void ShouldReportFailedValidationWithMissingPropertiesFromJson()
        {
            var targetFolder = GetTestFolder("MissingProps");
            var fileName = GetTestFileClientGeneratedTestsNotSample(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            // Get object for the validation.json
            var dp = new DynamicParser();
            var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");

            // Validate result as pass
            Assert.AreEqual("failed", parsedValidation.ParsedObject.disposition.ToString());
        }

        private string ShortCutValidation(string folderName)
        {
            var targetFolder = Path.Combine(_testPath, folderName);
            RunValidation(targetFolder);
            return targetFolder;
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

        private string GetTestFolder(string name = "")
        {
            var prefix = name == "" ? "" : name + "--";
            var folderName = prefix + Guid.NewGuid().ToString().Substring(0, 8);
            var targetFolder = Path.Combine(_testPath, folderName);
            Directory.CreateDirectory(targetFolder);

            return targetFolder;
        }

        private void RunGenerationAndValidation(string targetFolder, string fileName)
        {
            RunGeneration(targetFolder, fileName);
            RunValidation(targetFolder);
        }

        private void RunGenerationAndValidationWithExpectedFailures(string targetFolder, string fileName, ref List<int> failureTcIds)
        {
            RunGeneration(targetFolder, fileName);
            GetFailureTestCases(targetFolder, ref failureTcIds);
            RunValidation(targetFolder);
        }

        private void RunGeneration(string targetFolder, string fileName)
        {
            // Run test vector generation
            var result = Program.Main(new [] { fileName });
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), $"{targetFolder}{_testVectorFileNames[0]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), $"{targetFolder}{_testVectorFileNames[1]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), $"{targetFolder}{_testVectorFileNames[2]}");
            Assert.IsTrue(result == 0);
        }

        private void RunValidation(string targetFolder)
        {
            // Run test vector validation
            var result = RSA_KeyGen_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames)
            );
            Assert.IsTrue(File.Exists($@"{targetFolder}\validation.json"), $"{targetFolder} validation");
            Assert.IsTrue(result == 0);
        }

        private void GetFailureTestCases(string targetFolder, ref List<int> failureTcIds)
        {
            var files = GetFileNamesWithPath(targetFolder, _testVectorFileNames);

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
            var rand = new Random800_90();
            foreach (var testCase in parsedValidation.ParsedObject.testResults)
            {
                if ((int)testCase.tcId % 2 == 0)
                {
                    failedTestCases.Add((int)testCase.tcId);

                    // If TC has a cipherText, change it
                    if (testCase.p != null)
                    {
                        BitString bs = new BitString(testCase.p.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);

                        // Can't get something "different" of empty bitstring of the same length
                        if (bs == null)
                        {
                            bs = new BitString("01");
                        }

                        testCase.p = bs.ToHex();
                    }

                    // If TC has a plainText, change it
                    if (testCase.q != null)
                    {
                        BitString bs = new BitString(testCase.q.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);

                        // Can't get something "different" of empty bitstring of the same length
                        if (bs == null)
                        {
                            bs = new BitString("01");
                        }

                        testCase.q = bs.ToHex();
                    }
                }
            }

            // Write the new JSON to the results file
            File.Delete(resultsFile);
            File.WriteAllText(resultsFile, parsedValidation.ParsedObject.ToString());

            return failedTestCases;
        }

        private static string CreateRegistration(string targetFolder, Parameters parameters)
        {
            var json = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>
                {
                    new BitstringConverter(),
                    new BigIntegerConverter()
                },
                Formatting = Formatting.Indented
            });
            var fileName = $@"{targetFolder}\registration.json";
            File.WriteAllText(fileName, json);

            return fileName;
        }

        private string GetCoverageExample(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "KeyGen",
                HashAlgs = new[] { "SHA-224" },
                InfoGeneratedByServer = true,
                IsSample = true,
                KeyGenModes = ParameterValidator.VALID_KEY_GEN_MODES,
                //KeyGenModes = new[] { "B.3.3" },
                Moduli = new[] { 2048 },
                PrimeTests = new[] { "tblC2" },
                PubExpMode = "random"
            };

            return CreateRegistration(targetFolder, p);
        }

        private string GetTestFileSingleTestCaseNoSample(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "KeyGen",
                HashAlgs = new[] { "SHA-224" },
                InfoGeneratedByServer = true,
                IsSample = false,
                KeyGenModes = new[] { "B.3.5" },
                Moduli = new[] { 2048 },
                PrimeTests = new[] { "tblC2" },
                PubExpMode = "random"
            };

            return CreateRegistration(targetFolder, p);
        }

        private string GetTestFileFewTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "KeyGen",
                HashAlgs = new[] {"SHA-224"},
                InfoGeneratedByServer = true,
                IsSample = true,
                KeyGenModes = new[] {"B.3.2", "B.3.4", "B.3.5", "B.3.6"},
                Moduli = new[] {2048},
                PrimeTests = new[] {"tblC2"},
                PubExpMode = "fixed",
                FixedPubExp = "010001"
            };

            return CreateRegistration(targetFolder, p);
        }

        private string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "KeyGen",
                HashAlgs = ParameterValidator.VALID_HASH_ALGS,
                InfoGeneratedByServer = true,
                IsSample = true,
                KeyGenModes = ParameterValidator.VALID_KEY_GEN_MODES,
                Moduli = ParameterValidator.VALID_MODULI,
                PrimeTests = ParameterValidator.VALID_PRIME_TESTS,
                PubExpMode = "random"
            };

            return CreateRegistration(targetFolder, p);
        }

        private string GetTestFileClientGeneratedTests(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "KeyGen",
                HashAlgs = new[] { "SHA-256", "SHA-384" },
                InfoGeneratedByServer = false,
                IsSample = true,
                KeyGenModes = ParameterValidator.VALID_KEY_GEN_MODES,
                Moduli = new [] {2048},
                PrimeTests = new [] {"tblC2"},
                PubExpMode = "random"
            };

            return CreateRegistration(targetFolder, p);
        }

        private string GetTestFileClientGeneratedTestsNotSample(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "KeyGen",
                HashAlgs = new[] { "SHA-256", "SHA-384" },
                InfoGeneratedByServer = false,
                IsSample = false,
                KeyGenModes = ParameterValidator.VALID_KEY_GEN_MODES,
                Moduli = new[] { 2048 },
                PrimeTests = new[] { "tblC2" },
                PubExpMode = "random"
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
