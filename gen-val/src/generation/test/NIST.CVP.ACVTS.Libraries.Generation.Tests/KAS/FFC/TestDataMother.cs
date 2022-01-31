using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.FFC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.FFC
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

            List<TestGroup> list = new EditableList<TestGroup>();
            vectorSet.TestGroups = list;

            for (int i = 0; i < groups; i++)
            {
                TestGroup tg = new TestGroup()
                {
                    TestType = testType,
                    KasMode = kasMode,
                    MacType = macType,
                    KcRole = kcRole,
                    KcType = kcDirection,
                    KeyLen = 128,
                    ParmSet = FfcParameterSet.Fc,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    AesCcmNonceLen = 56,
                    MacLen = 128,
                    Function = KasAssurance.FullVal | KasAssurance.DpGen,
                    DomainParams = new FfcDomainParameters(1, 2, 3),
                    OiPattern = "uPartyInfo||vPartyInfo",
                    IdIut = new BitString("AA"),
                    KdfType = "concatenation",
                    IdServer = new BitString("BB"),
                    IdServerLen = 8,
                    Scheme = FfcScheme.DhOneFlow,
                    IdIutLen = 42,
                    TestGroupId = 55
                };
                list.Add(tg);

                tg.Tests = new List<TestCase>
                {
                    new TestCase()
                    {
                        OtherInfo = new BitString("AABB"),
                        TestPassed = true,
                        Tag = new BitString("0111"),
                        Z = new BitString("0122"),
                        IdIut = new BitString("0133"),
                        EphemeralKeyIut = new FfcKeyPair(1, 2),
                        StaticKeyIut = new FfcKeyPair(3, 4),
                        EphemeralKeyServer = new FfcKeyPair(5, 6),
                        StaticKeyServer = new FfcKeyPair(7, 8),
                        DkmNonceServer = new BitString("0144"),
                        EphemeralNonceServer = new BitString("0155"),
                        DkmNonceIut = new BitString("0166"),
                        EphemeralNonceIut = new BitString("0177"),
                        NonceAesCcm = new BitString("0188"),
                        MacData = new BitString("0199"),
                        OiLen = 9,
                        NonceNoKc = new BitString("1101"),
                        TestCaseId = 10,
                        Dkm = new BitString("2201"),
                        IdIutLen = 11,
                        HashZ = new BitString("3301"),
                        ParentGroup = tg
                    }
                };
            }

            return vectorSet;
        }
    }
}
