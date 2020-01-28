using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.DSA.v1_0.SigVer;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "DSA";
        public override string Mode { get; } = "SigVer";

        public override AlgoMode AlgoMode => AlgoMode.DSA_SigVer_v1_0;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            // If TC has a result, change it
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
                Revision = Revision,
                IsSample = true,
                Capabilities = caps,
                Conformances = new[] { "SP800-106" }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var caps = new Capability[4];

            caps[0] = new Capability
            {
                L = 1024,
                N = 160,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
            };

            caps[1] = new Capability
            {
                L = 2048,
                N = 224,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
            };

            caps[2] = new Capability
            {
                L = 2048,
                N = 256,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
            };

            caps[3] = new Capability
            {
                L = 3072,
                N = 256,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = false,
                Capabilities = caps,
                Conformances = new[] { "SP800-106" }
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
