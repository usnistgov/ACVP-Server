using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Dilithium.Tests;

[TestFixture, UnitTest]
public class ExternalSignatureBaseTests
{
    // This should happen in Crypto.Common.Tests but that doesn't exist so it has to go into one of the PQC test projects
    // All testing happens using the FakeExternalSignatureBase that is set up with the following properties...
    //      Sign(sk, message, rnd): return message
    //      Verify(pk, message, sig): return message == sig
    // 
    // This allows for full testing of the generation of mPrime in all cases.

    [Test]
    [TestCase("ABCD", "123456", "0003123456ABCD")]
    public void ShouldGenerateMPrimeCorrectlyExternalPure(string messageHex, string contextHex, string expectedResultHex)
    {
        // result should just be 0x00 || len(ctx) || ctx || M
        var message = new BitString(messageHex).ToBytes();
        var context = new BitString(contextHex).ToBytes();
        var expectedResult = new BitString(expectedResultHex).ToBytes();
        
        var fake = new FakeExternalSignatureBase(new NativeShaFactory());
    
        // deterministic is not used
        var result = fake.ExternalSign(null, message, true, context);
        Assert.That(expectedResult, Is.EqualTo(result));
    }

    [Test]
    [TestCase(ModeValues.SHA2, DigestSizes.d224, "ABCD", "123456", "010312345606096086480165030402049B4F26AA9C504334597013989871FFFB91C354CC549E2008EB14934E")]
    [TestCase(ModeValues.SHA2, DigestSizes.d256, "ABCDEF", "", "01000609608648016503040201995DA3CF545787D65F9CED52674E92EE8171C87C7A4008AA4349EC47D21609A7")]
    [TestCase(ModeValues.SHA2, DigestSizes.d384, "ABCD1234ABCD1234", "00", "01010006096086480165030402026983DFAB1DC54F9E2EF11201E4D2C945F6EC3D0E8B5EC415EA9FF8C1D3010B75BD37BD8780B1A9B2A95E4EB7D4E7C07C")]
    [TestCase(ModeValues.SHA2, DigestSizes.d512, "ABCD", "123456123456123456123456123456123456123456123456123456123456", "011E12345612345612345612345612345612345612345612345612345612345606096086480165030402039B3A86C5DDDF6C13ACB969DB8F54EBB9FA50DC4F902EFF843380CEBFC9CC53FFC2CC4A5F4DDA0FAC9E3315578FAEBC999EE61609434E10A6C75E3D984EE2A426")]
    [TestCase(ModeValues.SHA2, DigestSizes.d512t224, "ABCD", "123456", "010312345606096086480165030402059EE1AA42313E7C14512DF4D714B38DDB98E5714521D40E59D3370CC6")]
    [TestCase(ModeValues.SHA2, DigestSizes.d512t256, "ABCD", "123456", "01031234560609608648016503040206AD9347408148D9D8731F16AD85AE953AACCBFABE49D999354801D613DBA0B688")]
    
    [TestCase(ModeValues.SHA3, DigestSizes.d224, "ABCD", "123456", "0103123456060960864801650304020793832AEE843C16D1F50B3E008E73898D3C238078DA8BAE62585369E1")]
    [TestCase(ModeValues.SHA3, DigestSizes.d256, "ABCDEF", "", "010006096086480165030402088B8A2A6BC589CD378FC57F47D5668C58B31167B2BF9E632696E5C2D50FC16002")]
    [TestCase(ModeValues.SHA3, DigestSizes.d384, "ABCD1234ABCD1234", "00", "01010006096086480165030402096DE69C8448E917DFA68A8D6917556FB547B196C1AB84F1230BF9363C73C8F0C971E942F6E98F927399494F56972FF0B6")]
    [TestCase(ModeValues.SHA3, DigestSizes.d512, "ABCD", "123456123456123456123456123456123456123456123456123456123456", "011E123456123456123456123456123456123456123456123456123456123456060960864801650304020A0A1391299F4FE72FA915FAD2263647C8B2AB868517093CF08C6C432A4D6B015F147FAB214B8182D5B01309F15812321E2593A3DC2A32A0DC3BB7023B735AD854")]

    [TestCase(ModeValues.SHAKE, DigestSizes.d128, "ABCD", "123456", "0103123456060960864801650304020B1D5EF4A42C8CB8EE37635232F0657DDC0AFF84EC63BC13E20E0DEF3B8275D7BC")]
    [TestCase(ModeValues.SHAKE, DigestSizes.d256, "ABCD", "123456", "0103123456060960864801650304020CC31FCBE076488758730DA7E5F8964AFEFA659A6F526B6F9CA67E599F318A7EA135E53A122EABA5055D10B3932EBAC2A72D3DC254ADCE1B75C56C69610C642332")]
    public void ShouldGenerateMPrimeCorrectlyExternalPreHash(ModeValues mode, DigestSizes digest, string messageHex, string contextHex, string expectedResultHex)
    {
        // result should be 0x01 || len(ctx) || ctx || OID || Hash(M)
        var message = new BitString(messageHex).ToBytes();
        var context = new BitString(contextHex).ToBytes();
        var expectedResult = new BitString(expectedResultHex).ToBytes();

        var hash = new HashFunction(mode, digest, true);
        
        var fake = new FakeExternalSignatureBase(new NativeShaFactory());
    
        // deterministic is not used
        var result = fake.ExternalPreHashSign(null, message, true, context, hash);
        Assert.That(expectedResult, Is.EqualTo(result));
    }

