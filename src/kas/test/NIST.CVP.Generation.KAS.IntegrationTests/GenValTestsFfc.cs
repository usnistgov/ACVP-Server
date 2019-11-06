using Autofac;
using NIST.CVP.Common;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.v1_0;
using NIST.CVP.Generation.KAS.v1_0.FFC;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsFfc : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "KAS-FFC";
        public override string Mode => string.Empty;
        
        public override AlgoMode AlgoMode => AlgoMode.KAS_FFC_v1_0;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a hashZIut, change it
            if (testCase.hashZIut != null)
            {
                BitString bs = new BitString(testCase.hashZIut.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.hashZIut = bs.ToHex();
            }
            // If TC has a tagIut, change it
            if (testCase.tagIut != null)
            {
                BitString bs = new BitString(testCase.tagIut.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.tagIut = bs.ToHex();
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
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Function = new string[] { "dpGen" },
                Scheme = new Schemes()
                {
                    FfcDhHybrid1 = new FfcDhHybrid1()
                    {
                        KasRole = new string[] { "initiator" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] { "SHA2-224" }
                                }
                            }
                        }
                    },
                    //FfcMqv2 = new FfcMqv2()
                    //{
                    //    KasRole = new string[] { "initiator" },
                    //    NoKdfNoKc = new NoKdfNoKc()
                    //    {
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" }
                    //            }
                    //        }
                    //    }
                    //},
                    //FfcDhEphem = new FfcDhEphem()
                    //{
                    //    KasRole = new string[] { "initiator" },
                    //    NoKdfNoKc = new NoKdfNoKc()
                    //    {
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" }
                    //            }
                    //        }
                    //    }
                    //},
                    //FfcDhHybridOneFlow = new FfcDhHybridOneFlow()
                    //{
                    //    KasRole = new string[] { "initiator" },
                    //    NoKdfNoKc = new NoKdfNoKc()
                    //    {
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" }
                    //            }
                    //        }
                    //    }
                    //},
                    //FfcMqv1 = new FfcMqv1()
                    //{
                    //    KasRole = new string[] { "initiator" },
                    //    NoKdfNoKc = new NoKdfNoKc()
                    //    {
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" }
                    //            }
                    //        }
                    //    }
                    //},
                    //FfcDhOneFlow = new FfcDhOneFlow()
                    //{
                    //    KasRole = new string[] { "initiator" },
                    //    NoKdfNoKc = new NoKdfNoKc()
                    //    {
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" }
                    //            }
                    //        }
                    //    }
                    //},
                    //FfcDhStatic = new FfcDhStatic()
                    //{
                    //    KasRole = new string[] { "initiator" },
                    //    NoKdfNoKc = new NoKdfNoKc()
                    //    {
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" }
                    //            }
                    //        }
                    //    }
                    //},
                },
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Function = new string[] { "dpGen", "dpVal", "keyPairGen", "fullVal", "keyRegen" },
                Scheme = new Schemes()
                {
                    FfcDhHybrid1 = new FfcDhHybrid1()
                    {
                        KasRole = new string[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] { "SHA2-224" }
                                }
                            }
                        },
                        KdfNoKc = new KdfNoKc()
                        {
                            KdfOption = new KdfOptions()
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets()
                            {
                                Fb = new Fb()
                                {
                                    HashAlg = new string[] { "SHA2-224" },
                                    MacOption = new MacOptions()
                                    {
                                        AesCcm = new MacOptionAesCcm()
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac()
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D512_T224 = new MacOptionHmacSha2_d512_t224()
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha3_D224 = new MacOptionHmacSha3_d224()
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    }
                                }
                            }
                        }
                    },
                    //FfcMqv2 = new FfcMqv2()
                    //{
                    //    KasRole = new string[] { "initiator", "responder" },
                    //    NoKdfNoKc = new NoKdfNoKc()
                    //    {
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" }
                    //            }
                    //        }
                    //    },
                    //    KdfNoKc = new KdfNoKc()
                    //    {
                    //        KdfOption = new KdfOptions()
                    //        {
                    //            Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                    //        },
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" },
                    //                MacOption = new MacOptions()
                    //                {
                    //                    AesCcm = new MacOptionAesCcm()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128,
                    //                        NonceLen = 64
                    //                    },
                    //                    Cmac = new MacOptionCmac()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128
                    //                    },
                    //                    HmacSha2_D224 = new MacOptionHmacSha2_d224()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    },
                    //    KdfKc = new KdfKc()
                    //    {
                    //        KcOption = new KcOptions()
                    //        {
                    //            KcRole = new string[] { "provider", "recipient" },
                    //            KcType = new string[] { "unilateral", "bilateral" },
                    //            NonceType = new string[] { "randomNonce" }
                    //        },
                    //        KdfOption = new KdfOptions()
                    //        {
                    //            Asn1 = "uPartyInfo||vPartyInfo||literal[cafe1234]"
                    //        },
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-512" },
                    //                MacOption = new MacOptions()
                    //                {
                    //                    AesCcm = new MacOptionAesCcm()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128,
                    //                        NonceLen = 64
                    //                    },
                    //                    Cmac = new MacOptionCmac()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128
                    //                    },
                    //                    HmacSha2_D384 = new MacOptionHmacSha2_d384()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //},
                    //FfcDhEphem = new FfcDhEphem()
                    //{
                    //    KasRole = new string[] { "initiator", "responder" },
                    //    NoKdfNoKc = new NoKdfNoKc()
                    //    {
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" }
                    //            }
                    //        }
                    //    },
                    //    KdfNoKc = new KdfNoKc()
                    //    {
                    //        KdfOption = new KdfOptions()
                    //        {
                    //            Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                    //        },
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" },
                    //                MacOption = new MacOptions()
                    //                {
                    //                    AesCcm = new MacOptionAesCcm()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128,
                    //                        NonceLen = 64
                    //                    },
                    //                    Cmac = new MacOptionCmac()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128
                    //                    },
                    //                    HmacSha2_D224 = new MacOptionHmacSha2_d224()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //},
                    //FfcDhHybridOneFlow = new FfcDhHybridOneFlow()
                    //{
                    //    KasRole = new string[] { "initiator", "responder" },
                    //    NoKdfNoKc = new NoKdfNoKc()
                    //    {
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" }
                    //            }
                    //        }
                    //    },
                    //    KdfNoKc = new KdfNoKc()
                    //    {
                    //        KdfOption = new KdfOptions()
                    //        {
                    //            Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                    //        },
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" },
                    //                MacOption = new MacOptions()
                    //                {
                    //                    AesCcm = new MacOptionAesCcm()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128,
                    //                        NonceLen = 64
                    //                    },
                    //                    Cmac = new MacOptionCmac()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128
                    //                    },
                    //                    HmacSha2_D224 = new MacOptionHmacSha2_d224()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    },
                    //    KdfKc = new KdfKc()
                    //    {
                    //        KcOption = new KcOptions()
                    //        {
                    //            KcRole = new string[] { "provider", "recipient" },
                    //            KcType = new string[] { "unilateral", "bilateral" },
                    //            NonceType = new string[] { "randomNonce" }
                    //        },
                    //        KdfOption = new KdfOptions()
                    //        {
                    //            Asn1 = "uPartyInfo||vPartyInfo||literal[cafe1234]"
                    //        },
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-512" },
                    //                MacOption = new MacOptions()
                    //                {
                    //                    AesCcm = new MacOptionAesCcm()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128,
                    //                        NonceLen = 64
                    //                    },
                    //                    Cmac = new MacOptionCmac()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128
                    //                    },
                    //                    HmacSha2_D384 = new MacOptionHmacSha2_d384()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //},
                    //FfcMqv1 = new FfcMqv1()
                    //{
                    //    KasRole = new string[] { "initiator", "responder" },
                    //    NoKdfNoKc = new NoKdfNoKc()
                    //    {
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" }
                    //            }
                    //        }
                    //    },
                    //    KdfNoKc = new KdfNoKc()
                    //    {
                    //        KdfOption = new KdfOptions()
                    //        {
                    //            Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                    //        },
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" },
                    //                MacOption = new MacOptions()
                    //                {
                    //                    AesCcm = new MacOptionAesCcm()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128,
                    //                        NonceLen = 64
                    //                    },
                    //                    Cmac = new MacOptionCmac()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128
                    //                    },
                    //                    HmacSha2_D224 = new MacOptionHmacSha2_d224()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    },
                    //    KdfKc = new KdfKc()
                    //    {
                    //        KcOption = new KcOptions()
                    //        {
                    //            KcRole = new string[] { "provider", "recipient" },
                    //            KcType = new string[] { "unilateral", "bilateral" },
                    //            NonceType = new string[] { "randomNonce" }
                    //        },
                    //        KdfOption = new KdfOptions()
                    //        {
                    //            Asn1 = "uPartyInfo||vPartyInfo||literal[cafe1234]"
                    //        },
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-512" },
                    //                MacOption = new MacOptions()
                    //                {
                    //                    AesCcm = new MacOptionAesCcm()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128,
                    //                        NonceLen = 64
                    //                    },
                    //                    Cmac = new MacOptionCmac()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128
                    //                    },
                    //                    HmacSha2_D384 = new MacOptionHmacSha2_d384()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //},
                    //FfcDhOneFlow = new FfcDhOneFlow()
                    //{
                    //    KasRole = new string[] { "initiator", "responder" },
                    //    NoKdfNoKc = new NoKdfNoKc()
                    //    {
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" }
                    //            }
                    //        }
                    //    },
                    //    KdfNoKc = new KdfNoKc()
                    //    {
                    //        KdfOption = new KdfOptions()
                    //        {
                    //            Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                    //        },
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" },
                    //                MacOption = new MacOptions()
                    //                {
                    //                    AesCcm = new MacOptionAesCcm()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128,
                    //                        NonceLen = 64
                    //                    },
                    //                    Cmac = new MacOptionCmac()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128
                    //                    },
                    //                    HmacSha2_D224 = new MacOptionHmacSha2_d224()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    },
                    //    KdfKc = new KdfKc()
                    //    {
                    //        KcOption = new KcOptions()
                    //        {
                    //            KcRole = new string[] { "provider", "recipient" },
                    //            KcType = new string[] { "unilateral" },
                    //            NonceType = new string[] { "randomNonce" }
                    //        },
                    //        KdfOption = new KdfOptions()
                    //        {
                    //            Asn1 = "uPartyInfo||vPartyInfo||literal[cafe1234]"
                    //        },
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-512" },
                    //                MacOption = new MacOptions()
                    //                {
                    //                    AesCcm = new MacOptionAesCcm()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128,
                    //                        NonceLen = 64
                    //                    },
                    //                    Cmac = new MacOptionCmac()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128
                    //                    },
                    //                    HmacSha2_D384 = new MacOptionHmacSha2_d384()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //},
                    //FfcDhStatic = new FfcDhStatic()
                    //{
                    //    KasRole = new string[] { "initiator", "responder" },
                    //    NoKdfNoKc = new NoKdfNoKc()
                    //    {
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" }
                    //            }
                    //        }
                    //    },
                    //    KdfNoKc = new KdfNoKc()
                    //    {
                    //        DkmNonceTypes = new string[] { "randomNonce" },
                    //        KdfOption = new KdfOptions()
                    //        {
                    //            Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                    //        },
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-224" },
                    //                MacOption = new MacOptions()
                    //                {
                    //                    AesCcm = new MacOptionAesCcm()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128,
                    //                        NonceLen = 64
                    //                    },
                    //                    Cmac = new MacOptionCmac()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128
                    //                    },
                    //                    HmacSha2_D224 = new MacOptionHmacSha2_d224()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    },
                    //    KdfKc = new KdfKc()
                    //    {
                    //        DkmNonceTypes = new string[] { "randomNonce" },
                    //        KcOption = new KcOptions()
                    //        {
                    //            KcRole = new string[] { "provider", "recipient" },
                    //            KcType = new string[] { "unilateral" },
                    //            NonceType = new string[] { "randomNonce" }
                    //        },
                    //        KdfOption = new KdfOptions()
                    //        {
                    //            Asn1 = "uPartyInfo||vPartyInfo||literal[cafe1234]"
                    //        },
                    //        ParameterSet = new ParameterSets()
                    //        {
                    //            Fb = new Fb()
                    //            {
                    //                HashAlg = new string[] { "SHA2-512" },
                    //                MacOption = new MacOptions()
                    //                {
                    //                    AesCcm = new MacOptionAesCcm()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128,
                    //                        NonceLen = 64
                    //                    },
                    //                    Cmac = new MacOptionCmac()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    //                        MacLen = 128
                    //                    },
                    //                    HmacSha2_D384 = new MacOptionHmacSha2_d384()
                    //                    {
                    //                        KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    //                        MacLen = 128
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //},
                },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}