using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SSH.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "ssh";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_SSH_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();


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
                Cipher = new[] { "AES-128" },
                HashAlg = new[] { "SHA-1", "SHA2-256", "SHA2-512" },
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
