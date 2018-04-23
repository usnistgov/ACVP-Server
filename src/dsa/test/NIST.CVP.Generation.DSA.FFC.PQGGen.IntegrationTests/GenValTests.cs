using Autofac;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "DSA";
        public override string Mode { get; } = "PQGGen";

        public override AlgoMode AlgoMode => AlgoMode.DSA_PQGGen;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();
            
            // If TC has a result, change it
            if (testCase.g != null)
            {
                var gLen = ((string)testCase.g).Length * 4;
                testCase.g = rand.GetRandomBitString(gLen).ToHex();
            }

            if (testCase.p != null)
            {
                var pLen = ((string)testCase.p).Length * 4;
                testCase.p = rand.GetRandomBitString(pLen).ToHex();
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
                HashAlg = new[] { "sha2-224", "sha2-512/224" }
            };

            caps[1] = new Capability
            {
                PQGen = new[] { "probable" },
                GGen = new[] { "unverifiable" },
                L = 2048,
                N = 256,
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

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var caps = new Capability[3];

            caps[0] = new Capability
            {
                PQGen = new[] { "probable", "provable" },
                GGen = new[] { "unverifiable", "canonical" },
                L = 2048,
                N = 224,
                HashAlg = new[] { "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256" }
            };

            caps[1] = new Capability
            {
                PQGen = new[] { "probable", "provable" },
                GGen = new[] { "unverifiable", "canonical" },
                L = 2048,
                N = 256,
                HashAlg = new[] { "sha2-256", "sha2-384", "sha2-512", "sha2-512/256" }
            };

            caps[2] = new Capability
            {
                PQGen = new[] { "probable", "provable" },
                GGen = new[] { "unverifiable", "canonical" },
                L = 3072,
                N = 256,
                HashAlg = new[] { "sha2-256", "sha2-384", "sha2-512", "sha2-512/256" }
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
