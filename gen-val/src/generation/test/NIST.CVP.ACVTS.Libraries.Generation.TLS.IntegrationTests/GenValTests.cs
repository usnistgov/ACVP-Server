using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.TLS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "tls";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_TLS_v1_0;

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
                TlsVersion = ParameterValidator.VALID_TLS_VERSIONS,
                HashAlg = new[] { "SHA2-256", "SHA2-512" },
                IsSample = true
                // uses default KeyBlockLength values of 832 and 1024
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
                TlsVersion = ParameterValidator.VALID_TLS_VERSIONS,
                HashAlg = ParameterValidator.VALID_HASH_ALGS,
                IsSample = true,
                KeyBlockLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 512, 1024, 8))
            };

            return CreateRegistration(folderName, p);
        }
    }
}
