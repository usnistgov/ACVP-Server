using Autofac;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using TDES_CBCI;

namespace NIST.CVP.Generation.TDES_CBCI.IntegrationTests
{

    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {

        public override string Algorithm { get; } = "TDES";
        public override string Mode { get; } = "CBCI";

        public override Executable Generator => Program.Main;
        public override Executable Validator => TDES_CBCI_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
            TDES_CBCI_Val.AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            TDES_CBCI_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            TDES_CBCI_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            //TODO the duplication between testCase and resultArray should be refactored
            var rand = new Random800_90();
            if (testCase.decryptFail != null)
            {
                testCase.decryptFail = false;
            }

            var propertiesToScramble = new[] { "ct", "ct1", "ct2", "ct3", "pt", "pt1", "pt2", "pt3" };
            foreach (var prop in propertiesToScramble)
            {
                if (testCase[prop] != null)
                {
                    var bs = new BitString(testCase[prop].ToString());
                    bs = rand.GetDifferentBitStringOfSameSize(bs);
                    testCase[prop] = bs.ToHex();
                }
            }

            // If TC has a resultsArray, change some of the elements
            if (testCase.resultsArray != null)
            {
                var arrayPropertiesToScramble = new[] { "ct", "pt", "iv1", "iv2", "iv3", "key1", "key2", "key3" };
                foreach (var prop in arrayPropertiesToScramble)
                {
                    if (testCase.resultsArray[0][prop] != null)
                    {
                        var bs = new BitString(testCase.resultsArray[0][prop].ToString());
                        bs = rand.GetDifferentBitStringOfSameSize(bs);
                        testCase.resultsArray[0][prop] = bs.ToHex();
                    }
                }
            }
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            return GetTestFileFewTestCases(targetFolder);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            RemoveMCTAndKATTestGroupFactories();

            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = new[] { "encrypt" },
                IsSample = true,
                KeyingOption = new[] { 1 }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                IsSample = false,
                KeyingOption = new[] { 1 }
            };

            return CreateRegistration(targetFolder, p);
        }

        /// <summary>
        /// Can be used to only generate MMT groups for the genval tests
        /// </summary>
        public class FakeTestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
        {
            public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
            {
                return new List<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGeneratorMultiblockMessage()
                };
            }
        }

        private void RemoveMCTAndKATTestGroupFactories()
        {
            AutofacConfig.OverrideRegistrations += builder =>
            {
                builder.RegisterType<FakeTestGroupGeneratorFactory>().AsImplementedInterfaces();
            };
        }
    }
}

