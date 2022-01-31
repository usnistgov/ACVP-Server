using NIST.CVP.ACVTS.Libraries.Crypto.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.DRBG.ConditioningComponents;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.BlockCipher_DF.Tests
{
    [TestFixture, FastCryptoTest]
    public class BlockCipherDfTests
    {
        // Tests are self-generated
        [Test]
        [TestCase(128, "ABCD", "E1685586E5F9F730D243EE91C1D22852")]
        [TestCase(192, "ABCD", "F1B075B0790DAA2F5A586302F82B5E36")]
        [TestCase(256, "ABCD", "CF0639CC0CC23383A40E9389B93CBCB5")]
        public void ShouldComputeCorrectResult(int keyLen, string inputHex, string outputHex)
        {
            var input = new BitString(inputHex);
            var expectedOutput = new BitString(outputHex);

            var factory = new BlockCipherConditioningComponentFactory(new DrbgFactory(new NativeShaFactory(), new HmacFactory(new NativeShaFactory())));
            var subject = factory.GetInstance(keyLen);

            var result = subject.DerivationFunction(input, 128);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(expectedOutput.ToHex(), result.Bits.ToHex());
        }
    }
}
