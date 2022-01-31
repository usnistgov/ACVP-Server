using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsEcc : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "KAS-ECC";
        public override string Mode => null;

        public override AlgoMode AlgoMode => AlgoMode.KAS_ECC_v1_0;


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
            Parameters p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Function = new[] { "dpGen", "partialVal", "fullVal" },
                Scheme = new Schemes
                {
                    EccEphemeralUnified = new EccEphemeralUnified
                    {
                        KasRole = new[] { "initiator" },
                        NoKdfNoKc = new NoKdfNoKc
                        {
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA-1", "SHA2-224" },
                                    Curve = "P-224"
                                }
                            }
                        }
                    },
                    EccOnePassMqv = new EccOnePassMqv
                    {
                        KasRole = new[] { "initiator" },
                        NoKdfNoKc = new NoKdfNoKc
                        {
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA-1", "SHA2-224" },
                                    Curve = "P-224"
                                }
                            }
                        }
                    },
                    EccStaticUnified = new EccStaticUnified
                    {
                        KasRole = new[] { "initiator", "responder" },
                        KdfKc = new KdfKc
                        {
                            DkmNonceTypes = new[] { "randomNonce" },
                            KcOption = new KcOptions
                            {
                                KcRole = new[] { "provider", "recipient" },
                                KcType = new[] { "unilateral" },
                                NonceType = new[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Ee = new Ee
                                {
                                    HashAlg = new[] { "SHA2-512" },
                                    MacOption = new MacOptions
                                    {
                                        HmacSha1 = new MacOptionHmacSha1
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(160)),
                                            MacLen = 160
                                        },
                                        HmacSha2_D256 = new MacOptionHmacSha2_d256
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-521"
                                }
                            }
                        },
                    },
                },
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Function = new[] { "dpGen", "dpVal", "keyPairGen", "partialVal", "keyRegen" },
                Scheme = new Schemes
                {
                    EccFullUnified = new EccFullUnified
                    {
                        KasRole = new[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc
                        {
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfNoKc = new KdfNoKc
                        {
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 104
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfKc = new KdfKc
                        {
                            KcOption = new KcOptions
                            {
                                KcRole = new[] { "provider", "recipient" },
                                KcType = new[] { "unilateral", "bilateral" },
                                NonceType = new[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D512_T224 = new MacOptionHmacSha2_d512_t224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha3_D224 = new MacOptionHmacSha3_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                    },
                    EccFullMqv = new EccFullMqv
                    {
                        KasRole = new[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc
                        {
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfNoKc = new KdfNoKc
                        {
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfKc = new KdfKc
                        {
                            KcOption = new KcOptions
                            {
                                KcRole = new[] { "provider", "recipient" },
                                KcType = new[] { "unilateral", "bilateral" },
                                NonceType = new[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                    },
                    EccEphemeralUnified = new EccEphemeralUnified
                    {
                        KasRole = new[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc
                        {
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfNoKc = new KdfNoKc
                        {
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        }
                    },
                    EccOnePassUnified = new EccOnePassUnified
                    {
                        KasRole = new[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc
                        {
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfNoKc = new KdfNoKc
                        {
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfKc = new KdfKc
                        {
                            KcOption = new KcOptions
                            {
                                KcRole = new[] { "provider", "recipient" },
                                KcType = new[] { "unilateral", "bilateral" },
                                NonceType = new[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                    },
                    EccOnePassMqv = new EccOnePassMqv
                    {
                        KasRole = new[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc
                        {
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfNoKc = new KdfNoKc
                        {
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfKc = new KdfKc
                        {
                            KcOption = new KcOptions
                            {
                                KcRole = new[] { "provider", "recipient" },
                                KcType = new[] { "unilateral", "bilateral" },
                                NonceType = new[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                    },
                    EccOnePassDh = new EccOnePassDh
                    {
                        KasRole = new[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc
                        {
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfNoKc = new KdfNoKc
                        {
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfKc = new KdfKc
                        {
                            KcOption = new KcOptions
                            {
                                KcRole = new[] { "provider", "recipient" },
                                KcType = new[] { "unilateral", "bilateral" },
                                NonceType = new[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                    },
                    EccStaticUnified = new EccStaticUnified
                    {
                        KasRole = new[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc
                        {
                            ParameterSet = new ParameterSets
                            {
                                Ee = new Ee
                                {
                                    HashAlg = new[] { "SHA2-512" },
                                    Curve = "P-521"
                                }
                            }
                        },
                        KdfNoKc = new KdfNoKc
                        {
                            DkmNonceTypes = new[] { "randomNonce" },
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfKc = new KdfKc
                        {
                            DkmNonceTypes = new[] { "randomNonce" },
                            KcOption = new KcOptions
                            {
                                KcRole = new[] { "provider", "recipient" },
                                KcType = new[] { "unilateral", "bilateral" },
                                NonceType = new[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets
                            {
                                Eb = new Eb
                                {
                                    HashAlg = new[] { "SHA2-224" },
                                    MacOption = new MacOptions
                                    {
                                        AesCcm = new MacOptionAesCcm
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128,
                                            NonceLen = 64
                                        },
                                        Cmac = new MacOptionCmac
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        },
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                    },
                },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
