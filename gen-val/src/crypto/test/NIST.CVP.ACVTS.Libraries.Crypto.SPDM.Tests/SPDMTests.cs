using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SPDM.Tests;

[TestFixture]
[FastCryptoTest]
public class SPDMTests
{  
    private HashFunction sha = new (ModeValues.SHA2, DigestSizes.d384);
    private Spdm spdm;

    [OneTimeSetUp]
    public void SetUp()
    {
        spdm = new Spdm(sha);
    }

    [Test]
    [TestCase("cd99ae6204f9779f274f06bb948bb0ee95a000585adf69317e4cbbd16e9629e197beddf81fe3332c87fe137933829252", "d42632379e857bfa3f72e00b536019a0a1d51e80021a6ee75739763ac88817db7bc7bbbf25fc4d694d86f831974fe740", 48, false, SPDMVersions.SPDM13, "b216d2e7377ce2f5d6738cf120e4869e3385f42b0817e99e4584a609d1e82335f626576538feb5bea37d0cc8fd25cfad", "717529164c426606d88415c5a98d485c83525d5eaa17c111bd5b97f68711e7e533ef4de8349cc3c5ceeeb9ccb5da650a")]
    public void ShouldGetMasterCorrectly(string masterString, string keyString, short hashLen, bool psk, SPDMVersions version, string TH1String = null, string TH2String = null)
    {
        BitString key = new BitString(keyString);
        //BitString TH1, TH2;
        //if (TH1String != null)
        //{
        //    TH1 = new BitString(TH1String);
        //}
        //if (TH2String != null)
        //{
        //    TH2 = new BitString(TH2String);
        //}

        BitString TH1 = new BitString(TH1String);
        BitString TH2 = new BitString(TH2String);

        //if(TH1String != null && TH2String != null)
        //if(TH1.BitLength > 0)
        //{
        //    SPDMResult res = spdm.KeySchedule(key, hashLen, psk, version, TH1, TH2);


        //}

        SPDMReturn res = spdm.KeySchedule(key, psk, version, TH1, TH2);

        BitString master = new BitString(masterString);
        Assert.That(res.ExportMaster, Is.EqualTo(master));
    }
}
