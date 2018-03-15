using Autofac;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "DSA";
        public override string Mode { get; } = "PQGVer";

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

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            var caps = new Capability[1];

            caps[0] = new Capability
            {
                PQGen = new[] { "probable" },
                GGen = new[] { "canonical" },
                L = 2048,
                N = 224,
                HashAlgs = new[] { "sha2-256" }
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
                PQGen = new[] { "provable" },
                GGen = new[] { "canonical" },
                L = 2048,
                N = 224,
                HashAlgs = new[] { "sha2-224" }
            };

            caps[1] = new Capability
            {
                PQGen = new[] { "probable" },
                GGen = new[] { "unverifiable" },
                L = 2048,
                N = 256,
                HashAlgs = new[] { "sha2-256" }
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
            var caps = new Capability[4];

            caps[0] = new Capability
            {
                PQGen = new[] { "probable", "provable" },
                GGen = new[] { "unverifiable", "canonical" },
                L = 1024,
                N = 160,
                HashAlgs = new[] { "sha-1", "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256" }
            };

            caps[1] = new Capability
            {
                PQGen = new[] { "probable", "provable" },
                GGen = new[] { "unverifiable", "canonical" },
                L = 2048,
                N = 224,
                HashAlgs = new[] { "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256" }
            };

            caps[2] = new Capability
            {
                PQGen = new[] { "probable", "provable" },
                GGen = new[] { "unverifiable", "canonical" },
                L = 2048,
                N = 256,
                HashAlgs = new[] { "sha2-256", "sha2-384", "sha2-512", "sha2-512/256" }
            };

            caps[3] = new Capability
            {
                PQGen = new[] { "probable", "provable" },
                GGen = new[] { "unverifiable", "canonical" },
                L = 3072,
                N = 256,
                HashAlgs = new[] { "sha2-256", "sha2-384", "sha2-512", "sha2-512/256" }
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = false,
                Capabilities = caps,
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
