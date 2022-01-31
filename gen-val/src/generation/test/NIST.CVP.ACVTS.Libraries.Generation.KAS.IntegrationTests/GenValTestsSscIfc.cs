using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Br2.Ifc;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSscIfc : GenValTestsWithNoSample
    {
        public override AlgoMode AlgoMode => AlgoMode.KAS_IFC_SSC_Sp800_56Br2;
        public override string Algorithm => "KAS-IFC-SSC";
        public override string Mode => null;
        public override string Revision => "Sp800-56Br2";
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has dkm, change it
            if (testCase.z != null)
            {
                BitString bs = new BitString(testCase.z.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.z = bs.ToHex();
            }

            // If TC has a result, change it
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
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Modulo = new[] { 2048 },
                KeyGenerationMethods = new[] { IfcKeyGenerationMethod.RsaKpg2_basic },
                Scheme = new Schemes
                {
                    Kas1 = new Kas1
                    {
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                    },
                }
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileFewTestCasesNotSample(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = false,
                Modulo = new[] { 2048 },
                KeyGenerationMethods = new[] { IfcKeyGenerationMethod.RsaKpg2_basic, IfcKeyGenerationMethod.RsaKpg2_crt },
                Scheme = new Schemes
                {
                    Kas1 = new Kas1
                    {
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                    },
                    Kas2 = new Kas2
                    {
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                    },
                }
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
                IsSample = true,
                Modulo = new[] { 2048, 3072 },
                KeyGenerationMethods = new[] { IfcKeyGenerationMethod.RsaKpg2_basic, IfcKeyGenerationMethod.RsaKpg2_crt, IfcKeyGenerationMethod.RsaKpg1_crt },
                FixedPublicExponent = new BigInteger(65539),
                Scheme = new Schemes
                {
                    Kas1 = new Kas1
                    {
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                    },
                    Kas2 = new Kas2
                    {
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                    },
                }
            };

            return CreateRegistration(folderName, p);
        }
    }
}
