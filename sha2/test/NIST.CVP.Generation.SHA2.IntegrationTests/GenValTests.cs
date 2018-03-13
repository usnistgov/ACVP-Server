using System.Collections.Generic;
using Autofac;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "SHA2";
        public override string Mode { get; } = string.Empty;

        public override Executable Generator => GenValApp.Program.Main;
        public override Executable Validator => GenValApp.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeVectorSetDeserializerException<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.md != null)
            {
                var bs = new BitString(testCase.md.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);
                testCase.md = bs.ToHex();
            }

            if (testCase.msg != null)
            {
                var bs = new BitString(testCase.msg.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);
                testCase.msg = bs.ToHex();
            }

            if (testCase.resultsArray != null)
            {
                var bsMessage = new BitString(testCase.resultsArray[0].msg.ToString());
                bsMessage = rand.GetDifferentBitStringOfSameSize(bsMessage);
                testCase.resultsArray[0].msg = bsMessage.ToHex();

                var bsDigest = new BitString(testCase.resultsArray[0].md.ToString());
                bsDigest = rand.GetDifferentBitStringOfSameSize(bsDigest);
                testCase.resultsArray[0].md = bsDigest.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            RemoveMCTTestGroupFactories();

            var parameters = new Parameters
            {
                Algorithm = "SHA1",
                DigestSizes = new[] {"160"},
                BitOriented = true,
                IncludeNull = true,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            RemoveMCTTestGroupFactories();

            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                DigestSizes = new[] {"224"},
                BitOriented = false,
                IncludeNull = false,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                DigestSizes = new[] { "224", "256", "384", "512", "512/224", "512/256" },
                BitOriented = true,
                IncludeNull = true,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        /// <summary>
        /// Can be used to exclude MCT tests
        /// </summary>
        public class FakeTestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
        {
            public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators()
            {
                return new List<ITestGroupGenerator<Parameters, TestGroup, TestCase>>()
                {
                    new TestGroupGeneratorAlgorithmFunctionalTest()
                };
            }
        }

        private void RemoveMCTTestGroupFactories()
        {
            AutofacConfig.OverrideRegistrations += builder =>
            {
                builder.RegisterType<FakeTestGroupGeneratorFactory>().AsImplementedInterfaces();
            };
        }
    }
}
