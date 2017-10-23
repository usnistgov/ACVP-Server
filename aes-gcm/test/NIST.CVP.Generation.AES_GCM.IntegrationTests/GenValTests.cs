using Newtonsoft.Json;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AES_GCM;
using Autofac;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.AES_GCM.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests
    {
        string _testPath;
        string[] _testVectorFileNames = new string[]
        {
                "\\testResults.json",
                "\\prompt.json",
                "\\answer.json"
        };

        [SetUp]
        public void Setup()
        {
             _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\temp_integrationTests\");

            AutofacConfig.OverrideRegistrations = null;
            AES_GCM_Val.AutofacConfig.OverrideRegistrations = null;
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Directory.Delete(_testPath, true);
        }
        
        [Test]
        public void GenShouldReturn1OnNoArgumentsSupplied()
        {
            var result = Program.Main(new string[] { });

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldReturn1OnInvalidFileName()
        {
            var result = Program.Main(new string[] { $"{Guid.NewGuid()}.json" });

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

            var result = Program.Main(new string[] { fileName });

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

            var result = Program.Main(new string[] { fileName });

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldCreateTestVectors()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);

            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), "testResults");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), "prompt");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), "answer");
        }

        [Test]
        public void ValShouldReturn1OnFailedRun()
        {
            AES_GCM_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = AES_GCM_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames)
            );

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ValShouldReturn1OnException()
        {
            AES_GCM_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = AES_GCM_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames)
            );

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldCreateValidationFile()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCasesEncryptDecrypt(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            Assert.IsTrue(File.Exists($"{targetFolder}\\validation.json"), "validation");
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationFewTestCases()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            // Get object for the validation.json
            DynamicParser dp = new DynamicParser();
            var parsedValidation = dp.Parse($"{targetFolder}\\validation.json");

            // Validate result as pass
            Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed), parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationLotsOfTests()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileLotsOfTestCases(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            // Get object for the validation.json
            DynamicParser dp = new DynamicParser();
            var parsedValidation = dp.Parse($"{targetFolder}\\validation.json");

            // Validate result as pass
            Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed), parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportSuccessfulTestsWith0LengthAADAnd0LengthPt()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileWithZeroLengthAadAndPt(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            // Get object for the validation.json
            DynamicParser dp = new DynamicParser();
            var parsedValidation = dp.Parse($"{targetFolder}\\validation.json");

            // Validate result as pass
            Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed), parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportFailedDispositionOnErrorTests()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder);
            
            List<int> expectedFailTestCases = new List<int>();
            RunGenerationAndValidationWithExpectedFailures(targetFolder, fileName, ref expectedFailTestCases);
            
            // Get object for the validation.json
            DynamicParser dp = new DynamicParser();
            var parsedValidation = dp.Parse($"{targetFolder}\\validation.json");

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

        [Test]
        public void ShouldReportFailedValidationWithMissingPropertiesFromJson()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileInternalIvNotSample(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            // Get object for the validation.json
            DynamicParser dp = new DynamicParser();
            var parsedValidation = dp.Parse($"{targetFolder}\\validation.json");

            // Validate result as pass
            Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Failed), parsedValidation.ParsedObject.disposition.ToString());
        }

        private string[] GetFileNamesWithPath(string directory, string[] fileNames)
        {
            int numOfFiles = fileNames.Length;
            string[] fileNamesWithPaths = new string[numOfFiles];

            for (int i = 0; i < numOfFiles; i++)
            {
                fileNamesWithPaths[i] = $"{directory}{fileNames[i]}";
            };

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

        private void RunGenerationAndValidationWithExpectedFailures(string targetFolder, string fileName, ref List<int> failureTcIds)
        {
            RunGeneration(targetFolder, fileName);
            GetFailureTestCases(targetFolder, ref failureTcIds);
            RunValidation(targetFolder);
        }

        private void RunGeneration(string targetFolder, string fileName)
        {
            // Run test vector generation
            var result = Program.Main(new string[] { fileName });
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), $"{targetFolder}{_testVectorFileNames[0]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), $"{targetFolder}{_testVectorFileNames[1]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), $"{targetFolder}{_testVectorFileNames[2]}");
            Assert.IsTrue(result == 0);
        }

        private void RunValidation(string targetFolder)
        {
            // Run test vector validation
            var result = AES_GCM_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames)
            );
            Assert.IsTrue(File.Exists($"{targetFolder}\\validation.json"), $"{targetFolder}validation");
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
            DynamicParser dp = new DynamicParser();
            var parsedValidation = dp.Parse(resultsFile);
            Assume.That(parsedValidation != null);
            Assume.That(parsedValidation.Success);

            List<int> failedTestCases = new List<int>();
            Random800_90 rand = new Random800_90();
            foreach (var testCase in parsedValidation.ParsedObject.testResults)
            {
                if ((int)testCase.tcId % 2 == 0)
                {
                    failedTestCases.Add((int)testCase.tcId);

                    // If TC is intended to be a failure test, change it
                    if (testCase.decryptFail != null)
                    {
                        testCase.decryptFail = false;
                    }

                    // If TC has a cipherText, change it
                    if (testCase.cipherText != null)
                    {
                        BitString bs = new BitString(testCase.cipherText.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);

                        // Can't get something "different" of empty bitstring of the same length
                        if (bs == null)
                        {
                            bs = new BitString("01");
                        }

                        testCase.cipherText = bs.ToHex();
                    }

                    // If TC has a plainText, change it
                    if (testCase.plainText != null)
                    {
                        BitString bs = new BitString(testCase.plainText.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);

                        // Can't get something "different" of empty bitstring of the same length
                        if (bs == null)
                        {
                            bs = new BitString("01");
                        }

                        testCase.plainText = bs.ToHex();
                    }
                }
            }

            // Write the new JSON to the results file
            File.Delete(resultsFile);
            File.WriteAllText(resultsFile, parsedValidation.ParsedObject.ToString());

            return failedTestCases;
        }

        private string GetTestFileWithZeroLengthAadAndPt(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-GCM",
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PtLen = new int[] { 0 },
                ivLen = new int[] { 96 },
                ivGen = ParameterValidator.VALID_IV_GEN[1],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[1],
                aadLen = new int[] { 0 },
                TagLen = new int[] { ParameterValidator.VALID_TAG_LENGTHS.First() },
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        private string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-GCM",
                Direction = new string[] { "encrypt" },
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PtLen = new int[] { 128, 0 },
                ivLen = new int[] { 96 },
                ivGen = ParameterValidator.VALID_IV_GEN[0],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[0],
                aadLen = new int[] { 128 },
                TagLen = new int[] { ParameterValidator.VALID_TAG_LENGTHS.First() },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        private string GetTestFileInternalIvNotSample(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-GCM",
                Direction = new string[] { "encrypt" },
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PtLen = new int[] { 128 },
                ivLen = new int[] { 96 },
                ivGen = ParameterValidator.VALID_IV_GEN[0],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[0],
                aadLen = new int[] { 128 },
                TagLen = new int[] { ParameterValidator.VALID_TAG_LENGTHS.First() },
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        private string GetTestFileFewTestCasesEncryptDecrypt(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-GCM",
                Direction = new string[] { "encrypt", "decrypt" },
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PtLen = new int[] { 128 },
                ivLen = new int[] { 96 },
                ivGen = ParameterValidator.VALID_IV_GEN[1],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[1],
                aadLen = new int[] { 128 },
                TagLen = new int[] { ParameterValidator.VALID_TAG_LENGTHS.First() },
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        private string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-GCM",
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                PtLen = new int[] { 128, 120, 256 },
                ivLen = new int[] { 96, 128 },
                ivGen = ParameterValidator.VALID_IV_GEN[1],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[1],
                aadLen = new int[] { 128, 120 },
                TagLen = ParameterValidator.VALID_TAG_LENGTHS,
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        private static string CreateRegistration(string targetFolder, Parameters parameters)
        {
            var json = JsonConvert.SerializeObject(parameters, Formatting.Indented);
            string fileName = $"{targetFolder}\\registration.json";
            File.WriteAllText(fileName, json);

            return fileName;
        }

    }
}
