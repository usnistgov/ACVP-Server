using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.KAS_IFC.v1_0;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    public class GenValTestsKasIfc : GenValTestsSingleRunnerBase
    {
        public override AlgoMode AlgoMode => AlgoMode.KAS_IFC_v1_0;
        public override string Algorithm => "KAS-IFC";
        public override string Mode => string.Empty;
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
            var p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                IutId = new BitString("BEEFFACE"),
                Scheme = new Schemes()
                {
//                    Kas1_basic = new Kas1_basic()
//                    {
//                        L = 512,
//                        KasRole = new []
//                        {
//                            KeyAgreementRole.InitiatorPartyU, 
//                            KeyAgreementRole.ResponderPartyV
//                        },
//                        KdfMethods = new KdfMethods()
//                        {
//                            OneStepKdf = new OneStepKdf()
//                            {
//                                Encoding = new []{ FixedInfoEncoding.Concatenation },
//                                AuxFunctions = new []
//                                {
//                                    new AuxFunction()
//                                    {
//                                        SaltLen = 128,
//                                        AuxFunctionName = KasKdfOneStepAuxFunction.HMAC_SHA2_D224,
//                                        MacSaltMethods = new []{ MacSaltMethod.Default, MacSaltMethod.Random }
//                                    }
//                                },
//                                FixedInputPattern = "l|uPartyInfo|vPartyInfo" 
//                            }
//                        },
//                        KeyGenerationMethods = new KeyGenerationMethods()
//                        {
////                            RsaKpg1_basic = new RsaKpg1_basic()
////                            {
////                                Modulo = new[] { 2048 },
////                                FixedPublicExponent = new BigInteger(65537)
////                            },
//                            RsaKpg2_basic = new RsaKpg2_basic()
//                            {
//                                Modulo = new[] { 2048 },
//                            }
//                        }
//                    },
//                    Kas2_basic = new Kas2_basic()
//                    {
//                        L = 512,
//                        KasRole = new []
//                        {
//                            KeyAgreementRole.InitiatorPartyU, 
//                            KeyAgreementRole.ResponderPartyV
//                        },
//                        KdfMethods = new KdfMethods()
//                        {
//                            OneStepKdf = new OneStepKdf()
//                            {
//                                Encoding = new []{ FixedInfoEncoding.Concatenation },
//                                AuxFunctions = new []
//                                {
//                                    new AuxFunction()
//                                    {
//                                        SaltLen = 128,
//                                        AuxFunctionName = KasKdfOneStepAuxFunction.HMAC_SHA2_D224,
//                                        MacSaltMethods = new []{ MacSaltMethod.Default, MacSaltMethod.Random }
//                                    }
//                                },
//                                FixedInputPattern = "l|uPartyInfo|vPartyInfo" 
//                            }
//                        },
//                        KeyGenerationMethods = new KeyGenerationMethods()
//                        {
////                            RsaKpg1_basic = new RsaKpg1_basic()
////                            {
////                                Modulo = new[] { 2048 },
////                                FixedPublicExponent = new BigInteger(65537)
////                            },
//                            RsaKpg2_basic = new RsaKpg2_basic()
//                            {
//                                Modulo = new[] { 2048 },
//                            }
//                        }
//                    },  
                    Kas1_partyV_confirmation = new Kas1_partyV_confirmation()
                    {
                        L = 512,
                        KasRole = new []
                        {
                            KeyAgreementRole.InitiatorPartyU, 
                            KeyAgreementRole.ResponderPartyV
                        },
                        KdfMethods = new KdfMethods()
                        {
                            OneStepKdf = new OneStepKdf()
                            {
                                Encoding = new []{ FixedInfoEncoding.Concatenation },
                                AuxFunctions = new []
                                {
                                    new AuxFunction()
                                    {
                                        SaltLen = 128,
                                        AuxFunctionName = KasKdfOneStepAuxFunction.KMAC_128,
                                        MacSaltMethods = new []{ MacSaltMethod.Default, MacSaltMethod.Random }
                                    }
                                },
                                FixedInputPattern = "l|uPartyInfo|vPartyInfo" 
                            }
                        },
                        KeyGenerationMethods = new KeyGenerationMethods()
                        {
//                            RsaKpg1_basic = new RsaKpg1_basic()
//                            {
//                                Modulo = new[] { 2048 },
//                                FixedPublicExponent = new BigInteger(65537)
//                            },
                            RsaKpg2_basic = new RsaKpg2_basic()
                            {
                                Modulo = new[] { 2048 },
                            }
                        },
                        MacMethods = new MacMethods()
                        {
                            Kmac128 = new MacOptionKmac128()
                            {
                                KeyLen = 128,
                                MacLen = 224
                            },
                        }
                    },
//                    Kas2_partyV_confirmation = new Kas2_partyV_confirmation()
//                    {
//                        L = 512,
//                        KasRole = new []
//                        {
//                            KeyAgreementRole.InitiatorPartyU, 
//                            KeyAgreementRole.ResponderPartyV
//                        },
//                        KdfMethods = new KdfMethods()
//                        {
//                            OneStepKdf = new OneStepKdf()
//                            {
//                                Encoding = new []{ FixedInfoEncoding.Concatenation },
//                                AuxFunctions = new []
//                                {
//                                    new AuxFunction()
//                                    {
//                                        SaltLen = 128,
//                                        AuxFunctionName = KasKdfOneStepAuxFunction.HMAC_SHA2_D224,
//                                        MacSaltMethods = new []{ MacSaltMethod.Default, MacSaltMethod.Random }
//                                    }
//                                },
//                                FixedInputPattern = "l|uPartyInfo|vPartyInfo" 
//                            }
//                        },
//                        KeyGenerationMethods = new KeyGenerationMethods()
//                        {
////                            RsaKpg1_basic = new RsaKpg1_basic()
////                            {
////                                Modulo = new[] { 2048 },
////                                FixedPublicExponent = new BigInteger(65537)
////                            },
//                            RsaKpg2_basic = new RsaKpg2_basic()
//                            {
//                                Modulo = new[] { 2048 },
//                            }
//                        },
//                        MacMethods = new MacMethods()
//                        {
//                            HmacSha2_D224 = new MacOptionHmacSha2_d224()
//                            {
//                                KeyLen = 128,
//                                MacLen = 224
//                            }
//                        }
//                    },
//                    Kas2_partyU_confirmation = new Kas2_partyU_confirmation()
//                    {
//                        L = 512,
//                        KasRole = new []
//                        {
//                            KeyAgreementRole.InitiatorPartyU, 
//                            KeyAgreementRole.ResponderPartyV
//                        },
//                        KdfMethods = new KdfMethods()
//                        {
//                            OneStepKdf = new OneStepKdf()
//                            {
//                                Encoding = new []{ FixedInfoEncoding.Concatenation },
//                                AuxFunctions = new []
//                                {
//                                    new AuxFunction()
//                                    {
//                                        SaltLen = 128,
//                                        AuxFunctionName = KasKdfOneStepAuxFunction.HMAC_SHA2_D224,
//                                        MacSaltMethods = new []{ MacSaltMethod.Default, MacSaltMethod.Random }
//                                    }
//                                },
//                                FixedInputPattern = "l|uPartyInfo|vPartyInfo" 
//                            }
//                        },
//                        KeyGenerationMethods = new KeyGenerationMethods()
//                        {
////                            RsaKpg1_basic = new RsaKpg1_basic()
////                            {
////                                Modulo = new[] { 2048 },
////                                FixedPublicExponent = new BigInteger(65537)
////                            },
//                            RsaKpg2_basic = new RsaKpg2_basic()
//                            {
//                                Modulo = new[] { 2048 },
//                            }
//                        },
//                        MacMethods = new MacMethods()
//                        {
//                            HmacSha2_D224 = new MacOptionHmacSha2_d224()
//                            {
//                                KeyLen = 128,
//                                MacLen = 224
//                            }
//                        }
//                    },
//                    Kas2_bilateral_confirmation = new Kas2_bilateral_confirmation()
//                    {
//                        L = 512,
//                        KasRole = new []
//                        {
//                            KeyAgreementRole.InitiatorPartyU, 
//                            KeyAgreementRole.ResponderPartyV
//                        },
//                        KdfMethods = new KdfMethods()
//                        {
//                            OneStepKdf = new OneStepKdf()
//                            {
//                                Encoding = new []{ FixedInfoEncoding.Concatenation },
//                                AuxFunctions = new []
//                                {
//                                    new AuxFunction()
//                                    {
//                                        SaltLen = 128,
//                                        AuxFunctionName = KasKdfOneStepAuxFunction.HMAC_SHA2_D224,
//                                        MacSaltMethods = new []{ MacSaltMethod.Default, MacSaltMethod.Random }
//                                    }
//                                },
//                                FixedInputPattern = "l|uPartyInfo|vPartyInfo" 
//                            }
//                        },
//                        KeyGenerationMethods = new KeyGenerationMethods()
//                        {
////                            RsaKpg1_basic = new RsaKpg1_basic()
////                            {
////                                Modulo = new[] { 2048 },
////                                FixedPublicExponent = new BigInteger(65537)
////                            },
//                            RsaKpg2_basic = new RsaKpg2_basic()
//                            {
//                                Modulo = new[] { 2048 },
//                            }
//                        },
//                        MacMethods = new MacMethods()
//                        {
//                            HmacSha2_D224 = new MacOptionHmacSha2_d224()
//                            {
//                                KeyLen = 128,
//                                MacLen = 224
//                            }
//                        }
//                    },
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