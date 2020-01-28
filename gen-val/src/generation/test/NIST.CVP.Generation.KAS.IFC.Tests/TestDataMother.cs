using System.Collections.Generic;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Generation.KAS_IFC.Sp800_56Br2;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.IFC.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetVectorSet(string testType, KasKdf kdfType)
        {
            var vectorSet = new TestVectorSet();

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            var tg = new TestGroup()
            {
                KeyGenerationMethod = IfcKeyGenerationMethod.RsaKpg1_crt,
                L = 512,
                Modulo = 2048,
                Scheme = IfcScheme.Kas2_bilateral_keyConfirmation,
                IutId = new BitString("BEEFFACE"),
                KdfConfiguration = new OneStepConfiguration()
                {
                    L = 512,
                    AuxFunction = KasKdfOneStepAuxFunction.KMAC_128,
                    SaltLen = 128,
                    SaltMethod = MacSaltMethod.Default,
                    FixedInfoEncoding = FixedInfoEncoding.Concatenation,
                    FixedInfoPattern = "uPartyInfo||vPartyInfo",
                },
                MacConfiguration = new MacConfiguration()
                {
                    MacType = KeyAgreementMacType.Kmac_256,
                    KeyLen = 256,
                    MacLen = 256
                },
                TestType = testType,
                ServerId = new BitString("FACEFACE"),
            };
            testGroups.Add(tg);
            
            tg.Tests = new List<TestCase>()
            {
                new TestCase()
                {
                    ParentGroup = tg,
                    Dkm = new BitString("01"),
                    Tag = new BitString("02"),
                    Z = new BitString("03"),
                    IutC = new BitString("04"),
                    IutK = new BitString("05"),
                    IutKey = new KeyPair()
                    {
                        PrivKey = new CrtPrivateKey()
                        {
                            D = 6,
                            P = 7,
                            Q = 8,
                            DMP1 = 1,
                            DMQ1 = 2,
                            IQMP = 3
                        },
                        PubKey = new PublicKey()
                        {
                            E = 9,
                            N = 10
                        }
                    },
                    IutNonce = new BitString("11"),
                    IutZ = new BitString("12"),
                    ServerC = new BitString("13"),
                    ServerK = new BitString("14"),
                    ServerKey = new KeyPair()
                    {
                        PrivKey = new CrtPrivateKey()
                        {
                            D = 15,
                            P = 16,
                            Q = 17,
                            DMP1 = 5,
                            DMQ1 = 6,
                            IQMP = 7
                        },
                        PubKey = new PublicKey()
                        {
                            E = 18,
                            N = 19
                        }
                    },
                    ServerNonce = new BitString("20"),
                    ServerZ = new BitString("21"),
                    KdfParameter = new KdfParameterOneStep()
                    {
                        Z = new BitString("22"),
                        Salt = new BitString("23")
                    },
                    MacData = new BitString("24"),
                    MacKey = new BitString("25"),
                    TestPassed = true,
                    TestCaseDisposition = KasIfcValTestDisposition.FailKeyConfirmationBits
                }
            };
            
            return vectorSet;
        }
    }
}