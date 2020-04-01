using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.SafePrimeGroups.v1_0.KeyGen;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTest_SafePrime_KeyGen : GenValTestsWithNoSample
    {
        public override AlgoMode AlgoMode => AlgoMode.SafePrimes_keyGen_v1_0;
        public override string Algorithm => "SafePrimes";
        public override string Mode => "KeyGen";
        public override string Revision => "1.0";
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a x, change it
            if (testCase.x != null)
            {
                BitString bs = new BitString(testCase.x.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.x = bs.ToHex();
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

        protected override string GetTestFileFewTestCasesNotSample(string folderName)
        {
            var p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = false,
                SafePrimeGroups = new []{ SafePrime.Ffdhe2048, SafePrime.Modp2048 }
            };

            return CreateRegistration(folderName, p);
        }
    }
}