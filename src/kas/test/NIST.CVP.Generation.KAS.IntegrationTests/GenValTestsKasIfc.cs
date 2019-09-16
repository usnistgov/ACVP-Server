using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
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
        public override string Algorithm => "Kas-Ifc";
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
                IutId = new BitString("BEEFFACE"),
                PublicKeys = new []
                {
                    // 2048
                    new PublicKey()
                    {
                        E = new BitString("3BCF32AB").ToPositiveBigInteger(),
                        N = new BitString("CAAAB260AC87B780BD80F76CAB0B54F9B938803C1036A5F9D66F6AEF1869B43945E991155EC7178AA54A0BB1331FDAFD3B6A0984780842C96A88500B19D054111B53EAB5977D4B7F15230C610EE67177C361CB3A108581B47D5362EEC7C0AB7F3CEA25D6688EE69539682C9FFB1BE7F507EA875E3E234D0AA253467109ED9A5D9C1510AFA9A1DFCA0C827C96032AE5699E6D6C4271327DCA55093DE0C44025090014E4AD694D1C04582435FB5CCEDC9D28CD10608D44A6118AA60E91823330A0FD5E68C78028DAB1318642B7151FBED2937D46AB3C38F27E72D008C71DDD75071B4559F3B74990EA313B3542C402803BE4CCF499251B0C3349C92B394AD0EAF7").ToPositiveBigInteger()
                    }
                },
                Scheme = new Schemes()
                {
                    Kas1_basic = new Kas1_basic()
                    {
                        L = 512,
                        KasRole = new []{ KeyAgreementRole.InitiatorPartyU, KeyAgreementRole.ResponderPartyV },
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
                                        AuxFunctionName = KasKdfOneStepAuxFunction.HMAC_SHA2_D224,
                                        MacSaltMethods = new []{ MacSaltMethod.Default, MacSaltMethod.Random }
                                    }
                                },
                                FixedInputPattern = "l|uPartyInfo|vPartyInfo" 
                            }
                        },
                        KeyGenerationMethods = new KeyGenerationMethods()
                        {
                            RsaKpg1_crt = new RsaKpg1_crt()
                            {
                                Modulo = new[] { 2048 },
                                FixedPublicExponent = new BitString("3BCF32AB").ToPositiveBigInteger()
                            }
                        }
                    },
//                    Kas1_partyV_confirmation = new Kas1_partyV_confirmation()
//                    {
//                        L = 512,
//                        KasRole = new []{ KeyAgreementRole.InitiatorPartyU, KeyAgreementRole.ResponderPartyV },
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
//                        MacMethods = new MacMethods()
//                        {
//                            HmacSha2_D224 = new MacOptionHmacSha2_d224()
//                            {
//                                KeyLen = 224,
//                                MacLen = 224
//                            }
//                        },
//                        KeyGenerationMethods = new KeyGenerationMethods()
//                        {
//                            RsaKpg1_crt = new RsaKpg1_crt()
//                            {
//                                Modulo = new[] { 2048 },
//                                FixedPublicExponent = new BitString("3BCF32AB").ToPositiveBigInteger()
//                            }
//                        }
//                    }
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