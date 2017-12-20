using Autofac;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm { get; } = "ECDSA";
        public override string Mode { get; } = "KeyVer";

        public override Executable Generator => ECDSA_KeyVer.Program.Main;
        public override Executable Validator => ECDSA_KeyVer_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            ECDSA_KeyVer.AutofacConfig.OverrideRegistrations = null;
            ECDSA_KeyVer_Val.AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            ECDSA_KeyVer.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            ECDSA_KeyVer_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            ECDSA_KeyVer_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            if (testCase.result == null) return;
            var previousValue = EnumHelpers.GetEnumFromEnumDescription<Disposition>(testCase.result.ToString(), false);
            var newValue = previousValue == Disposition.Passed ? Disposition.Failed : Disposition.Passed;
            testCase.result = EnumHelpers.GetEnumDescriptionFromEnum(newValue);
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = true,
                Curve = new [] { "p-224" }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = true,
                Curve = new [] { "p-224", "b-233", "k-233" }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = true,
                Curve = ParameterValidator.VALID_CURVES
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
