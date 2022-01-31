using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsKtsIfc : GenValTestsWithNoSample
    {
        public override AlgoMode AlgoMode => AlgoMode.KTS_IFC_Sp800_56Br2;
        public override string Algorithm => "KTS-IFC";
        public override string Mode => null;
        public override string Revision => "Sp800-56Br2";
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
        public override IJsonConverterProvider JsonConverterProvider => new KasJsonConverterProvider();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has dkm, change it
            if (testCase.dkm != null)
            {
                BitString bs = new BitString(testCase.dkm.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.dkm = bs.ToHex();
            }
            // If TC has a tag, change it
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
                IutId = new BitString("123456ABCD"),
                KeyGenerationMethods = new[]
                {
                    IfcKeyGenerationMethod.RsaKpg2_basic,
                },
                Modulo = new[] { 2048 },
                Scheme = new Schemes
                {
                    Kts_oaep_partyV_confirmation = new Kts_oaep_partyV_confirmation
                    {
                        L = 1024,
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                        KtsMethod = new KtsMethod
                        {
                            Encoding = new[] { FixedInfoEncoding.Concatenation },
                            HashAlgs = new[] { KasHashAlg.SHA1, KasHashAlg.SHA2_D512 },
                            AssociatedDataPattern = "l||uPartyInfo||vPartyInfo||label",
                            SupportsNullAssociatedData = true
                        },
                        MacMethods = new MacMethods
                        {
                            HmacSha1 = new MacOptionHmacSha1
                            {
                                KeyLen = 160,
                                MacLen = 160
                            },
                            Kmac128 = new MacOptionKmac128
                            {
                                KeyLen = 128,
                                MacLen = 224
                            }
                        }
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
                IutId = new BitString("123456ABCD"),
                PublicExponent = new BigInteger(65537),
                KeyGenerationMethods = new[]
                {
                    IfcKeyGenerationMethod.RsaKpg1_basic
                },
                Modulo = new[] { 2048 },
                Scheme = new Schemes
                {
                    Kts_oaep_basic = new Kts_oaep_basic
                    {
                        L = 512,
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                        KtsMethod = new KtsMethod
                        {
                            Encoding = new[] { FixedInfoEncoding.Concatenation },
                            HashAlgs = new[] { KasHashAlg.SHA2_D224 },
                            AssociatedDataPattern = "l||uPartyInfo||vPartyInfo",
                            SupportsNullAssociatedData = true
                        }
                    },
                    Kts_oaep_partyV_confirmation = new Kts_oaep_partyV_confirmation
                    {
                        L = 512,
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                        KtsMethod = new KtsMethod
                        {
                            Encoding = new[] { FixedInfoEncoding.Concatenation },
                            HashAlgs = new[] { KasHashAlg.SHA2_D224 },
                            AssociatedDataPattern = "l||uPartyInfo||vPartyInfo",
                            SupportsNullAssociatedData = true
                        },
                        MacMethods = new MacMethods
                        {
                            Kmac128 = new MacOptionKmac128
                            {
                                KeyLen = 128,
                                MacLen = 224
                            }
                        }
                    },
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
