using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KAS_KC;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsKasKc : GenValTestsSingleRunnerBase
    {
        public override AlgoMode AlgoMode => AlgoMode.KAS_KC_Sp800_56;
        public override string Algorithm => "KAS-KC";
        public override string Mode => null;
        public override string Revision => "Sp800-56";
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a dkm, change it
            if (testCase.tag != null)
            {
                BitString bs = new BitString(testCase.tag.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.tag = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                KasRole = new[] { KeyAgreementRole.InitiatorPartyU, KeyAgreementRole.ResponderPartyV },
                KeyConfirmationMethod = new KeyConfirmationMethod
                {
                    KeyConfirmationDirections = new[] { KeyConfirmationDirection.Bilateral, KeyConfirmationDirection.Unilateral },
                    KeyConfirmationRoles = new[] { KeyConfirmationRole.Provider, KeyConfirmationRole.Recipient },
                    MacMethods = new MacMethods
                    {
                        Cmac = new MacOptionCmac
                        {
                            KeyLen = 256,
                            MacLen = 64
                        },
                        Kmac256 = new MacOptionKmac256
                        {
                            KeyLen = 256,
                            MacLen = 256
                        },
                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                        {
                            KeyLen = 256,
                            MacLen = 128
                        }
                    }
                }
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            return GetTestFileFewTestCases(folderName);
        }
    }
}
