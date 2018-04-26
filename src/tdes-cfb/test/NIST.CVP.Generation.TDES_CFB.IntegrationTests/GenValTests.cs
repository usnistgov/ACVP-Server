using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using tdes_cfb;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES_CFB;
using NIST.CVP.Generation.TDES_CFB;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;


namespace NIST.CVP.Generation.TDES_CFB.IntegrationTests
{
    [TestFixture, FastIntegrationTest, UnitTest]
    public class GenValTestBase
    {
        protected string _testPath;
        protected string[] _testVectorFileNames = new string[]
        {
            "\\testResults.json",
            "\\prompt.json",
            "\\answer.json"
        };

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\temp_integrationTests\");

            tdes_cfb.AutofacConfig.OverrideRegistrations = null;
            TDES_CFB_Val.AutofacConfig.OverrideRegistrations = null;
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            //Directory.Delete(_testPath, true);
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
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void GenShouldReturn1OnFailedRun(Algo algo)
        {
            tdes_cfb.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder, algo);

            var result = Program.Main(new string[] { fileName });

            Assert.AreEqual(1, result);
        }

        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void GenShouldReturn1OnException(Algo algo)
        {
            tdes_cfb.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionParameterParser<Parameters>>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder, algo);

            var result = Program.Main(new string[] { fileName });

