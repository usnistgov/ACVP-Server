using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.KAS_IFC.v1_0;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using Parameters = NIST.CVP.Generation.KAS_IFC.v1_0.Parameters;
using RegisterInjections = NIST.CVP.Generation.KAS_IFC.v1_0.RegisterInjections;

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
                //                PublicKeys = new []
                //                {
                //                    new PublicKey()
                //                    {
                //                        N = new BitString("CAAAB260AC87B780BD80F76CAB0B54F9B938803C1036A5F9D66F6AEF1869B43945E991155EC7178AA54A0BB1331FDAFD3B6A0984780842C96A88500B19D054111B53EAB5977D4B7F15230C610EE67177C361CB3A108581B47D5362EEC7C0AB7F3CEA25D6688EE69539682C9FFB1BE7F507EA875E3E234D0AA253467109ED9A5D9C1510AFA9A1DFCA0C827C96032AE5699E6D6C4271327DCA55093DE0C44025090014E4AD694D1C04582435FB5CCEDC9D28CD10608D44A6118AA60E91823330A0FD5E68C78028DAB1318642B7151FBED2937D46AB3C38F27E72D008C71DDD75071B4559F3B74990EA313B3542C402803BE4CCF499251B0C3349C92B394AD0EAF7").ToPositiveBigInteger(),
                //                        E = new BitString("3BCF32AB").ToPositiveBigInteger()
                //                    }, 
                //                },
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
                    //                                        AuxFunctionName = KasKdfOneStepAuxFunction.KMAC_128,
                    //                                        MacSaltMethods = new []{ MacSaltMethod.Default, MacSaltMethod.Random }
                    //                                    }
                    //                                },
                    //                                FixedInputPattern = "l||uPartyInfo||vPartyInfo" 
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
                    //                                        AuxFunctionName = KasKdfOneStepAuxFunction.KMAC_128,
                    //                                        MacSaltMethods = new []{ MacSaltMethod.Default, MacSaltMethod.Random }
                    //                                    }
                    //                                },
                    //                                FixedInputPattern = "l||uPartyInfo||vPartyInfo" 
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
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                        KdfMethods = new KdfMethods()
                        {
                            OneStepKdf = new OneStepKdf()
                            {
                                Encoding = new[] { FixedInfoEncoding.Concatenation },
                                AuxFunctions = new[]
                                {
                                    new AuxFunction()
                                    {
                                        SaltLen = 128,
                                        AuxFunctionName = KasKdfOneStepAuxFunction.KMAC_128,
                                        MacSaltMethods = new []{ MacSaltMethod.Default, MacSaltMethod.Random }
                                    }
                                },
                                FixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo"
                            },
                            TwoStepKdf = new TwoStepKdf()
                            {
                                Capabilities = new[]
                                {
                                    new TwoStepCapabilities()
                                    {
                                        Encoding = new[] { FixedInfoEncoding.Concatenation },
                                        FixedInfoPattern = "l||label||uPartyInfo||vPartyInfo||context",
                                        MacSaltMethods = new[] { MacSaltMethod.Default, MacSaltMethod.Random },
                                        CounterLength = new [] { 32 },
                                        SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                                        MacMode = new [] { MacModes.CMAC_AES256, MacModes.HMAC_SHA3_224 },
                                        KdfMode = KdfModes.Feedback,
                                        FixedDataOrder = new []{ CounterLocations.AfterFixedData },
                                        SupportsEmptyIv = false
                                    },
                                    new TwoStepCapabilities()
                                    {
                                        Encoding = new[] { FixedInfoEncoding.Concatenation },
                                        FixedInfoPattern = "l||label||uPartyInfo||vPartyInfo||context",
                                        MacSaltMethods = new[] { MacSaltMethod.Default, MacSaltMethod.Random },
                                        CounterLength = new [] { 32 },
                                        SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                                        MacMode = new [] { MacModes.CMAC_AES256, MacModes.HMAC_SHA3_224 },
                                        KdfMode = KdfModes.Counter,
                                        FixedDataOrder = new []{ CounterLocations.AfterFixedData },
                                        SupportsEmptyIv = false
                                    },
                                },
                            },
                            IkeV1Kdf = new IkeV1Kdf()
                            {
                                HashFunctions = new[] { HashFunctions.Sha3_d512 }
                            },
                            //IkeV2Kdf = new IkeV2Kdf()
                            //{
                            //    HashFunctions = new[] { HashFunctions.Sha3_d512 }
                            //},
                            //TlsV10_11Kdf = new TlsV10_11Kdf(),
                            //TlsV12Kdf = new TlsV12Kdf()
                            //{
                            //    HashFunctions = new[] { HashFunctions.Sha2_d256 }
                            //}
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
                            },
                            RsaKpg2_primeFactor = new RsaKpg2_primeFactor()
                            {
                                Modulo = new[] { 2048 },
                            },
                            RsaKpg2_crt = new RsaKpg2_crt()
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
                    //                                FixedInputPattern = "l||uPartyInfo||vPartyInfo" 
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
                    //                                FixedInputPattern = "l||uPartyInfo||vPartyInfo" 
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
                    //                                FixedInputPattern = "l||uPartyInfo||vPartyInfo" 
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