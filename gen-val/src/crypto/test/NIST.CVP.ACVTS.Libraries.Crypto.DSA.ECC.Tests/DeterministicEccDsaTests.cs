using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC.Tests
{
    [TestFixture, LongCryptoTest]
    public class DeterministicEccDsaTests
    {
        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, Curve.P192)]

        [TestCase(ModeValues.SHA2, DigestSizes.d224, Curve.P192)]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, Curve.P224)]

        [TestCase(ModeValues.SHA2, DigestSizes.d256, Curve.P192)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, Curve.P224)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, Curve.P256)]
        public void ShouldVerifyRandomlyGeneratedSignatures(ModeValues mode, DigestSizes digest, Curve curveEnum)
        {
            var nonces = new List<BigInteger>();

            var hashFunction = new HashFunction(mode, digest);
            var shaFactory = new NativeShaFactory();
            var sha = shaFactory.GetShaInstance(hashFunction);
            var hmacFactory = new HmacFactory(shaFactory);
            var hmac = hmacFactory.GetHmacInstance(hashFunction);

            var subject = new EccDsa(sha, new DeterministicNonceProvider(hmac), EntropyProviderTypes.Random);

            var curveFactory = new EccCurveFactory();
            var curve = curveFactory.GetCurve(curveEnum);
            var domainParams = new EccDomainParameters(curve);
            var key = subject.GenerateKeyPair(domainParams).KeyPair;

            var rand = new Random800_90();

            for (var i = 0; i < 100; i++)
            {
                var message = rand.GetRandomBitString(1024);

                var signature = subject.Sign(domainParams, key, message).Signature;
                var verify = subject.Verify(domainParams, key, message, signature);

                nonces.Add(signature.R);

                Assert.That(verify.Success, Is.True, verify.ErrorMessage);
            }

            // Check nonces for uniqueness
            Assert.That(nonces.Distinct().Count(), Is.EqualTo(nonces.Count), "Repeated nonce detected");
        }
        
        
        /* Known Answer Tests taken from the test vectors in RFC 6979 A.2.7 */ 
        [Test]
        [TestCase(ModeValues.SHA1,
              DigestSizes.d160,
              "0089C071B419E1C2820962321787258469511958E80582E95D8378E0C2CCDB3CB42BEDE42F50E3FA3C71F5A76724281D31D9C89F0F91FC1BE4918DB1C03A5838D0F9",
              "00343B6EC45728975EA5CBA6659BBB6062A5FF89EEA58BE3C80B619F322C87910FE092F7D45BB0F8EEE01ED3F20BABEC079D202AE677B243AB40B5431D497C55D75D",
              "00E7B0E675A9B24413D448B8CC119D2BF7B2D2DF032741C096634D6D65D0DBE3D5694625FB9E8104D3B842C1B0E2D0B98BEA19341E8676AEF66AE4EBA3D5475D5D16")]
        [TestCase(ModeValues.SHA2,
            DigestSizes.d256,
            "00EDF38AFCAAECAB4383358B34D67C9F2216C8382AAEA44A3DAD5FDC9C32575761793FEF24EB0FC276DFC4F6E3EC476752F043CF01415387470BCBD8678ED2C7E1A0",
            "01511BB4D675114FE266FC4372B87682BAECC01D3CC62CF2303C92B3526012659D16876E25C7C1E57648F23B73564D67F61C6F14D527D54972810421E7D87589E1A7",
            "004A171143A83163D6DF460AAF61522695F207A58B95C0644D87E52AA1A347916E4F7A72930B1BC06DBE22CE3F58264AFD23704CBB63B29B931F7DE6C9D949A7ECFC")]
        [TestCase(ModeValues.SHA2,
            DigestSizes.d512,
            "01DAE2EA071F8110DC26882D4D5EAE0621A3256FC8847FB9022E2B7D28E6F10198B1574FDD03A9053C08A1854A168AA5A57470EC97DD5CE090124EF52A2F7ECBFFD3",
            "00C328FAFCBD79DD77850370C46325D987CB525569FB63C5D3BC53950E6D4C5F174E25A1EE9017B5D450606ADD152B534931D7D4E8455CC91F9B15BF05EC36E377FA",
            "00617CCE7CF5064806C467F678D3B4080D6F1CC50AF26CA209417308281B68AF282623EAA63E5B5C0723D8B8C37FF0777B1A20F8CCB1DCCC43997F1EE0E44DA4A67A")]
        public void ShouldVerifyKatSignatures( ModeValues mode, DigestSizes digestSize, string expectedK, string expectedR, string expectedS )
        {
            // RFC 6979 - ECDSA, 521 bits (Prime Field) [Page 38]
            var hashFunction = new HashFunction(mode, digestSize);
            var shaFactory = new NativeShaFactory();
            var sha = shaFactory.GetShaInstance(hashFunction);
            var hmacFactory = new HmacFactory(shaFactory);
            var hmac = hmacFactory.GetHmacInstance(hashFunction);
            var nonceProvider = new DeterministicNonceProvider(hmac);

            var subject = new EccDsa(sha, nonceProvider, EntropyProviderTypes.Random);

            var curveFactory = new EccCurveFactory();
            var curve = curveFactory.GetCurve(Curve.P521);
            var domainParams = new EccDomainParameters(curve);
            
            var keyPair = new EccKeyPair()
            {
                PublicQ = new EccPoint()
                {
                    X = new BitString(
                            "01894550D0785932E00EAA23B694F213F8C3121F86DC97A04E5A7167DB4E5BCD371123D46E45DB6B5D5370A7F20FB633155D38FFA16D2BD761DCAC474B9A2F5023A4")
                        .ToPositiveBigInteger(),
                    Y = new BitString(
                            "00493101C962CD4D2FDDF782285E64584139C2F91B47F87FF82354D6630F746A28A0DB25741B5B34A828008B22ACC23F924FAAFBD4D33F81EA66956DFEAA2BFDFCF5")
                        .ToPositiveBigInteger()
                },
                PrivateD = new BitString(
                        "00FAD06DAA62BA3B25D2FB40133DA757205DE67F5BB0018FEE8C86E1B68C7E75CAA896EB32F1F47C70855836A6D16FCC1466F6D8FBEC67DB89EC0C08B0E996B83538")
                    .ToPositiveBigInteger()
            };
            
            var message = new BitString(System.Text.Encoding.UTF8.GetBytes("sample"));
            
            // Should reflect the same deterministic k value here as within the Sign method below
            var k = nonceProvider.GetNonce(keyPair.PrivateD, sha.HashMessage(message).Digest, domainParams.CurveE.OrderN);
            
            var signature = subject.Sign(domainParams, keyPair, message).Signature;
            
            Assert.Multiple(() =>
            {
                Assert.That(k, Is.EqualTo(StringToBigInteger(expectedK)), "DetEccDsa->ShouldVerifyKatSignatures, K not the same");
                Assert.That(signature.R, Is.EqualTo(StringToBigInteger(expectedR)), "DetEccDsa->ShouldVerifyKatSignatures, R not the same");
                Assert.That(signature.S, Is.EqualTo(StringToBigInteger(expectedS)), "DetEccDsa->ShouldVerifyKatSignatures, S not the same");
            });
        }
        
        private BigInteger StringToBigInteger(string value)
        {
            return new BitString(value).ToPositiveBigInteger();
        }
    }
}
