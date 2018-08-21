using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.Ed.KeyGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "EDDSA";
        public override string Mode { get; } = "KeyGen";


        public override AlgoMode AlgoMode => AlgoMode.EDDSA_KeyGen;

        public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();
            if (testCase.q != null)
            {
                testCase.q = rand.GetDifferentBitStringOfSameSize(new BitString(testCase.q.ToString())).ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = true,
                Curve = new string[] { "ed-25519" },
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
