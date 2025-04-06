﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XECDH.Tests
{
    [TestFixture, LongCryptoTest]
    public class XecdhTests
    {
        [Test]
        #region X25519
        [TestCase(Curve.Curve25519,
            "a546e36bf0527c9d3b16154b82465edd62144c0ac1fc5a18506a2244ba449ac4",
            "e6db6867583030db3594c1a424b15f7c726624ec26b3353b10a903a6d0ab1c4c",
            "c3da55379de9c6908e94ea4df28d084f32eccf03491c71f754b4075577a28552",
            TestName = "X25519 - RFC 7748 - 1")]
        [TestCase(Curve.Curve25519,
            "4b66e9d4d1b4673c5ad22691957d6af5c11b6421e0ea01d42ca4169e7918ba0d",
            "e5210f12786811d3f4b7959d0538ae2c31dbe7106fc03c3efc4cd549c715a493",
            "95cbde9476e8907d7aade45cb4b873f88b595a68799fa152e6f8f7647aac7957",
            TestName = "X25519 - RFC 7748 - 2")]
        #endregion X25519
        #region X448
        [TestCase(Curve.Curve448,
            "3d262fddf9ec8e88495266fea19a34d28882acef045104d0d1aae121700a779c984c24f8cdd78fbff44943eba368f54b29259a4f1c600ad3",
            "06fce640fa3487bfda5f6cf2d5263f8aad88334cbd07437f020f08f9814dc031ddbdc38c19c6da2583fa5429db94ada18aa7a7fb4ef8a086",
            "ce3e4ff95a60dc6697da1db1d85e6afbdf79b50a2412d7546d5f239fe14fbaadeb445fc66a01b0779d98223961111e21766282f73dd96b6f",
            TestName = "X448 - RFC 7748 - 1")]
        [TestCase(Curve.Curve448,
            "203d494428b8399352665ddca42f9de8fef600908e0d461cb021f8c538345dd77c3e4806e25f46d3315c44e0a5b4371282dd2c8d5be3095f",
            "0fbcc2f993cd56d3305b0b7d9e55d4c1a8fb5dbb52f8e9a1e9b6201b165d015894e56c4d3570bee52fe205e28a78b91cdfbde71ce8d157db",
            "884a02576239ff7a2f2f63b2db6a9ff37047ac13568e1e30fe63c4a7ad1b3ee3a5700df34321d62077e63633c575c1c954514e99da7c179d",
            TestName = "X448 - RFC 7748 - 2")]
        #endregion X448
        public void ShouldPerformXECDHCorrectly(Curve curveEnum, string scalarHex, string inputUHex, string outputUHex)
        {
            var scalar = LoadValue(scalarHex);
            var inputU = LoadValue(inputUHex);
            var outputU = LoadValue(outputUHex);

            var factory = new XecdhFactory();
            var subject = factory.GetXecdh(curveEnum, EntropyProviderTypes.Testable);

            var output = subject.XECDH(scalar, inputU);

            Assert.That(output, Is.EqualTo(outputU), "output u-coordinate");
        }

        [Test]
        #region X25519
        [TestCase(Curve.Curve25519,
            "0900000000000000000000000000000000000000000000000000000000000000",
            "684cf59ba83309552800ef566f2f4d3c1c3887c49360e3875f2eb94d99532c51",
            TestName = "X25519 - RFC 7748")]
        #endregion X25519
        #region X448
        [TestCase(Curve.Curve448,
            "0500000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000",
            "aa3b4749d55b9daf1e5b00288826c467274ce3ebbdd5c17b975e09d4af6c67cf10d087202db88286e2b79fceea3ec353ef54faa26e219f38",
            TestName = "X448 - RFC 7748")]
        #endregion X448
        public void ShouldPerformIteratedXECDHCorrectly(Curve curveEnum, string inputHex, string outputHex)
        {
            var input = LoadValue(inputHex);
            var k = input;
            var u = input;
            var output = LoadValue(outputHex);

            var factory = new XecdhFactory();
            var subject = factory.GetXecdh(curveEnum, EntropyProviderTypes.Testable);

            for (var i = 0; i < 1000; i++)
            {
                (k, u) = (subject.XECDH(k, u), k);
            }

            Assert.That(k, Is.EqualTo(output), "output");
        }

        [Test]
        #region KeyPairGen-25519
        [TestCase(Curve.Curve25519,
            "77076d0a7318a57d3c16c17251b26645df4c2f87ebc0992ab177fba51db92c2a",
            "8520f0098930a754748b7ddcb43ef75a0dbf3a0d26381af4eba4a98eaa9b4e6a",
            TestName = "KeyGen 25519 - RFC 7748 - Alice")]
        [TestCase(Curve.Curve25519,
            "5dab087e624a8a4b79e17f8b83800ee66f3bb1292618b6fd1c2f8b27ff88e0eb",
            "de9edb7d7b7dc1b4d35b61c2ece435373f8343c85b78674dadfc7e146f882b4f",
            TestName = "KeyGen 25519 - RFC 7748 - Bob")]
        #endregion KeyPairGen-25519
        #region KeyPairGen-448
        [TestCase(Curve.Curve448,
            "9a8f4925d1519f5775cf46b04b5800d4ee9ee8bae8bc5565d498c28dd9c9baf574a9419744897391006382a6f127ab1d9ac2d8c0a598726b",
            "9b08f7cc31b7e3e67d22d5aea121074a273bd2b83de09c63faa73d2c22c5d9bbc836647241d953d40c5b12da88120d53177f80e532c41fa0",
            TestName = "KeyGen 448 - RFC 7748 - Alice")]
        [TestCase(Curve.Curve448,
            "1c306a7ac2a0e2e0990b294470cba339e6453772b075811d8fad0d1d6927c120bb5ee8972b0d3e21374c9c921b09d1b0366f10b65173992d",
            "3eb7a829b0cd20f5bcfc0b599b6feccf6da4627107bdb0d4f345b43027d8b972fc3e34fb4232a13ca706dcb57aec3dae07bdc1c67bf33609",
            TestName = "KeyGen 448 - RFC 7748 - Bob")]
        #endregion KeyPairGen-448
        public void ShouldGenerateKeyPairsCorrectly(Curve curveEnum, string privHex, string pubHex)
        {
            var priv = LoadValue(privHex);
            var pub = LoadValue(pubHex);

            var entropyProvider = new EntropyProviderFactory().GetEntropyProvider(EntropyProviderTypes.Testable);
            entropyProvider.AddEntropy(priv);

            var factory = new XecdhFactory();
            var subject = factory.GetXecdh(curveEnum, entropyProvider);

            var result = subject.GenerateKeyPair();

            Assert.That(result.Success, Is.True);
            Assert.That(result.KeyPair.PrivateKey, Is.EqualTo(priv), "private key");
            Assert.That(result.KeyPair.PublicKey, Is.EqualTo(pub), "public key");
        }

        private BitString LoadValue(string value)
        {
            return new BitString(value);
        }
    }
}
