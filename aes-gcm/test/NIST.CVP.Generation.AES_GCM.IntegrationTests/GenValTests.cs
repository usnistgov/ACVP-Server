using Newtonsoft.Json;
using NIST.CVP.Generation.AES_GCM.Parsers;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AES_GCM;
using NIST.CVP.Tests.Core;

namespace NIST.CVP.Generation.AES_GCM.IntegrationTests
{
    [TestFixture]
    public class GeneratorTests
    {

        string _testPath = Utilities.GetConsistentTestingStartPath(@"..\..\TestFiles\temp_integrationTests\");
        string[] _testVectorFileNames = new string[]
        {
                "\\testResults.json",
                "\\prompt.json",
                "\\answer.json"
        };

        [OneTimeTearDown]
        public void Teardown()
        {
            Directory.Delete(_testPath, true);
        }

        [Test]
        public void ShouldReturn1OnNoArgumentsSupplied()
        {
            var result = Program.Main(new string[] { });

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldReturn1OnInvalidFileName()
        {
            var result = Program.Main(new string[] { $"{Guid.NewGuid()}.json" });

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldCreateTestVectors()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileLotsOfTestCases(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), "testResults");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), "prompt");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), "answer");
        }

        [Test]
        public void ShouldCreateValidationFile()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileLotsOfTestCases(targetFolder);

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
            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
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
            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
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
            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        public void ShouldReportFailedDispositionOnErrorTests()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileLotsOfTestCases(targetFolder);
            
            List<int> expectedFailTestCases = new List<int>();
            RunGenerationAndValidationWithExpectedFailures(targetFolder, fileName, ref expectedFailTestCases);
            
            // Get object for the validation.json
            DynamicParser dp = new DynamicParser();
            var parsedValidation = dp.Parse($"{targetFolder}\\validation.json");

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
            Assume.That(result == 0);

            Assume.That(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), "testResults");
            Assume.That(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), "prompt");
            Assume.That(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), "answer");
        }

        private void RunValidation(string targetFolder)
        {
            // Run test vector validation
            var result = AES_GCM_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames)
            );
            Assume.That(result == 0);
            Assume.That(File.Exists($"{targetFolder}\\validation.json"), "validation");
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

                        testCase.cipherText = bs.ToHex();
                    }

                    // If TC has a plainText, change it
                    if (testCase.plainText != null)
                    {
                        BitString bs = new BitString(testCase.plainText.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);

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
                Mode = ParameterValidator.VALID_DIRECTIONS,
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
                Mode = new string[] { "encrypt" },
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PtLen = new int[] { 128 },
                ivLen = new int[] { 96 },
                ivGen = ParameterValidator.VALID_IV_GEN[0],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[0],
                aadLen = new int[] { 128 },
                TagLen = new int[] { ParameterValidator.VALID_TAG_LENGTHS.First() },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        private string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-GCM",
                Mode = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                PtLen = new int[] { 128 },
                ivLen = new int[] { 96 },
                ivGen = ParameterValidator.VALID_IV_GEN[1],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[1],
                aadLen = new int[] { 128 },
                TagLen = ParameterValidator.VALID_TAG_LENGTHS,
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        private static string CreateRegistration(string targetFolder, Parameters parameters)
        {
            var json = JsonConvert.SerializeObject(parameters);
            string fileName = $"{targetFolder}\\registration.json";
            File.WriteAllText(fileName, json);

            return fileName;
        }

    }
}
