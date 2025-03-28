using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.SSC;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.SSCKeyVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        private readonly Random800_90 _random = new Random800_90();
        public override string Algorithm { get; } = "XECDH";
        public override string Mode { get; } = "SSC";
        public override string Revision { get; set; } = "RFC7748";

        public override AlgoMode AlgoMode => AlgoMode.XECDH_SSC_RFC7748;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            // If TC has a z, change it
            if (testCase.z != null)
            {
                BitString bs = new BitString(testCase.z.ToString());
                bs = _random.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.z = bs.ToHex();
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
