using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.ECC.Tests
{
    internal class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1)
        {
            List<TestGroup> list = new List<TestGroup>();

            for (int i = 0; i < groups; i++)
            {
                TestGroup tg = new TestGroup()
                {
                    Scheme = EccScheme.EphemeralUnified,
                    KasMode = KasMode.KdfNoKc,
                    KasRole = KeyAgreementRole.ResponderPartyV,
                    KeyLen = 128,
                    KcRole = KeyConfirmationRole.None,
                    ParmSet = EccParameterSet.Eb,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    MacType = KeyAgreementMacType.AesCcm,
                    AesCcmNonceLen = 56,
                    MacLen = 128,
                    Function = KasAssurance.FullVal | KasAssurance.DpGen,
                    CurveName = Curve.P224,
                    OiPattern = "uPartyInfo||vPartyInfo",
                    TestType = "VAL",
                    IdIut = new BitString("AA"),
                    KcType = KeyConfirmationDirection.None,
                    KdfType = "concatenation",
                    IdServer = new BitString("BB"),
                    IdServerLen = 8
                };
                tg.Tests = new List<ITestCase>()
                {
                    new TestCase()
                    {
                        OtherInfo = new BitString("AABB"),
                        FailureTest = false,
                        Tag = new BitString("01"),
                        Z = new BitString("01"),
                        IdIut = new BitString("01"),
                        StaticPrivateKeyServer = 1,
                        StaticPublicKeyServerX = 1,
                        StaticPublicKeyServerY = 1,
                        EphemeralPrivateKeyServer = 1,
                        EphemeralPublicKeyServerX = 1,
                        EphemeralPublicKeyServerY = 1,
                        DkmNonceServer = new BitString("01"),
                        EphemeralNonceServer = new BitString("01"),
                        StaticPrivateKeyIut = 1,
                        StaticPublicKeyIutX = 1,
                        StaticPublicKeyIutY = 1,
                        EphemeralPrivateKeyIut = 1,
                        EphemeralPublicKeyIutX = 1,
                        EphemeralPublicKeyIutY = 1,
                        DkmNonceIut = new BitString("01"),
                        EphemeralNonceIut = new BitString("01"),
                        NonceAesCcm = new BitString("01"),
                        MacData = new BitString("01"),
                        OiLen = 1,
                        NonceNoKc = new BitString("01"),
                        TestCaseId = 1,
                        Dkm = new BitString("01"),
                        IdIutLen = 1,
                        HashZ = new BitString("01"),
                        Result = "pass"
                    }
                };
                list.Add(tg);
            }

            return list;
        }
    }
}
