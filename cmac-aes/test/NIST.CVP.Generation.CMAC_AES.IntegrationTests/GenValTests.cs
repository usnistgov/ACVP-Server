using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using CMAC_AES;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC_AES.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
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
            CMAC_AES_Val.AutofacConfig.OverrideRegistrations = null;
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
            CMAC_AES_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = CMAC_AES_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames)
            );

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ValShouldReturn1OnException()
        {
            CMAC_AES_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = CMAC_AES_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames)
            );

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldCreateValidationFile()
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder);

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
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), $"{targetFolder}{_testVectorFileNames[0]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), $"{targetFolder}{_testVectorFileNames[1]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), $"{targetFolder}{_testVectorFileNames[2]}");
            Assert.IsTrue(result == 0);
        }

        private void RunValidation(string targetFolder)
        {
            // Run test vector validation
            var result = CMAC_AES_Val.Program.Main(
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
                    if (testCase.result != null)
                    {
                        if (testCase.result == "fail")
                        {
                            testCase.result = "pass";
                        }
                        else
                        {
                            testCase.result = "fail";
                        }
                    }

                    // If TC has a mac, change it
                    if (testCase.mac != null)
                    {
                        BitString bs = new BitString(testCase.mac.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);

                        // Can't get something "different" of empty bitstring of the same length
                        if (bs == null)
                        {
                            bs = new BitString("01");
                        }

                        testCase.mac = bs.ToHex();
                    }
                }
            }

            // Write the new JSON to the results file
            File.Delete(resultsFile);
            File.WriteAllText(resultsFile, parsedValidation.ParsedObject.ToString());

            return failedTestCases;
        }

        private string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "CMAC-AES-128",
                Direction = new string[] { "gen", "ver" },
                MsgLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                MacLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(128))
                    .AddSegment(new ValueDomainSegment(127)),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }
        
        private string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Random800_90 random = new Random800_90();
            
            Parameters p = new Parameters()
            {
                Algorithm = "CMAC-AES-128",
                Direction = ParameterValidator.VALID_DIRECTIONS,
                MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 0, 65536, 8)),
                MacLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 64, 128, 8)),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        private static string CreateRegistration(string targetFolder, Parameters parameters)
        {
            var json = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new BitstringConverter(),
                    new DomainConverter()
                },
                Formatting = Formatting.Indented
            });
            string fileName = $"{targetFolder}\\registration.json";
            File.WriteAllText(fileName, json);

            return fileName;
        }

    }
}
