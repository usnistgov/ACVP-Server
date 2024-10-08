﻿using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.ECC
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
                    Curve = Curve.P224,
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
                        StaticKeyServer = new EccKeyPair(new EccPoint(1, 2), 3),
                        EphemeralKeyServer = new EccKeyPair(new EccPoint(4, 5), 6),
                        StaticKeyIut = new EccKeyPair(new EccPoint(1, 2), 3),
                        EphemeralKeyIut = new EccKeyPair(new EccPoint(4, 5), 6),
                        DkmNonceServer = new BitString("01"),
                        EphemeralNonceServer = new BitString("01"),
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
