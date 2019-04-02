using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NUnit.Framework;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using Autofac;
using NIST.CVP.Math;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SSH.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "ssh";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_SSH_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
		public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.initialIvClient != null)
            {
                var bs = new BitString(testCase.initialIvClient.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.initialIvClient = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Cipher = new[] {"aes-128"},
                HashAlg = new[] {"sha-1", "sha2-256", "sha2-512"},
                IsSample = true
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
                Cipher = ParameterValidator.VALID_CIPHERS,
                HashAlg = ParameterValidator.VALID_HASH_ALGS,
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }
    }
}
