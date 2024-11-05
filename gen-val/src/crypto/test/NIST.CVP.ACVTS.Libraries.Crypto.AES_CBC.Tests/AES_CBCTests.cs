using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_CBC.Tests
{
    [TestFixture, FastCryptoTest]
    public class AES_CBCTests
    {
        [Test]
        public void SpecificTestNewEngineEncrypt()
        {
            var subject = new CbcBlockCipher(new AesEngine());

            var key = new BitString("70912e89521a485ac35a10ad06187eac");
            var iv = new BitString("603336e51e55003d3bcc69d050616bc9");
            var pt = new BitString("09424ff1ed3fb8ad593f82619a7e1794bbddc1fc1013ae78c3d3f34632215100ba95d395d6ecea98c3013efdf6194cb71680fff50c8882c14bd21f6be5e380c61e51c89ecce34a540a2c474ad17de2a6285fc677917777d93b60c466bab4dd26b2241d033ae3488d803e17db4963ec94fdd88ef84056a19f6647ca1bd08f3c2f92ea193b116db92c1ec3b3b0fe9a70b7ee15b0c88f902f44a9770c9720019b53");
            var ct = new BitString("37771cf060b9623c5e5410f140edcb72ec9ed152b73f2fbb33e4341048a625d187a732bfba50a941c0dd6a07981c9bedbb719c22af1e3d89d9c12f5f931eb91f7e1faca21919998b935141e2debf4dff051240aabff4a980448e9d995f4f6467c5e973e1a59d9ae180b97fcb3b5f4b707387a3321047da681d536504d7c08f73030d6ec48563150d2f1c0b0ad783fc08acdaaf01caa238090b166fe820f9a009");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                pt
            );
            var result = subject.ProcessPayload(param);

            Assert.That(result.Result.ToHex(), Is.EqualTo(ct.ToHex()));
        }

        [Test]
        public void SpecificTestNewEngineDecrypt()
        {
            var subject = new CbcBlockCipher(new AesEngine());

            var key = new BitString("70912e89521a485ac35a10ad06187eac");
            var iv = new BitString("603336e51e55003d3bcc69d050616bc9");
            var pt = new BitString("09424ff1ed3fb8ad593f82619a7e1794bbddc1fc1013ae78c3d3f34632215100ba95d395d6ecea98c3013efdf6194cb71680fff50c8882c14bd21f6be5e380c61e51c89ecce34a540a2c474ad17de2a6285fc677917777d93b60c466bab4dd26b2241d033ae3488d803e17db4963ec94fdd88ef84056a19f6647ca1bd08f3c2f92ea193b116db92c1ec3b3b0fe9a70b7ee15b0c88f902f44a9770c9720019b53");
            var ct = new BitString("37771cf060b9623c5e5410f140edcb72ec9ed152b73f2fbb33e4341048a625d187a732bfba50a941c0dd6a07981c9bedbb719c22af1e3d89d9c12f5f931eb91f7e1faca21919998b935141e2debf4dff051240aabff4a980448e9d995f4f6467c5e973e1a59d9ae180b97fcb3b5f4b707387a3321047da681d536504d7c08f73030d6ec48563150d2f1c0b0ad783fc08acdaaf01caa238090b166fe820f9a009");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                ct
            );
            var result = subject.ProcessPayload(param);

            Assert.That(result.Result.ToHex(), Is.EqualTo(pt.ToHex()));
        }
    }
}
