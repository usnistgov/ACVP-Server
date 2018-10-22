using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.TPMv1._2;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TPM.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "tpm";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_TPM;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
        public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.sKey != null)
            {
                var bs = new BitString(testCase.sKey.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.sKey = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
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
                IsSample = false
            };

            return CreateRegistration(folderName, p);
        }
    }
}
