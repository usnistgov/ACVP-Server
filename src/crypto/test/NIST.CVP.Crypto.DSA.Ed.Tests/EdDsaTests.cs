using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DSA.Ed.Tests
{
    [TestFixture, LongCryptoTest]
    public class EdDsaTests
    {
        [Test]
        #region KeyPairGen-25519
        [TestCase(Curve.Ed25519,
            "9d61b19deffd5a60ba844af492ec2cc44449c5697b326919703bac031cae7f60",
            "d75a980182b10ab7d54bfed3c964073a0ee172f3daa62325af021a68f707511a",
            TestName = "KeyGen 25519 - 1")]
        [TestCase(Curve.Ed25519,
            "4ccd089b28ff96da9db6c346ec114e0f5b8a319f35aba624da8cf6ed4fb8a6fb",
            "3d4017c3e843895a92b70aa74d1b7ebc9c982ccf2ec4968cc0cd55f12af4660c",
            TestName = "KeyGen 25519 - 2")]
        [TestCase(Curve.Ed25519,
            "c5aa8df43f9f837bedb7442f31dcb7b166d38535076f094b85ce3a2e0b4458f7",
            "fc51cd8e6218a1a38da47ed00230f0580816ed13ba3303ac5deb911548908025",
            TestName = "KeyGen 25519 - 3")]
        [TestCase(Curve.Ed25519,
            "f5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5",
            "278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e",
            TestName = "KeyGen 25519 - 4")]
        [TestCase(Curve.Ed25519,
            "833fe62409237b9d62ec77587520911e9a759cec1d19755b7da901b96dca3d42",
            "ec172b93ad5e563bf4932c70e1245034c35467ef2efd4d64ebf819683467e2bf",
            TestName = "KeyGen 25519 - 5")]
        #endregion KeyPairGen-25519
        #region KeyPairGen-448
        [TestCase(Curve.Ed448,
            "6c82a562cb808d10d632be89c8513ebf6c929f34ddfa8c9f63c9960ef6e348a3528c8a3fcc2f044e39a3fc5b94492f8f032e7549a20098f95b",
            "5fd7449b59b461fd2ce787ec616ad46a1da1342485a70e1f8a0ea75d80e96778edf124769b46c7061bd6783df1e50f6cd1fa1abeafe8256180",
            TestName = "KeyGen 448 - 1")]
        [TestCase(Curve.Ed448,
            "c4eab05d357007c632f3dbb48489924d552b08fe0c353a0d4a1f00acda2c463afbea67c5e8d2877c5e3bc397a659949ef8021e954e0a12274e",
            "43ba28f430cdff456ae531545f7ecd0ac834a55d9358c0372bfa0c6c6798c0866aea01eb00742802b8438ea4cb82169c235160627b4c3a9480",
            TestName = "KeyGen 448 - 2")]
        [TestCase(Curve.Ed448,
            "cd23d24f714274e744343237b93290f511f6425f98e64459ff203e8985083ffdf60500553abc0e05cd02184bdb89c4ccd67e187951267eb328",
            "dcea9e78f35a1bf3499a831b10b86c90aac01cd84b67a0109b55a36e9328b1e365fce161d71ce7131a543ea4cb5f7e9f1d8b00696447001400",
            TestName = "KeyGen 448 - 3")]
        [TestCase(Curve.Ed448,
            "258cdd4ada32ed9c9ff54e63756ae582fb8fab2ac721f2c8e676a72768513d939f63dddb55609133f29adf86ec9929dccb52c1c5fd2ff7e21b",
            "3ba16da0c6f2cc1f30187740756f5e798d6bc5fc015d7c63cc9510ee3fd44adc24d8e968b6e46e6f94d19b945361726bd75e149ef09817f580",
            TestName = "KeyGen 448 - 4")]
        [TestCase(Curve.Ed448,
            "7ef4e84544236752fbb56b8f31a23a10e42814f5f55ca037cdcc11c64c9a3b2949c1bb60700314611732a6c2fea98eebc0266a11a93970100e",
            "b3da079b0aa493a5772029f0467baebee5a8112d9d3a22532361da294f7bb3815c5dc59e176b4d9f381ca0938e13c6c07b174be65dfa578e80",
            TestName = "KeyGen 448 - 5")]
        [TestCase(Curve.Ed448,
            "d65df341ad13e008567688baedda8e9dcdc17dc024974ea5b4227b6530e339bff21f99e68ca6968f3cca6dfe0fb9f4fab4fa135d5542ea3f01",
            "df9705f58edbab802c7f8363cfe5560ab1c6132c20a9f1dd163483a26f8ac53a39d6808bf4a1dfbd261b099bb03b3fb50906cb28bd8a081f00",
            TestName = "KeyGen 448 - 6")]
        [TestCase(Curve.Ed448,
            "2ec5fe3c17045abdb136a5e6a913e32ab75ae68b53d2fc149b77e504132d37569b7e766ba74a19bd6162343a21c8590aa9cebca9014c636df5",
            "79756f014dcfe2079f5dd9e718be4171e2ef2486a08f25186f6bff43a9936b9bfe12402b08ae65798a3d81e22e9ec80e7690862ef3d4ed3a00",
            TestName = "KeyGen 448 - 7")]
        [TestCase(Curve.Ed448,
            "872d093780f5d3730df7c212664b37b8a0f24f56810daa8382cd4fa3f77634ec44dc54f1c2ed9bea86fafb7632d8be199ea165f5ad55dd9ce8",
            "a81b2e8a70a5ac94ffdbcc9badfc3feb0801f258578bb114ad44ece1ec0e799da08effb81c5d685c0c56f64eecaef8cdf11cc38737838cf400",
            TestName = "KeyGen 448 - 8")]
        #endregion KeyPairGen-448
        public void ShouldGenerateKeyPairsCorrectly(Curve curveEnum, string dHex, string qHex)
        {
            var d = LoadValue(dHex);
            var q = LoadValue(qHex);

            var factory = new EdwardsCurveFactory();
            var curve = factory.GetCurve(curveEnum);

            var domainParams = new EdDomainParameters(curve, new ShaFactory());

            var subject = new EdDsa(EntropyProviderTypes.Testable);
            subject.AddEntropy(d);

            var result = subject.GenerateKeyPair(domainParams);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.KeyPair.PrivateD, d, "d");
            Assert.AreEqual(q, result.KeyPair.PublicQ, "q");
        }

        private BigInteger LoadValue(string value)
        {
            var bits = new BitString(value);
            return bits.ToPositiveBigInteger();
        }
    }
}
