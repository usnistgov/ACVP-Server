using Autofac;
using DSA_SigGen;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.SigGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm { get; } = "DSA";
        public override string Mode { get; } = "SigGen";

        public override Executable Generator => Program.Main;
        public override Executable Validator => DSA_SigGen_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
            DSA_SigGen_Val.AutofacConfig.OverrideRegistrations = null;
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
            DSA_SigGen_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            DSA_SigGen_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a result, change it
            if (testCase.r != null)
            {
                testCase.r = rand.GetDifferentBitStringOfSameSize(new BitString((string)testCase.r)).ToHex();
            }

            if (testCase.s != null)
            {
                testCase.s = rand.GetDifferentBitStringOfSameSize(new BitString((string)testCase.s)).ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            var caps = new Capability[1];

            caps[0] = new Capability
            {
                L = 2048,
                N = 224,
                HashAlg = new[] { "sha2-256" }
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
            var caps = new Capability[2];

            caps[0] = new Capability
            {
                L = 2048,
                N = 224,
                HashAlg = new[] { "sha2-256" }
            };

            caps[1] = new Capability
            {
                L = 2048,
                N = 256,
                HashAlg = new[] { "sha2-224", "sha2-512" }
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
            var caps = new Capability[3];

            caps[0] = new Capability
            {
                L = 2048,
                N = 224,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
            };

            caps[1] = new Capability
            {
                L = 2048,
                N = 256,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
            };

            caps[2] = new Capability
            {
                L = 3072,
                N = 256,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
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
    }
}
