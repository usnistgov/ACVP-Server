using NIST.CVP.Generation.Core.Tests;
using Autofac;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KDF_Components.v1_0.SRTP;

namespace NIST.CVP.Generation.SRTP.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "srtp";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_SRTP_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
		public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.srtpKe != null)
            {
                var bs = new BitString(testCase.srtpKe.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.srtpKe = bs.ToHex();
            }
        }
        
        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                AesKeyLength = new [] {128, 192},
                KdrExponent = new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12},
                SupportsZeroKdr = true,
                IsSample = false
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
                AesKeyLength = ParameterValidator.VALID_AES_KEY_LENGTHS,
                KdrExponent = ParameterValidator.VALID_KDR_EXPONENTS,
                SupportsZeroKdr = true,
                IsSample = false
            };

            return CreateRegistration(folderName, p);
        }
    }
}
