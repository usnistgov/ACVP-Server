using Autofac;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm { get; } = "ECDSA";
        public override string Mode { get; } = "KeyGen";

        public override Executable Generator => ECDSA_KeyGen.Program.Main;
        public override Executable Validator => ECDSA_KeyGen_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            ECDSA_KeyGen.AutofacConfig.OverrideRegistrations = null;
            ECDSA_KeyGen_Val.AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            ECDSA_KeyGen.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            ECDSA_KeyGen_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            ECDSA_KeyGen_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();
            if (testCase.qx != null)
            {
                testCase.qx = rand.GetDifferentBitStringOfSameSize(new BitString(testCase.qx.ToString())).ToHex();
            }

            if (testCase.qy != null)
            {
                testCase.qy = rand.GetDifferentBitStringOfSameSize(new BitString(testCase.qy.ToString())).ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = true,
                Curve = new [] {"p-224"},
                SecretGenerationMode = new [] {"testing candidates"}
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
                Curve = new string[] { "p-224", "b-233", "k-233" },
                SecretGenerationMode = new string[] { "testing candidates" }
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
                Curve = ParameterValidator.VALID_CURVES,
                SecretGenerationMode = ParameterValidator.VALID_SECRET_GENERATION_MODES
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
