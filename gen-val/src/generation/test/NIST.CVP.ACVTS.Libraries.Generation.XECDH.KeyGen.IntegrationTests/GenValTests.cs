using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyGen;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.KeyGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "XECDH";
        public override string Mode { get; } = "keyGen";
        public override string Revision { get; set; } = "RFC7748";

        public override AlgoMode AlgoMode => AlgoMode.XECDH_KeyGen_RFC7748;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();
            if (testCase.publicKey != null)
            {
                testCase.publicKey = rand.GetDifferentBitStringOfSameSize(new BitString(testCase.publicKey.ToString())).ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Curve = new[] { "Curve25519" }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Curve = ParameterValidator.VALID_CURVES
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
