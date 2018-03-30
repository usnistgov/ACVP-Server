using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA2.Tests
{
    [TestFixture, FastCryptoTest]
    public class RsaKeyComposerTests
    {
        [Test]
        [TestCase("0100000001",
            "ff03b1a74827c746db83d2eaff00067622f545b62584321256e62b01509f10962f9c5c8fd0b7f5184a9ce8e81f439df47dda14563dd55a221799d2aa57ed2713271678a5a0b8b40a84ad13d5b6e6599e6467c670109cf1f45ccfed8f75ea3b814548ab294626fe4d14ff764dd8b091f11a0943a2dd2b983b0df02f4c4d00b413",
            "dacaabc1dc57faa9fd6a4274c4d588765a1d3311c22e57d8101431b07eb3ddcb05d77d9a742ac2322fe6a063bd1e05acb13b0fe91c70115c2b1eee1155e072527011a5f849de7072a1ce8e6b71db525fbcda7a89aaed46d27aca5eaeaf35a26270a4a833c5cda681ffd49baa0f610bad100cdf47cc86e5034e2a0b2179e04ec7",
            "d9f3094b36634c05a02ae1a5569035107a48029e39b3c6a1853817f063e18e761c0c538e55ff2c7e53d603bb35cabb3b8d07f82aa0afdeaf7441fcf6746c5bcaaa2cde398ad73edb9c340c3ffca559132581eaf8f65c13d02f3445a932a3e1fadb5912f7553edec5047e4d0ed06ee87effc549e194d38e06b73a971c961688ba2d4aa4f450d2523372f317d41d06f9f0360e962ce953a69f36c53c370799fcfba195e8f691ebe862f84ae4bbd7747bc14499bd0efffcdc7154325908355c2ffc5b3948b8102b33aa2420381470e4ee858380ff0eea58288516c263f6d51dbbd0e477d1393a0a3ee60e1fde4330856665bf522006608a6104c138c0f39e09c4c5",
            "1bf009caddc664b4404d59711fde16d7c55822449de1c5a084d22ed5791fdaa37ea538867fc91a17e6856e277c2dedd70ca8bf6ec44b0e729917a88e5988cc561d948ddeea46e21fd8ff46cce7657c94bfb1bdf40b3b30d4595a8bc3a15f1d4ad4c665c09b3b265ba19cdb0b89cbaadd0097ff52e9f6e594f86829c5bb4e9ba0200f12fa6dc60fd28dec0d194f08deb50f5a7749540160d6e8338e75b11165b76f4650c2fcce08f979ad9941daedaa5e328473bf712f8f549c36967f5e15477dc643d1f48d563139134e5cdc4bb84f9782cd5125e864e067cb980290f215cb41090e297bac2714efba61115d85613851c2de50a82f4ab526b88c61b7c9a0b589")]
        [TestCase("0100000001",
            "f006d9631c1fc1f7b4feda14a87e4ba096394d49cbeb16e7b16fe201026dd370fe5d630139c7a4715c6278919a38aafa558092728feabbce49a23aa5a69245f78b705da5340e63b81fd0e8fc999f39e8c26c930bc92fb88bd81873fd67d89284a1ff6e677a8951d5589f7be18822cec86ad5092fb60bbe66cdd41628be6afc5ad03452bc72e926600e26252b5bae60af0f769e966bc231318152c671c2b69fe5d004898413a65478ed058cce1a1c6ae545afd3eed439dcd0f82a3965381d0a6f",
            "efbfd0ef2afa51bb74ad1b8aa1cab32ef68c02d91a4f49aebffa98bbd1a1b17d37286baa3221cabb8f6ef0951afbc6f2ae9390c29100b8235cdcef943aa5849e8fb07fb50d8451bc8734bd40ae98d3633ec63f95a1f73237c303127a6e4cf88b1a57a817c4bbb833a49e82dfc2f965709a6d11637221d7db74e8f9b4745eb12ca5ec4d571535e4e4e457f7ec02dca08a62a7b88946976ed5a1bf7da7cebd2a7a6cd87e49f7ec01989549f4fe750776213562d486fb89ecdc6f344a44ab05dc2d",
            "e0ca3df58784ef25839e8e25360a06c1fdab9f869e16d9f0308c68e4e794ca993f566f9f05a2cd2613a0dfcd1c5811dc96e26bb44a33de4d993b1505c480394f2ee6a151bd96bb2c067fc8d91b1840c5ff388d7f2f55813a08fc1d1b4b2160c3b7c7986b12ca4a859e755be9cfb1d6651baeae408e6ac14337fe6cc9579e62231f2e3297da0d001b503303d80ba39f01710a08a61ed04b1793fab5a132f8f9e173377f5f04e4262caac2c1a8ff59e306069e5e15a3b16823fb503fcff2383c2897a27d9f62a35d4ab0d92ebf9470cef8873eaf6231902da7b269a418ac9c2ea3ac59083dc768736789848a1506e080a489686fe795354713180aee28a22f729d2cf6c2a7caf9bf9025050c1eb83d9122280fc278b431e87752d21eac7d369eec96cbb89f323e658a7f723dd45a587abc3ccc78ac7dc2df33cc5e4cb4a28dcb270b4ad9ce72f7fba0e8818c20dbd33dc2cc8cdf64272a44419dd101a6c34a5e7c6347eff01ce16132b44c6b2f6568a3bd206d974902753d15307b6d092b3d3983",
            "39d70d55f2de05d384f4af6d405e7d6d52d5346e9b9eebc51281ca88fbb62c8454b3c37d33b6166b507db649241f5e8ece26750b098c62ad43825c98ecc6ef82cc9305c424a12a46a61aa33efea7642db91800892c5f9886b43795e946916749fa30cdf289dfc5bf7f939ec445c3b873af374a9566c1e441bbef08a6fdf154ec6559d78a54d571964e60663d4217ba82f53ed306ba5a870e344f0be2332b939ee97b75f34b846fb4ac53d2f489ab0c755c749fd7c3c124232149a05eb916a13bd2d16e474e379bf0ee32f1c3cd00c3c86a0a9ff7e699962cb80cd1c593b77f4ec84f78324496fd8ba2c1c7e315733372472f8ef0b6c148497573dbaf53014cc8fda091c2b196d38b72a57363eec2872e5d3bdba0e20fc3f0ed0fe378bf563d7d6a6cf8b4dca48e5581d5a65cd1e4e97ac30e9a093ab38351d86dba96d0898dece6935c4788a52633da355e322d2836e119de2d76727456e117e022e095600aa1af79139f539409baaffe2363f0fb2bf6341a4dd4ece72b193e865a5ce3b1cb4d")]
        public void ShouldComposeKeyCorrectly(string eHex, string pHex, string qHex, string nHex, string dHex)
        {
            var e = new BitString(eHex).ToPositiveBigInteger();
            var p = new BitString(pHex).ToPositiveBigInteger();
            var q = new BitString(qHex).ToPositiveBigInteger();
            var expectedN = new BitString(nHex).ToPositiveBigInteger();
            var expectedD = new BitString(dHex).ToPositiveBigInteger();

            var subject = new RsaKeyComposer();
            var result = subject.ComposeKey(e, new PrimePair {P = p, Q = q});

            // TODO fix the cast
            Assert.AreEqual(expectedN, result.PubKey.N, "n");
            Assert.AreEqual(expectedD, ((PrivateKey)result.PrivKey).D, "d");
        }
    }
}
