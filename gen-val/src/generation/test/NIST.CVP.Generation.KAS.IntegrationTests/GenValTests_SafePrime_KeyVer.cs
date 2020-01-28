using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.SafePrimeGroups.v1_0.KeyVer;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    public class GenValTest_SafePrime_KeyVer : GenValTestsSingleRunnerBase
    {
        public override AlgoMode AlgoMode => AlgoMode.SafePrimes_keyGen_v1_0;
        public override string Algorithm => "SafePrimes";
        public override string Mode => "KeyVer";
        public override string Revision => "1.0";
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            if (testCase.testPassed != null)
            {
                if (testCase.testPassed == true)
                {
                    testCase.testPassed = false;
                }
                else
                {
                    testCase.testPassed = true;
                }
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                SafePrimeGroups = new []{ SafePrime.Ffdhe2048, SafePrime.Modp2048 }
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            return GetTestFileFewTestCases(folderName);
        }
    }
}