    [Test]
    // This is just raw concatenation, don't think a ton of test cases are needed
    [TestCase("ABCD", "123456", "0003123456ABCD")]
    public void ShouldGenerateMPrimeCorrectlyVerifyPure(string messageHex, string contextHex, string expectedResultHex)
    {
        // result should just be 0x00 || len(ctx) || ctx || M
        var message = new BitString(messageHex).ToBytes();
        var context = new BitString(contextHex).ToBytes();
        var expectedResult = new BitString(expectedResultHex).ToBytes();
        
        var fake = new FakeExternalSignatureBase(new NativeShaFactory());
    
        // Note weird interface for verify because it needs to return a bool but we want to know the value of mPrime
        var result = fake.ExternalVerify(null, message, context, expectedResult);
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(ModeValues.SHA2, DigestSizes.d224, "ABCD", "123456", "010312345606096086480165030402049B4F26AA9C504334597013989871FFFB91C354CC549E2008EB14934E")]
    [TestCase(ModeValues.SHA2, DigestSizes.d256, "ABCDEF", "", "01000609608648016503040201995DA3CF545787D65F9CED52674E92EE8171C87C7A4008AA4349EC47D21609A7")]
    [TestCase(ModeValues.SHA2, DigestSizes.d384, "ABCD1234ABCD1234", "00", "01010006096086480165030402026983DFAB1DC54F9E2EF11201E4D2C945F6EC3D0E8B5EC415EA9FF8C1D3010B75BD37BD8780B1A9B2A95E4EB7D4E7C07C")]
    [TestCase(ModeValues.SHA2, DigestSizes.d512, "ABCD", "123456123456123456123456123456123456123456123456123456123456", "011E12345612345612345612345612345612345612345612345612345612345606096086480165030402039B3A86C5DDDF6C13ACB969DB8F54EBB9FA50DC4F902EFF843380CEBFC9CC53FFC2CC4A5F4DDA0FAC9E3315578FAEBC999EE61609434E10A6C75E3D984EE2A426")]
    [TestCase(ModeValues.SHA2, DigestSizes.d512t224, "ABCD", "123456", "010312345606096086480165030402059EE1AA42313E7C14512DF4D714B38DDB98E5714521D40E59D3370CC6")]
    [TestCase(ModeValues.SHA2, DigestSizes.d512t256, "ABCD", "123456", "01031234560609608648016503040206AD9347408148D9D8731F16AD85AE953AACCBFABE49D999354801D613DBA0B688")]
    
    [TestCase(ModeValues.SHA3, DigestSizes.d224, "ABCD", "123456", "0103123456060960864801650304020793832AEE843C16D1F50B3E008E73898D3C238078DA8BAE62585369E1")]
    [TestCase(ModeValues.SHA3, DigestSizes.d256, "ABCDEF", "", "010006096086480165030402088B8A2A6BC589CD378FC57F47D5668C58B31167B2BF9E632696E5C2D50FC16002")]
    [TestCase(ModeValues.SHA3, DigestSizes.d384, "ABCD1234ABCD1234", "00", "01010006096086480165030402096DE69C8448E917DFA68A8D6917556FB547B196C1AB84F1230BF9363C73C8F0C971E942F6E98F927399494F56972FF0B6")]
    [TestCase(ModeValues.SHA3, DigestSizes.d512, "ABCD", "123456123456123456123456123456123456123456123456123456123456", "011E123456123456123456123456123456123456123456123456123456123456060960864801650304020A0A1391299F4FE72FA915FAD2263647C8B2AB868517093CF08C6C432A4D6B015F147FAB214B8182D5B01309F15812321E2593A3DC2A32A0DC3BB7023B735AD854")]

    [TestCase(ModeValues.SHAKE, DigestSizes.d128, "ABCD", "123456", "0103123456060960864801650304020B1D5EF4A42C8CB8EE37635232F0657DDC0AFF84EC63BC13E20E0DEF3B8275D7BC")]
    [TestCase(ModeValues.SHAKE, DigestSizes.d256, "ABCD", "123456", "0103123456060960864801650304020CC31FCBE076488758730DA7E5F8964AFEFA659A6F526B6F9CA67E599F318A7EA135E53A122EABA5055D10B3932EBAC2A72D3DC254ADCE1B75C56C69610C642332")]
    public void ShouldGenerateMPrimeCorrectlyVerifyPreHash(ModeValues mode, DigestSizes digest, string messageHex, string contextHex, string expectedResultHex)
    {
        // result should be 0x01 || len(ctx) || ctx || OID || Hash(M)
        var message = new BitString(messageHex).ToBytes();
        var context = new BitString(contextHex).ToBytes();
        var expectedResult = new BitString(expectedResultHex).ToBytes();

        var hash = new HashFunction(mode, digest, true);
        
        var fake = new FakeExternalSignatureBase(new NativeShaFactory());
    
        // deterministic is not used
        var result = fake.ExternalPreHashVerify(null, message, context, expectedResult, hash);
        Assert.That(result, Is.True);
    }
}
