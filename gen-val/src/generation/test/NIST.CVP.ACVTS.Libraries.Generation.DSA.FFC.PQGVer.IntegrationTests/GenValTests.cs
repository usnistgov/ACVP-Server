using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.FFC.PQGVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "DSA";
        public override string Mode { get; } = "pqgVer";

        public override AlgoMode AlgoMode => AlgoMode.DSA_PQGVer_v1_0;


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
                PQGen = new[] { "provable" },
                GGen = new[] { "canonical" },
                L = 2048,
                N = 224,
                HashAlg = new[] { "SHA2-224" }
            };

            caps[1] = new Capability
            {
                PQGen = new[] { "probable" },
                GGen = new[] { "unverifiable" },
                L = 2048,
                N = 256,
                HashAlg = new[] { "SHA2-256" }
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Capabilities = caps,
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var caps = new Capability[4];

            caps[0] = new Capability
            {
                PQGen = new[] { "probable", "provable" },
                GGen = new[] { "unverifiable", "canonical" },
                L = 1024,
                N = 160,
                HashAlg = new[] { "SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256" }
            };

            caps[1] = new Capability
            {
                PQGen = new[] { "probable", "provable" },
                GGen = new[] { "unverifiable", "canonical" },
                L = 2048,
                N = 224,
                HashAlg = new[] { "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256" }
            };

            caps[2] = new Capability
            {
                PQGen = new[] { "probable", "provable" },
                GGen = new[] { "unverifiable", "canonical" },
                L = 2048,
                N = 256,
                HashAlg = new[] { "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/256" }
            };

            caps[3] = new Capability
            {
                PQGen = new[] { "probable", "provable" },
                GGen = new[] { "unverifiable", "canonical" },
                L = 3072,
                N = 256,
                HashAlg = new[] { "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/256" }
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = false,
                Capabilities = caps,
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
