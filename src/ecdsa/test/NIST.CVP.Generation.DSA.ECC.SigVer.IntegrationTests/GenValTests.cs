using Autofac;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "ECDSA";
        public override string Mode { get; } = "SigVer";

        public override AlgoMode AlgoMode => AlgoMode.ECDSA_SigVer;

        public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

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
            if (testCase.testPassed != null)
            {
                if (testCase.testPassed == true)
                {
                    testCase.testPassed = false;
                }
                else
                {
                    testCase.testPassed = true;
                }
            }
        }
    }
}
