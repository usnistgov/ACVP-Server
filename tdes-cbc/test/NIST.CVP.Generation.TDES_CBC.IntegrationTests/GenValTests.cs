using System.Collections.Generic;
using Autofac;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using tdes_cbc;

namespace NIST.CVP.Generation.TDES_CBC.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm { get; } = "TDES";
        public override string Mode { get; } = "CBC";

        public override Executable Generator => Program.Main;
        public override Executable Validator => TDES_CBC_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
            TDES_CBC_Val.AutofacConfig.OverrideRegistrations = null;
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
            TDES_CBC_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            TDES_CBC_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();
            // If TC is intended to be a failure test, change it
            if (testCase.decryptFail != null)
            {
                testCase.decryptFail = false;
            }

            // If TC has a cipherText, change it
            if (testCase.cipherText != null)
            {
                var bs = new BitString(testCase.cipherText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.cipherText = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.plainText != null)
            {
                var bs = new BitString(testCase.plainText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.plainText = bs.ToHex();
            }

            // If TC has a resultsArray, change some of the elements
            if (testCase.resultsArray != null)
            {
                var bsIV = new BitString(testCase.resultsArray[0].iv.ToString());
                bsIV = rand.GetDifferentBitStringOfSameSize(bsIV);
                testCase.resultsArray[0].iv = bsIV.ToHex();

                var bsKey1 = new BitString(testCase.resultsArray[0].key1.ToString());
                bsKey1 = rand.GetDifferentBitStringOfSameSize(bsKey1);
                testCase.resultsArray[0].key1 = bsKey1.ToHex();

                var bsKey2 = new BitString(testCase.resultsArray[0].key2.ToString());
                bsKey2 = rand.GetDifferentBitStringOfSameSize(bsKey2);
                testCase.resultsArray[0].key2 = bsKey2.ToHex();

                var bsKey3 = new BitString(testCase.resultsArray[0].key3.ToString());
                bsKey3 = rand.GetDifferentBitStringOfSameSize(bsKey3);
                testCase.resultsArray[0].key3 = bsKey3.ToHex();

                var bsPlainText = new BitString(testCase.resultsArray[0].plainText.ToString());
                bsPlainText = rand.GetDifferentBitStringOfSameSize(bsPlainText);
                testCase.resultsArray[0].plainText = bsPlainText.ToHex();

                var bsCipherText = new BitString(testCase.resultsArray[0].cipherText.ToString());
                bsCipherText = rand.GetDifferentBitStringOfSameSize(bsCipherText);
                testCase.resultsArray[0].cipherText = bsCipherText.ToHex();
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
                Direction = new [] { "encrypt" },
                IsSample = true,
                KeyingOption = new[] { 1 }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                IsSample = false,
                KeyingOption = new []{1}
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
