using System;
using System.IO;
using Autofac;
using KAS;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.SHA2;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests
    {
        string _testPath;
        private const string _DEFAULT_ALGO = "KAS-FFC";
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
            KAS_Val.AutofacConfig.OverrideRegistrations = null;
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
            var result = Program.Main(new string[] { _DEFAULT_ALGO, $"{Guid.NewGuid()}.json" });

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldReturn1OnFailedRun()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };

            var targetFolder = Utilities.GetTestFolder(_testPath, "genFail");
            var fileName = GetTestFileFewTestCases(targetFolder);

            var result = Program.Main(new string[] { _DEFAULT_ALGO, fileName });

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldReturn1OnException()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionParameterParser<Parameters>>().AsImplementedInterfaces();
            };

            var targetFolder = Utilities.GetTestFolder(_testPath, "genException");
            var fileName = GetTestFileFewTestCases(targetFolder);

            var result = Program.Main(new string[] { _DEFAULT_ALGO, fileName });

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GenShouldCreateTestVectors()
        {
            var targetFolder = Utilities.GetTestFolder(_testPath, "genTestVectors");
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);

            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), "testResults");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), "prompt");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), "answer");
        }

        [Test]
        public void ValShouldReturn1OnFailedRun()
        {
            KAS_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = Utilities.GetTestFolder(_testPath, "valFail");
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = KAS_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames)
            );

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ValShouldReturn1OnException()
        {
            KAS_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };

            var targetFolder = Utilities.GetTestFolder(_testPath, "valException");
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGeneration(targetFolder, fileName);

            var result = KAS_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames)
            );

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldCreateValidationFile()
        {
            var targetFolder = Utilities.GetTestFolder(_testPath, "valCreate");
            var fileName = GetTestFileFewTestCases(targetFolder);

            RunGenerationAndValidation(targetFolder, fileName);

            Assert.IsTrue(File.Exists($"{targetFolder}\\validation.json"), "validation");
        }
        
        [Test]
        public void ShouldReportAllSuccessfulTestsWithinValidationLotsOfTests()
        {
            var targetFolder = Utilities.GetTestFolder(_testPath, "valSuccessManyCases");
            var fileName = GetTestFileLotsOfTestCases(targetFolder);

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
            var targetFolder = Utilities.GetTestFolder(_testPath, "valFailedDisposition");
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

        private string[] GetFileNamesWithPath(string directory, string[] fileNames)
        {
            int numOfFiles = fileNames.Length;
            string[] appArgs = new string[numOfFiles];

            for (int i = 0; i < numOfFiles; i++)
            {
                appArgs[i] = $"{directory}{fileNames[i]}";
            };

            return appArgs.Prepend(_DEFAULT_ALGO).ToArray();
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
            var result = Program.Main(new string[] { _DEFAULT_ALGO, fileName });
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[0]}"), $"{targetFolder}{_testVectorFileNames[0]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[1]}"), $"{targetFolder}{_testVectorFileNames[1]}");
            Assert.IsTrue(File.Exists($"{targetFolder}{_testVectorFileNames[2]}"), $"{targetFolder}{_testVectorFileNames[2]}");
            Assert.IsTrue(result == 0);
        }

        private void RunValidation(string targetFolder)
        {
            // Run test vector validation
            var result = KAS_Val.Program.Main(
                GetFileNamesWithPath(targetFolder, _testVectorFileNames)
            );
            Assert.IsTrue(File.Exists($"{targetFolder}\\validation.json"), $"{targetFolder}validation");
            Assert.IsTrue(result == 0);
        }

        private void GetFailureTestCases(string targetFolder, ref List<int> failureTcIds)
        {
            var files = GetFileNamesWithPath(targetFolder, _testVectorFileNames);

            // Modify testResults in order to contain some tests that will fail
            var expectedFailTestCases = DoBadThingsToResultsFile(files[1]);
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

                    // If TC has a hashZIut, change it
                    if (testCase.hashZIut != null)
                    {
                        BitString bs = new BitString(testCase.hashZIut.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);

                        // Can't get something "different" of empty bitstring of the same length
                        if (bs == null)
                        {
                            bs = new BitString("01");
                        }

                        testCase.hashZIut = bs.ToHex();
                    }
                    // If TC has a tagIut, change it
                    if (testCase.tagIut != null)
                    {
                        BitString bs = new BitString(testCase.tagIut.ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);

                        // Can't get something "different" of empty bitstring of the same length
                        if (bs == null)
                        {
                            bs = new BitString("01");
                        }

                        testCase.tagIut = bs.ToHex();
                    }
                    // If TC has a result, change it
                    if (testCase.result != null)
                    {
                        testCase.result = testCase.result.ToString().Equals("pass") ? "fail" : "pass";
                        
                    }
                }
            }

            // Write the new JSON to the results file
            File.Delete(resultsFile);
            File.WriteAllText(resultsFile, parsedValidation.ParsedObject.ToString());

            return failedTestCases;
        }

        private string GetTestFileFewTestCases(string targetFolder, string algoName = "KAS-FFC")
        {
            Parameters p = new Parameters()
            {
                Algorithm = algoName,
                Function = new string [] {"dpGen"},
                Scheme = new Schemes()
                {
                    DhEphem = new DhEphem()
                    {
                        Role = new string[] { "initiator"},
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] {"SHA2-224"}
                                }
                            }
                        }
                    }
                },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        private string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = _DEFAULT_ALGO,
                Function = new string[] { "dpGen", "dpVal", "keyPairGen", "fullVal", "keyRegen" },
                Scheme = new Schemes()
                {
                    DhEphem = new DhEphem()
                    {
                        Role = new string[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] { "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512" }
                                },
                                Fc = new Fc()
                                {
                                    HashAlg = new string[] { "SHA2-256", "SHA2-384", "SHA2-512" }
                                }
                            }
                        },
                        KdfNoKc = new KdfNoKc()
                        {
                            KdfOption = new KdfOptions()
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] { "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512" },
                                    MacOption = new MacOptions()
                                    {
                                        AesCcm = new MacOptionAesCcm()
                                        {
                                            KeyLen = new int[] { 128, 192, 256 },
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac()
                                        {
                                            KeyLen = new int[] { 128, 192, 256 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D256 = new MacOptionHmacSha2_d256()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D384 = new MacOptionHmacSha2_d384()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D512 = new MacOptionHmacSha2_d512()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        }
                                    }
                                },
                                Fc = new Fc()
                                {
                                    HashAlg = new string[] { "SHA2-256", "SHA2-384", "SHA2-512" },
                                    MacOption = new MacOptions()
                                    {
                                        AesCcm = new MacOptionAesCcm()
                                        {
                                            KeyLen = new int[] { 128, 192, 256 },
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac()
                                        {
                                            KeyLen = new int[] { 128, 192, 256 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D256 = new MacOptionHmacSha2_d256()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D384 = new MacOptionHmacSha2_d384()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        },
                                        HmacSha2_D512 = new MacOptionHmacSha2_d512()
                                        {
                                            KeyLen = new int[] { 128 },
                                            MacLen = 128
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                IsSample = true
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