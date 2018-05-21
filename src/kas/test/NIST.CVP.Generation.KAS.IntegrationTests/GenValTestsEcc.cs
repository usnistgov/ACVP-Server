using Autofac;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsEcc : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "KAS-ECC";
        public override string Mode => string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.KAS_ECC;

        public override IRegisterInjections RegistrationsGenVal => new ECC.RegisterInjections();

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
            if (testCase.result != null)
            {
                testCase.result = testCase.result.ToString().Equals("pass") ? "fail" : "pass";
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Function = new string[] { "dpGen" },
                Scheme = new Schemes()
                {
                    EccEphemeralUnified = new EccEphemeralUnified()
                    {
                        Role = new string[] { "initiator" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
                                {
                                    HashAlg = new string[] { "SHA2-224" },
                                    Curve = "P-224"
                                }
                            }
                        }
                    },
                    EccOnePassMqv = new EccOnePassMqv()
                    {
                        Role = new string[] { "initiator" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
                                {
                                    HashAlg = new string[] { "SHA2-224" },
                                    Curve = "P-224"
                                }
                            }
                        }
                    },
                    EccStaticUnified = new EccStaticUnified()
                    {
                        Role = new string[] { "initiator" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
                                {
                                    HashAlg = new string[] { "SHA2-224" },
                                    Curve = "P-224"
                                }
                            }
                        }
                    }
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
                Function = new string[] { "dpGen", "dpVal", "keyPairGen", "partialVal", "keyRegen" },
                Scheme = new Schemes()
                {
                    EccFullUnified = new EccFullUnified()
                    {
                        Role = new string[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
                                {
                                    HashAlg = new string[] { "SHA2-224" },
                                    Curve = "P-224"
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
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfKc = new KdfKc()
                        {
                            KcOption = new KcOptions()
                            {
                                KcRole = new string[] { "provider", "recipient" },
                                KcType = new string[] { "unilateral", "bilateral" },
                                NonceType = new string[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions()
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
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
                    EccFullMqv = new EccFullMqv()
                    {
                        Role = new string[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
                                {
                                    HashAlg = new string[] { "SHA2-224" },
                                    Curve = "P-224"
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
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfKc = new KdfKc()
                        {
                            KcOption = new KcOptions()
                            {
                                KcRole = new string[] { "provider", "recipient" },
                                KcType = new string[] { "unilateral", "bilateral" },
                                NonceType = new string[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions()
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
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
                    EccEphemeralUnified = new EccEphemeralUnified()
                    {
                        Role = new string[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
                                {
                                    HashAlg = new string[] { "SHA2-224" },
                                    Curve = "P-224"
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
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
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
                    EccOnePassUnified = new EccOnePassUnified()
                    {
                        Role = new string[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
                                {
                                    HashAlg = new string[] { "SHA2-224" },
                                    Curve = "P-224"
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
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfKc = new KdfKc()
                        {
                            KcOption = new KcOptions()
                            {
                                KcRole = new string[] { "provider", "recipient" },
                                KcType = new string[] { "unilateral", "bilateral" },
                                NonceType = new string[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions()
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
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
                    EccOnePassMqv = new EccOnePassMqv()
                    {
                        Role = new string[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
                                {
                                    HashAlg = new string[] { "SHA2-224" },
                                    Curve = "P-224"
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
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfKc = new KdfKc()
                        {
                            KcOption = new KcOptions()
                            {
                                KcRole = new string[] { "provider", "recipient" },
                                KcType = new string[] { "unilateral", "bilateral" },
                                NonceType = new string[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions()
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
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
                    EccOnePassDh = new EccOnePassDh()
                    {
                        Role = new string[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
                                {
                                    HashAlg = new string[] { "SHA2-224" },
                                    Curve = "P-224"
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
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfKc = new KdfKc()
                        {
                            KcOption = new KcOptions()
                            {
                                KcRole = new string[] { "provider", "recipient" },
                                KcType = new string[] { "unilateral", "bilateral" },
                                NonceType = new string[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions()
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
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
                    EccStaticUnified = new EccStaticUnified()
                    {
                        Role = new string[] { "initiator", "responder" },
                        NoKdfNoKc = new NoKdfNoKc()
                        {
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
                                {
                                    HashAlg = new string[] { "SHA2-224" },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfNoKc = new KdfNoKc()
                        {
                            DkmNonceTypes = new string[] { "randomNonce" },
                            KdfOption = new KdfOptions()
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
                                        {
                                            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                                            MacLen = 128
                                        }
                                    },
                                    Curve = "P-224"
                                }
                            }
                        },
                        KdfKc = new KdfKc()
                        {
                            DkmNonceTypes = new string[] { "randomNonce" },
                            KcOption = new KcOptions()
                            {
                                KcRole = new string[] { "provider", "recipient" },
                                KcType = new string[] { "unilateral", "bilateral" },
                                NonceType = new string[] { "randomNonce" }
                            },
                            KdfOption = new KdfOptions()
                            {
                                Asn1 = "uPartyInfo||vPartyInfo||literal[cafecafe]"
                            },
                            ParameterSet = new ParameterSets()
                            {
                                Eb = new Eb()
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
                                        HmacSha2_D224 = new MacOptionHmacSha2_d224()
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