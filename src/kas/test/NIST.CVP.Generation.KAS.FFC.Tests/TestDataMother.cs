using Castle.Components.DictionaryAdapter;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.KAS.v1_0.FFC;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using System.Collections.Generic;

namespace NIST.CVP.Generation.KAS.FFC.Tests
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
                    P = 1.ToBitString(),
                    Q = 2.ToBitString(),
                    G = 3.ToBitString(),
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
                        StaticPrivateKeyServer = 1.ToBitString(),
                        StaticPublicKeyServer = 2.ToBitString(),
                        EphemeralPrivateKeyServer = 3.ToBitString(),
                        EphemeralPublicKeyServer = 4.ToBitString(),
                        DkmNonceServer = new BitString("0144"),
                        EphemeralNonceServer = new BitString("0155"),
                        StaticPrivateKeyIut = 5.ToBitString(),
                        StaticPublicKeyIut = 6.ToBitString(),
                        EphemeralPrivateKeyIut = 7.ToBitString(),
                        EphemeralPublicKeyIut = 8.ToBitString(),
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