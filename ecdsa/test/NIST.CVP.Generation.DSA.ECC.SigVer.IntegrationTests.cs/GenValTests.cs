using Autofac;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.IntegrationTests.cs
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm { get; } = "ECDSA";
        public override string Mode { get; } = "SigVer";

        public override Executable Generator => ECDSA_SigVer.Program.Main;
        public override Executable Validator => ECDSA_SigVer_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            ECDSA_SigVer.AutofacConfig.OverrideRegistrations = null;
            ECDSA_SigVer.AutofacConfig.IoCConfiguration();

            ECDSA_SigVer_Val.AutofacConfig.OverrideRegistrations = null;
            ECDSA_SigVer_Val.AutofacConfig.IoCConfiguration();
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            ECDSA_SigVer.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            ECDSA_SigVer_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            ECDSA_SigVer_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            var caps = new []
            {
                new Capability
                {
                    Curve = new[] { "p-224" },
                    HashAlg = new[] { "sha2-224" }
                }
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = true,
                Capabilities = caps,
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var caps = new []
            {
                new Capability
                {
                    Curve = new[] { "p-224", "b-233" },
                    HashAlg = new[] { "sha2-224" }
                },
                new Capability
                {
                    Curve = new[] { "k-283" },
                    HashAlg = new[] {"sha2-512" }
                }
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = true,
                Capabilities = caps,
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var caps = new []
            {
                new Capability
                {
                    Curve = ParameterValidator.VALID_CURVES,
                    HashAlg = ParameterValidator.VALID_HASH_ALGS
                }
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = true,
                Capabilities = caps,
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            if (testCase.result == null) return;
            var previousValue = EnumHelpers.GetEnumFromEnumDescription<Disposition>(testCase.result.ToString(), false);
            var newValue = previousValue == Disposition.Passed ? Disposition.Failed : Disposition.Passed;
            testCase.result = EnumHelpers.GetEnumDescriptionFromEnum(newValue);
        }
    }
}
