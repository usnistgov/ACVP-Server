using Autofac;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CTR.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "TDES";
        public override string Mode => "CTR";

        public override Executable Generator => GenValApp.Program.Main;
        public override Executable Validator => GenValApp.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            // SaveJson = true;
            AutofacConfig.OverrideRegistrations = null;
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = new[] { "encrypt", "decrypt" },
                KeyingOption = new[] { 1, 2 },
                IncrementalCounter = false,
                OverflowCounter = false,
                DataLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 64, 8)),
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = new[] { "encrypt", "decrypt" },
                KeyingOption = new[] { 1, 2 },
                IncrementalCounter = false,
                OverflowCounter = true,
                DataLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 1, 64)),
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = new[] {"encrypt"},
                KeyingOption = new[] {1},
                IncrementalCounter = true,
                OverflowCounter = false,
                DataLength = new MathDomain().AddSegment(new ValueDomainSegment(64)),
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

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
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }
    }
}
