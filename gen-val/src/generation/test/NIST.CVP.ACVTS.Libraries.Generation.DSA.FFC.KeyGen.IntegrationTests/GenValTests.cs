using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.FFC.KeyGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "DSA";
        public override string Mode { get; } = "keyGen";

        public override AlgoMode AlgoMode => AlgoMode.DSA_KeyGen_v1_0;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a result, change it
            if (testCase.x != null)
            {
                testCase.x = rand.GetDifferentBitStringOfSameSize(new BitString((string)testCase.x)).ToHex();
            }

            if (testCase.y != null)
            {
                testCase.y = rand.GetDifferentBitStringOfSameSize(new BitString((string)testCase.y)).ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var caps = new Capability[2];

            caps[0] = new Capability
            {
                L = 2048,
                N = 224
            };

            caps[1] = new Capability
            {
                L = 2048,
                N = 256
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
                N = 224
            };

            caps[1] = new Capability
            {
                L = 2048,
                N = 256
            };

            caps[2] = new Capability
            {
                L = 3072,
                N = 256
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
