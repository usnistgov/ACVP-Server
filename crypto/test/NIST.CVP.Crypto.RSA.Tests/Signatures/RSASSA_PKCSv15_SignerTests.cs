using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.RSA.Tests.Signatures
{
    [TestFixture, FastIntegrationTest]
    public class RSASSA_PKCSv15_SignerTests
    {
        [Test]
        public void ShouldSignCorrectly()
        {
            var signer = new RSASSA_PKCSv15_Signer(new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d224 });
            var message = new BitString("74230447bcd492f2f8a8c594a04379271690bf0c8a13ddfc1b7b96413e77ab2664cba1acd7a3c57ee5276e27414f8283a6f93b73bd392bd541f07eb461a080bb667e5ff095c9319f575b3893977e658c6c001ceef88a37b7902d4db31c3e34f3c164c47bbeefde3b946bad416a752c2cafcee9e401ae08884e5b8aa839f9d0b5");
            var d = new BitString("0997634c477c1a039d44c810b2aaa3c7862b0b88d3708272e1e15f66fc9389709f8a11f3ea6a5af7effa2d01c189c50f0d5bcbe3fa272e56cfc4a4e1d388a9dcd65df8628902556c8b6bb6a641709b5a35dd2622c73d4640bfa1359d0e76e1f219f8e33eb9bd0b59ec198eb2fccaae0346bd8b401e12e3c67cb629569c185a2e0f35a2f741644c1cca5ebb139d77a89a2953fc5e30048c0e619f07c8d21d1e56b8af07193d0fdf3f49cd49f2ef3138b5138862f1470bd2d16e34a2b9e7777a6c8c8d4cb94b4e8b5d616cd5393753e7b0f31cc7da559ba8e98d888914e334773baf498ad88d9631eb5fe32e53a4145bf0ba548bf2b0a50c63f67b14e398a34b0d").ToPositiveBigInteger();
            var e = new BitString("260445").ToPositiveBigInteger();
            var n = new BitString("cea80475324c1dc8347827818da58bac069d3419c614a6ea1ac6a3b510dcd72cc516954905e9fef908d45e13006adf27d467a7d83c111d1a5df15ef293771aefb920032a5bb989f8e4f5e1b05093d3f130f984c07a772a3683f4dc6fb28a96815b32123ccdd13954f19d5b8b24a103e771a34c328755c65ed64e1924ffd04d30b2142cc262f6e0048fef6dbc652f21479ea1c4b1d66d28f4d46ef7185e390cbfa2e02380582f3188bb94ebbf05d31487a09aff01fcbb4cd4bfd1f0a833b38c11813c84360bb53c7d4481031c40bad8713bb6b835cb08098ed15ba31ee4ba728a8c8e10f7294e1b4163b7aee57277bfd881a6f9d43e02c6925aa3a043fb7fb78d").ToPositiveBigInteger();
            var key = new KeyPair { PrivKey = new PrivateKey { D = d }, PubKey = new PublicKey { E = e, N = n } };

            var result = signer.Sign(2048, message, key);

            Assert.AreEqual("27da4104eace1991e08bd8e7cfccd97ec48b896a0e156ce7bdc23fd570aaa9a00ed015101f0c6261c7371ceca327a73c3cecfcf6b2d9ed920c9698046e25c89adb2360887d99983bf632f9e6eb0e5df60715902b9aeaa74bf5027aa246510891c74ae366a16f397e2c8ccdc8bd56aa10e0d01585e69f8c4856e76b53acfd3d782b8171529008fa5eff030f46956704a3f5d9167348f37021fc277c6c0a8f93b8a23cfbf918990f982a56d0ed2aa08161560755adc0ce2c3e2ab2929f79bfc0b24ff3e0ff352e6445d8a617f1785d66c32295bb365d61cfb107e9993bbd93421f2d344a86e4127827fa0d0b2535f9b1d547de12ba2868acdecf2cb5f92a6a159a", result.Signature.ToHex());
        }
    }
}
