using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Tests;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using ParameterValidator = NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v2_0.ParameterValidator;
using RegisterInjections = NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v2_0.RegisterInjections;

namespace NIST.CVP.ACVTS.Libraries.Generation.TLS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests_v1_2_Extended : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "TLS-v1.2";
        public override string Mode => "KDF";
        public override string Revision => "RFC7627";

        public override AlgoMode AlgoMode => AlgoMode.Tls_v1_2_RFC7627;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();


        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.masterSecret != null)
            {
                var bs = new BitString(testCase.masterSecret.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.masterSecret = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                HashAlg = new[] { "SHA2-256", "SHA2-512" },
                IsSample = true
                // Uses default KeyBlockLength of 1024
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                HashAlg = ParameterValidator.VALID_HASH_ALGS,
                IsSample = true,
                KeyBlockLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 512, 1024, 8))
            };

            return CreateRegistration(folderName, p);
        }
    }
}
