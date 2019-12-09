using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TLS.Tests
{
    [TestFixture, FastCryptoTest]
    public class TlsKdfv12Tests
    {
        [Test]
        [TestCase(1024, ModeValues.SHA2, DigestSizes.d256,
            "d9251356c301bde554a7202675246a652952ff82f54da3317382a61f3ea2a57fcbdfde0279313218f0fedaf8e901c5b0",
            "2417c649b217775a2a204e2dd5b272fc0f17baeca0db61bcdd4b4ec7d1f8f815",
            "1e068e84018e80832c5343b8dbe0a39ecff5f09360f0af0e79cf2a134c6bd73d",
            "072a5c8a0c23790f2df8bae90230a1e02404a9208d8de6b2654a67d1ded698e0",
            "6c8a9d0ae0a2f162aa4accd8d45833e34b576665d2a019dfe90dd5a28f63fe42",
            "f886e1b095b6cba5dbb1959f830368d5cadda8b0a394a5ce2218a55e8b2bd60f776ee3cb2a1218e970846e72bef3dd19",
            "c3ea18179a15b0c09cd795d5a8c0cbee7c0fa7aebcfe87bea9bde2948fcf39f9bb6db0bbde46dbd1187ed8bc0afd5b913c84055f9c4ad6187e994dc8389f06b4f1bd2f6f6a6a18f173c2b8abf5d7e23f571a08a3df43c99cf0b11f90ec58e94adcf75e199f3791dd95f461994bf7a544a00d4557a4d32889ee0d0c2f81a71c3c",
            TestName = "TLS 1.2 - SHA2-256 - Test #1")]
        [TestCase(1024, ModeValues.SHA2, DigestSizes.d384,
            "cbabad9ad6e402382a5fc3cf27c08443fd66eda7d5b505f65b1bdbca0854d11bc848ce3d0a69665ba516daf4170b4d98",
            "595fb21e86858eb148fd280b372193bf0189a3c083a40a8f3528db9b626e5406",
            "50a929418255cdb7e66ac4291af52f4396e261c0e546eb7a78615382f08d5cd6",
            "af8cab6ac4e4cf859e826a523c4c2206feb178639245418e85bc534defa9ceb4",
            "3fe71eafa0acbdc017710ee6ec67820e64872d80212e68c478f7ed7d6a584b2e",
            "aa828e9a18b98337835d2ee0e73a790b61ba41fea3444d9db872c48afee22faf4b9d890f7cc6fa779bd629b3d703afef",
            "1ecb51e2998265cd5e5462051904ba1b2a17760bafc59910550b4708637620a984423b694ac782fb4f2796b51336b85f6e5572ac58c727eae0fd83948e68f675685781bde7173f4cca903114aded3c3e9373a0a05e3753ff85f9a99ccde3915a626023f33e5b3c09d3331340e89238c3a59af1d6e8c50d062afd4f640ef603ce",
            TestName = "TLS 1.2 - SHA2-384 - Test #2")]
        [TestCase(1024, ModeValues.SHA2, DigestSizes.d512, 
            "10325c7de098a25e1366c31c3dedf47cb99d39bf7b6643a689c9a34b27c22133022f40104740a0d7566143af5fbec7bd",
            "6b0fb7a2662ad42f692ac1cee98eb8968ae660a323f61d3c16c4844a8ed513d7",
            "db18673482c23cfcd1634f33db1d58bd61ca4a5d1790f219a178cf1781f93c44",
            "fe8d40431d21ad71561489142d30295278c7cbb99b3742c87444bd54c9082097",
            "ca4efc9d28835ba7dc715aee6b8b0bffa79681ed3b5e83518e30a43bcfc85f86",
            "70e770b05757c7d3dae14ac0a72ac9598e07f83584ec5705a2d9eb119d02029f2d99d5c34b5e90acf97b3c3e0a8aaf47",
            "87ce398e98d5c80b2d1f183342b43dcebc0b37588e857552c56ea8a47afb0f1902c5fd1cc845c7c1d9ac3bbc13d36bd363b1371a89a1d82aebf6a028bb2221c2c8b49a8672737f85c358d3687b61270c0106fe768ff46df9bc3aa8901b04d02e53b03fdc6824b3c61cbb70577e75febb1c273003de0f41969a825ade1fc3b9f0",
            TestName = "TLS 1.2 - SHA2-512 Test #3")]
        public void ShouldTlsKdfCorrectly(int kbLen, ModeValues mode, DigestSizes digestSize, string pmsHex, string shrHex, string chrHex, string srHex, string crHex, string msHex, string kbHex)
        {
            var preMasterSecret = new BitString(pmsHex);
            var serverHelloRandom = new BitString(shrHex);
            var clientHelloRandom = new BitString(chrHex);
            var serverRandom = new BitString(srHex);
            var clientRandom = new BitString(crHex);

            var expectedMasterSecret = new BitString(msHex);
            var expectedKeyBlock = new BitString(kbHex);

            var hmac = new HmacFactory(new ShaFactory()).GetHmacInstance(new HashFunction(mode, digestSize));
            var subject = new TlsKdfv12(hmac);

            var result = subject.DeriveKey(preMasterSecret, clientHelloRandom, serverHelloRandom, clientRandom, serverRandom, kbLen);
            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(expectedMasterSecret, result.MasterSecret, "master secret");
            Assert.AreEqual(expectedKeyBlock, result.DerivedKey, "key block");
        }
    }
}
