using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.ANSIX963.Tests
{
    [TestFixture, FastCryptoTest]
    public class AnsiX963Tests
    {
        [Test]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 128,
            "de6029f5d7d1e913137ce785b7728869c81a26c08649b8db39d6b882",
            "",
            "83f5f34f1376c0626fc823476292dde3",
            TestName = "ANSI X9.63 - SHA2-224 - 128")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 2048,
            "1f354b8e596d7adc08339173db9fee51c49390434844d21e78e58113",
            "47449403a3ce88a1a7c78fdf194e918171009b7dd7aad4b8f5483fb99c50229afbfb045d79cbbd040708e701f7f55081736755cbc1b2faa8a822f3a48c22a0a7",
            "089afbdb0e9fc0fe54f4ef3ec0e86e4d4963ebf925c29a2c4dcbd620fca56d9f4336cbd287b62f15bc2cad13a8751672f6f76e526aefbc4cc7622f18692bf210a9523f2ec034b3a499695a9c80bf6408213dc7027dfe1a0f8d2f0d30a1cb9662dc4b5d5a917ec9c15c1080ff8ea7e1c0d7f40f156b24ac93811e3279857a5e1515c9cf0ec93444aa33286bf30db6f2b85bf2a83c05eec58805cb7fef1c01f0a3d867d137fa5cfe10dcb9deb43c7553a5c62a3e4ded6456c2c0fb3b211b7a0f9d93f569f021fc9386d52c8c45d8b5c00ea7b9bc889a06e17c015365fe48f55decabd4c96943ecff669cf5874b4423a3028a56abc8145e349069183f0e51325864",
            TestName = "ANSI X9.63 - SHA2-224 - 2048")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 2048,
            "f7edd63d18d74a690cf3ea6222fe223b52a0388a46f609ad10fee9d7",
            "fce013b5b389f2ad21f661c2e64e2011c16e0f0221fb50b468a5b614882f3047aa9e9d1a944bf3472d503dc0f6a5bb48de0a51ccfb13a642b13cd4a538573282",
            "7c1e65b72d9f7c24655e42e2571629ea63da0c2df83ff25d8de93bebbbbad696c8ab7881aa7c2d78c422c2f5acf3b497c3a9bb31ab1cc91731ba7131314ca2c8c11ba15026c0644d669244bda4e2475448182fb84cc20316491060d9687567ed8c1e81e5154d2bd41dd820837e702554ea47d07f82c23441a7d14b4082f469aa9dfbb7b875090f562c7a5734aa801d1863706efa30ab050dbf202c9c04fd25f2d96a8c89d4ba3dd5191a5313bb6d6d24a853a87987e0f88aad587d9dabf86fab12a5e012f4a4c13ddf3a684ca3d99837d6615bc9d98a30d753da9e485fe2a82a3ba31e82bf9610fae91233e65f3bd1b105e130cc77bb2a92bef57e45b9f8a956",
            TestName = "ANSI X9.63 - SHA2-256 - 2048")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 128,
            "0a3891a3767e7c793985fcb657119e55f13e8a7999c6eb7ec715c100",
            "",
            "02b0c00c64797a24f695567bd65d8335",
            TestName = "ANSI X9.63 - SHA2-512 - 128")]
        public void ShouldAnsiX963Correctly(ModeValues mode, DigestSizes digestSize, int keyLength, string zHex, string sharedInfoHex, string keyData)
        {
            var hash = new HashFunction(mode, digestSize);
            var sha = new ShaFactory().GetShaInstance(hash);
            var z = new BitString(zHex);
            var sharedInfo = new BitString(sharedInfoHex);
            
            var subject = new AnsiX963(sha);
            var result = subject.DeriveKey(z, sharedInfo, keyLength);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(keyData, result.DerivedKey.ToHex().ToLower());
        }
    }
}
