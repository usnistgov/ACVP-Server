using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.FFC.Tests
{
    internal class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1)
        {
            List<TestGroup> list = new EditableList<TestGroup>();

            for (int i = 0; i < groups; i++)
            {
                TestGroup tg = new TestGroup()
                {
                    Scheme = FfcScheme.DhEphem,
                    KasMode = KasMode.KdfNoKc,
                    KasRole = KeyAgreementRole.ResponderPartyV,
                    KeyLen = 128,
                    KcRole = KeyConfirmationRole.None,
                    ParmSet = FfcParameterSet.Fc,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    MacType = KeyAgreementMacType.AesCcm,
                    AesCcmNonceLen = 56,
                    MacLen = 128,
                    Function = KasAssurance.FullVal | KasAssurance.DpGen,
                    P = 1,
                    Q = 2,
                    G = 3,
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
                        StaticPublicKeyServer = 1,
                        EphemeralPrivateKeyServer = 1,
                        EphemeralPublicKeyServer = 1,
                        StaticPrivateKeyIut = 1,
                        StaticPublicKeyIut = 1,
                        EphemeralPrivateKeyIut = 1,
                        EphemeralPublicKeyIut = 1,
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