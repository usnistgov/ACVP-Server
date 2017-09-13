using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DSA.FFC.Tests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FfcDsaTests
    {
        private IShaFactory _shaFactory = new ShaFactory();

        [Test]
        public void ShouldSignCorrectly(string hash, string pHex, string qHex, string gHex, string xHex, string yHex, string kHex, string msgHex, string sigHex, string randHex)
        {
            var p = new BitString(pHex).ToPositiveBigInteger();
            var q = new BitString(qHex).ToPositiveBigInteger();
            var g = new BitString(gHex).ToPositiveBigInteger();
            var x = new BitString(xHex).ToPositiveBigInteger();
            var y = new BitString(yHex).ToPositiveBigInteger();
            var k = new BitString(kHex).ToPositiveBigInteger();
            var message = new BitString(msgHex);
            var expectedSignature = new BitString(sigHex).ToPositiveBigInteger();
            var expectedRandom = new BitString(randHex).ToPositiveBigInteger();

            var domainParams = new FfcDomainParameters(p, q, g);
            var keyPair = new FfcKeyPair(x, y);
            var dsa = new FfcDsa(GetSha(hash), EntropyProviderTypes.Testable);
            dsa.AddEntropy(k);

            var result = dsa.Sign(domainParams, keyPair, message);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(expectedSignature, result.Signature.S);
            Assert.AreEqual(expectedRandom, result.Signature.R);
        }

        [Test]
        public void ShouldVerifySignaturesCorrectly()
        {

        }

        [Test]
        [TestCase(
            "d38311e2cd388c3ed698e82fdf88eb92b5a9a483dc88005d4b725ef341eabb47cf8a7a8a41e792a156b7ce97206c4f9c5ce6fc5ae7912102b6b502e59050b5b21ce263dddb2044b652236f4d42ab4b5d6aa73189cef1ace778d7845a5c1c1c7147123188f8dc551054ee162b634d60f097f719076640e20980a0093113a8bd73",
            "96c5390a8b612c0e422bb2b0ea194a3ec935a281",
            "06b7861abbd35cc89e79c52f68d20875389b127361ca66822138ce4991d2b862259d6b4548a6495b195aa0e0b6137ca37eb23b94074d3c3d300042bdf15762812b6333ef7b07ceba78607610fcc9ee68491dbc1e34cd12615474e52b18bc934fb00c61d39e7da8902291c4434a4e2224c3f4fd9f93cd6f4f17fc076341a7e7d9",
            TestName = "FfcDsa KeyGen - 1024 - 160")]
        [TestCase(
            "904ef8e31e14721910fa0969e77c99b79f190071a86026e37a887a6053960dbfb74390a6641319fe0af32c4e982934b0f1f4c5bc57534e8e56d77c36f0a99080c0d5bc9022fa34f5892281d7b1009571cb5b35699303f912b276d86b1b0722fc0b1500f0ffb2e4d90867a3bdca181a9734617a8a9f991aa7c14dec1cf45ceba00600f8425440ed0c3b52c82e3aa831932a98b477da220867eb2d5e0ca34580b33b1b65e558411ed09c369f4717bf03b551787e13d9e47c267c91c697225265da157945cd8b32e84fc45b80533265239aa00a2dd3d05f5cb231b7daf724b7ecdce170360a83972e5be94626273d449f441be300a7345db387bebadad67d8060a7",
            "d7d0a83e84d13032b830ed74a6a88592ec9a4cf42bf37080c6600aad",
            "2050b18d3c9f39fac396c009310d6616f9309b67b59aef9aee813d6b4f12ee29ba8a6b350b11d4336d44b4641230002d870f1e6b1d8728bdd40262df0d2440999185ae077f7034c61679f4360fbb5d181569e7cb8acb04371c11ba55f1bbd777b74304b99b66d4405303e7120dc8bc4785f56e9533e65b63a0c77cce7bba0d5d6069df5edffa927c5a255a09405a008258ed93506a8433662154f6f67e922d7c9788f04d4ec09581063950d9cde8e373ea59a58b2a6df6ba8663345574fabba9ca981696d83aeac1f34f14f1a813ba900b3f0341dea23f7d3297f919a97e1ae00ac0728c93fe0a88b66591baf4eb0bc6900f39ba5feb41cbbeea7eb7919aa4d3",
            TestName = "FfcDsa KeyGen - 2048 - 224")]
        [TestCase(
            "ea1fb1af22881558ef93be8a5f8653c5a559434c49c8c2c12ace5e9c41434c9cf0a8e9498acb0f4663c08b4484eace845f6fb17dac62c98e706af0fc74e4da1c6c2b3fbf5a1d58ff82fc1a66f3e8b12252c40278fff9dd7f102eed2cb5b7323ebf1908c234d935414dded7f8d244e54561b0dca39b301de8c49da9fb23df33c6182e3f983208c560fb5119fbf78ebe3e6564ee235c6a15cbb9ac247baba5a423bc6582a1a9d8a2b4f0e9e3d9dbac122f750dd754325135257488b1f6ecabf21bff2947fe0d3b2cb7ffe67f4e7fcdf1214f6053e72a5bb0dd20a0e9fe6db2df0a908c36e95e60bf49ca4368b8b892b9c79f61ef91c47567c40e1f80ac5aa66ef7",
            "8ec73f3761caf5fdfe6e4e82098bf10f898740dcb808204bf6b18f507192c19d",
            "e4c4eca88415b23ecf811c96e48cd24200fe916631a68a684e6ccb6b1913413d344d1d8d84a333839d88eee431521f6e357c16e6a93be111a98076739cd401bab3b9d565bf4fb99e9d185b1e14d61c93700133f908bae03e28764d107dcd2ea7674217622074bb19efff482f5f5c1a86d5551b2fc68d1c6e9d8011958ef4b9c2a3a55d0d3c882e6ad7f9f0f3c61568f78d0706b10a26f23b4f197c322b825002284a0aca91807bba98ece912b80e10cdf180cf99a35f210c1655fbfdd74f13b1b5046591f8403873d12239834dd6c4eceb42bf7482e1794a1601357b629ddfa971f2ed273b146ec1ca06d0adf55dd91d65c37297bda78c6d210c0bc26e558302",
            TestName = "FfcDsa KeyGen - 2048 - 256")]
        [TestCase(
            "f335666dd1339165af8b9a5e3835adfe15c158e4c3c7bd53132e7d5828c352f593a9a787760ce34b789879941f2f01f02319f6ae0b756f1a842ba54c85612ed632ee2d79ef17f06b77c641b7b080aff52a03fc2462e80abc64d223723c236deeb7d201078ec01ca1fbc1763139e25099a84ec389159c409792080736bd7caa816b92edf23f2c351f90074aa5ea2651b372f8b58a0a65554db2561d706a63685000ac576b7e4562e262a14285a9c6370b290e4eb7757527d80b6c0fd5df831d36f3d1d35f12ab060548de1605fd15f7c7aafed688b146a02c945156e284f5b71282045aba9844d48b5df2e9e7a5887121eae7d7b01db7cdf6ff917cd8eb50c6bf1d54f90cce1a491a9c74fea88f7e7230b047d16b5a6027881d6f154818f06e513faf40c8814630e4e254f17a47bfe9cb519b98289935bf17673ae4c8033504a20a898d0032ee402b72d5986322f3bdfb27400561f7476cd715eaabb7338b854e51fc2fa026a5a579b6dcea1b1c0559c13d3c1136f303f4b4d25ad5b692229957",
            "d3eba6521240694015ef94412e08bf3cf8d635a455a398d6f210f6169041653b",
            "ce84b30ddf290a9f787a7c2f1ce92c1cbf4ef400e3cd7ce4978db2104d7394b493c18332c64cec906a71c3778bd93341165dee8e6cd4ca6f13afff531191194ada55ecf01ff94d6cf7c4768b82dd29cd131aaf202aefd40e564375285c01f3220af4d70b96f1395420d778228f1461f5d0b8e47357e87b1fe3286223b553e3fc9928f16ae3067ded6721bedf1d1a01bfd22b9ae85fce77820d88cdf50a6bde20668ad77a707d1c60fcc5d51c9de488610d0285eb8ff721ff141f93a9fb23c1d1f7654c07c46e58836d1652828f71057b8aff0b0778ef2ca934ea9d0f37daddade2d823a4d8e362721082e279d003b575ee59fd050d105dfd71cd63154efe431a0869178d9811f4f231dc5dcf3b0ec0f2b0f9896c32ec6c7ee7d60aa97109e09224907328d4e6acd10117e45774406c4c947da8020649c3168f690e0bd6e91ac67074d1d436b58ae374523deaf6c93c1e6920db4a080b744804bb073cecfe83fa9398cf150afa286dc7eb7949750cf5001ce104e9187f7e16859afa8fd0d775ae",
            TestName = "FfcDsa KeyGen - 3072 - 256")]
        public void ShouldGenerateKeysCorrectly(string pHex, string qHex, string gHex)
        {
            var p = new BitString(pHex).ToPositiveBigInteger();
            var q = new BitString(qHex).ToPositiveBigInteger();
            var g = new BitString(gHex).ToPositiveBigInteger();

            var dsa = new FfcDsa(null);
            var domainParams = new FfcDomainParameters(p, q, g);

            for(var i = 0; i < 100; i++)
            {
                var keyPairResult = dsa.GenerateKeyPair(domainParams);
                Assert.IsTrue(keyPairResult.Success, keyPairResult.ErrorMessage);

                var verifyResult = dsa.ValidateKeyPair(domainParams, keyPairResult.KeyPair);
                Assert.IsTrue(verifyResult.Success, verifyResult.ErrorMessage);
            }
        }

        [Test]
        public void ShouldVerifyKeysCorrectly()
        {

        }

        private ISha GetSha(string hash)
        {
            var mapping = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(hash);
            return _shaFactory.GetShaInstance(new HashFunction(mapping.shaMode, mapping.shaDigestSize));
        }
    }
}
