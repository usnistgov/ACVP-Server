using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.ECC.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups, bool isSample, 
            string testType,
            KasMode kasMode = KasMode.NoKdfNoKc,
            KeyAgreementMacType macType = KeyAgreementMacType.None,
            KeyConfirmationRole kcRole = KeyConfirmationRole.None,
            KeyConfirmationDirection kcDirection = KeyConfirmationDirection.None
        )
        {
            var vectorSet = new TestVectorSet();

            List<TestGroup> list = new List<TestGroup>();
            vectorSet.TestGroups = list;

            for (int i = 0; i < groups; i++)
            {
                var tg = new TestGroup
                {
                    TestType = testType,
                    KasMode = kasMode,
                    MacType = macType,
                    KcRole = kcRole,
                    KcType = kcDirection,
                    Scheme = EccScheme.EphemeralUnified,
                    KasRole = KeyAgreementRole.ResponderPartyV,
                    KeyLen = 128,
                    ParmSet = EccParameterSet.Ed,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    AesCcmNonceLen = 56,
                    MacLen = 128,
                    Function = KasAssurance.FullVal | KasAssurance.DpGen,
                    CurveName = Curve.P224,
                    OiPattern = "uPartyInfo||vPartyInfo",
                    IdIut = new BitString("AA"),
                    KdfType = "concatenation",
                    IdServer = new BitString("BB"),
                    IdServerLen = 8,
                    IdIutLen = 9,
                    NonceType = "something",
                    TestGroupId = 5
                };
                list.Add(tg);

                tg.Tests = new List<TestCase>()
                {
                    new TestCase()
                    {
                        ParentGroup = tg,
                        OtherInfo = new BitString("AABB"),
                        TestPassed = true,
                        Tag = new BitString("01"),
                        Z = new BitString("01"),
                        IdIut = new BitString("01"),
                        StaticPrivateKeyServer = 1,
                        StaticPublicKeyServerX = 2,
                        StaticPublicKeyServerY = 3,
                        EphemeralPrivateKeyServer = 4,
                        EphemeralPublicKeyServerX = 5,
                        EphemeralPublicKeyServerY = 6,
                        DkmNonceServer = new BitString("01"),
                        EphemeralNonceServer = new BitString("01"),
                        StaticPrivateKeyIut = 7,
                        StaticPublicKeyIutX = 8,
                        StaticPublicKeyIutY = 9,
                        EphemeralPrivateKeyIut = 10,
                        EphemeralPublicKeyIutX = 11,
                        EphemeralPublicKeyIutY = 12,
                        DkmNonceIut = new BitString("0155"),
                        EphemeralNonceIut = new BitString("0144"),
                        NonceAesCcm = new BitString("0166"),
                        MacData = new BitString("0771"),
                        OiLen = 13,
                        NonceNoKc = new BitString("0881"),
                        TestCaseId = 14,
                        Dkm = new BitString("0991"),
                        IdIutLen = 15,
                        HashZ = new BitString("0111")
                    }
                };
            }

            return vectorSet;
        }
    }
}