            Assert.AreEqual(1, result);
        }

        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void GenShouldCreateTestVectors(Algo algo)
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder, algo);

            RunGeneration(targetFolder, fileName, algo);

            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), "testResults");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), "prompt");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), "answer");
        }

        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void ValShouldReturn1OnFailedRun(Algo algo)
        {
            TDES_CFB_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder, algo);

            RunGeneration(targetFolder, fileName, algo);

            var result = TDES_CFB_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames).Prepend(EnumHelpers.GetEnumDescriptionFromEnum(algo)).ToArray()
            );

            Assert.AreEqual(1, result);
        }

        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void ValShouldReturn1OnException(Algo algo)
        {
            TDES_CFB_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder, algo);

            RunGeneration(targetFolder, fileName, algo);

            var result = TDES_CFB_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames).Prepend(EnumHelpers.GetEnumDescriptionFromEnum(algo)).ToArray()
            );

            Assert.AreEqual(1, result);
        }


        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void ShouldCreateValidationFile(Algo algo)
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder, algo);

            RunGenerationAndValidation(targetFolder, fileName, algo);

            Assert.IsTrue(File.Exists($"{targetFolder}\\validation.json"), "validation");
        }

        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void ShouldReportAllSuccessfulTestsWithinValidationFewTestCases(Algo algo)
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder, algo);

            RunGenerationAndValidation(targetFolder, fileName, algo);

            // Get object for the validation.json
            DynamicParser dp = new DynamicParser();
            var parsedValidation = dp.Parse($"{targetFolder}\\validation.json");

            // Validate result as pass
            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }


        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void ShouldReportAllSuccessfulTestsWithinValidationLotsOfTests(Algo algo)
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileLotsOfTestCases(targetFolder, algo);

            RunGenerationAndValidation(targetFolder, fileName, algo);

            // Get object for the validation.json
            DynamicParser dp = new DynamicParser();
            var parsedValidation = dp.Parse($"{targetFolder}\\validation.json");

            // Validate result as pass
            Assert.AreEqual("passed", parsedValidation.ParsedObject.disposition.ToString());
        }

        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void ShouldReportFailedDispositionOnErrorTests(Algo algo)
        {
            var targetFolder = GetTestFolder();
            var fileName = GetTestFileFewTestCases(targetFolder, algo);

            List<int> expectedFailTestCases = new List<int>();
            RunGenerationAndValidationWithExpectedFailures(targetFolder, fileName, ref expectedFailTestCases, algo);

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

        protected string[] GetFileNamesWithPath(string directory, string[] fileNames)
        {
            int numOfFiles = fileNames.Length;
            string[] fileNamesWithPaths = new string[numOfFiles];

            for (int i = 0; i < numOfFiles; i++)
            {
                fileNamesWithPaths[i] = $"{directory}{fileNames[i]}";
            };

            return fileNamesWithPaths;
        }

        protected static string CreateRegistration(string targetFolder, IParameters parameters)
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
        private void RunGeneration(string targetFolder, string fileName, Algo algo)
        {
            // Run test vector generation
            var result = Program.Main(new string[] { EnumHelpers.GetEnumDescriptionFromEnum(algo), fileName });
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), $"{targetFolder}{_testVectorFileNames[0]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), $"{targetFolder}{_testVectorFileNames[1]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), $"{targetFolder}{_testVectorFileNames[2]}");
            Assert.IsTrue(result == 0);
        }

        private void RunGenerationAndValidation(string targetFolder, string fileName, Algo algo)
        {
            RunGeneration(targetFolder, fileName, algo);
            RunValidation(targetFolder, algo);
        }

        private void RunGenerationAndValidationWithExpectedFailures(string targetFolder, string fileName, ref List<int> failureTcIds, Algo algo)
        {
            RunGeneration(targetFolder, fileName, algo);
            GetFailureTestCases(targetFolder, ref failureTcIds);
            RunValidation(targetFolder, algo);
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
                    if (testCase.ct != null)
                    {
                        BitString bs = new BitString(testCase.ct.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);

                        testCase.ct = bs.ToHex();
                    }

                    // If TC has a plainText, change it
                    if (testCase.pt != null)
                    {
                        BitString bs = new BitString(testCase.pt.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);

                        testCase.pt = bs.ToHex();
                    }

                    // If TC has a resultsArray, change some of the elements
                    if (testCase.resultsArray != null)
                    {
                        BitString bsIV = new BitString(testCase.resultsArray[0].iv.ToString());
                        bsIV = rand.GetDifferentBitStringOfSameSize(bsIV);
                        testCase.resultsArray[0].iv = bsIV.ToHex();

                        BitString bsKey1 = new BitString(testCase.resultsArray[0].key1.ToString());
                        bsKey1 = rand.GetDifferentBitStringOfSameSize(bsKey1);
                        testCase.resultsArray[0].key1 = bsKey1.ToHex();

                        BitString bsKey2 = new BitString(testCase.resultsArray[0].key2.ToString());
                        bsKey2 = rand.GetDifferentBitStringOfSameSize(bsKey2);
                        testCase.resultsArray[0].key2 = bsKey2.ToHex();

                        BitString bsKey3 = new BitString(testCase.resultsArray[0].key3.ToString());
                        bsKey3 = rand.GetDifferentBitStringOfSameSize(bsKey3);
                        testCase.resultsArray[0].key3 = bsKey3.ToHex();

                        BitString bsPlainText = new BitString(testCase.resultsArray[0].pt.ToString());
                        bsPlainText = rand.GetDifferentBitStringOfSameSize(bsPlainText);
                        testCase.resultsArray[0].pt = bsPlainText.ToHex();

                        BitString bsCipherText = new BitString(testCase.resultsArray[0].ct.ToString());
                        bsCipherText = rand.GetDifferentBitStringOfSameSize(bsCipherText);
                        testCase.resultsArray[0].ct = bsCipherText.ToHex();
                    }
                }
            }

            // Write the new JSON to the results file
            File.Delete(resultsFile);
            File.WriteAllText(resultsFile, parsedValidation.ParsedObject.ToString());

            return failedTestCases;
        }

        private string GetTestFolder()
        {
            var targetFolder = Path.Combine(_testPath, Guid.NewGuid().ToString());
            Directory.CreateDirectory(targetFolder);

            return targetFolder;
        }

        private void RunValidation(string targetFolder, Algo algo)
        {
            // Run test vector validation
            var result = TDES_CFB_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames).Prepend(EnumHelpers.GetEnumDescriptionFromEnum(algo)).ToArray());
            Assert.IsTrue(File.Exists($"{targetFolder}\\validation.json"), $"{targetFolder}validation");
            Assert.IsTrue(result == 0);
        }

        //protected abstract int ExecuteMainGenerator(string fileName);
        //protected abstract int ExecuteMainValidator(string fileName);
        protected string GetTestFileFewTestCases(string targetFolder, Algo algo)
        {
            Parameters p = new Parameters()
            {
                Algorithm = EnumHelpers.GetEnumDescriptionFromEnum(algo),
                Direction = ParameterValidator.VALID_DIRECTIONS,
                IsSample = false,
                KeyingOption = new[] { 1 }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected string GetTestFileLotsOfTestCases(string targetFolder, Algo algo)
        {
            Random800_90 random = new Random800_90();

            Parameters p = new Parameters()
            {
                Algorithm = EnumHelpers.GetEnumDescriptionFromEnum(algo),
                Direction = ParameterValidator.VALID_DIRECTIONS,
                //Direction = new []{ "decrypt" },
                IsSample = false,
                KeyingOption = new[] { 1, 2 }
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
