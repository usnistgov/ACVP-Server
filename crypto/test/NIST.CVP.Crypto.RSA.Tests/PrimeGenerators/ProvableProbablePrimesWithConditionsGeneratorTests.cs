using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests.PrimeGenerators
{
    [TestFixture]
    public class ProvableProbablePrimesWithConditionsGeneratorTests
    {
        [Test]
        [TestCase(0, "010001", "ABCD")]
        [TestCase(2048, "03", "ABCD")]
        public void ShouldFailWithBadParameters(int nlen, string e, string seed)
        {
            var hashFunction = new HashFunction
            {
                Mode = ModeValues.SHA1,
                DigestSize = DigestSizes.d160
            };
            var subject = new ProvableProbablePrimesWithConditionsGenerator(hashFunction, EntropyProviderTypes.Random);
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(1024, "0100000001", "5c029cd058da46698662234f46ca7fc9eabe138c")]
        [TestCase(1024, "d70ff9", "5c029cd058da46698662234f46ca7fc9eabe138c")]
        [TestCase(2048, "0100000001", "5c029cd058da46698662234f46ca7fc9eabe138cb20173cc1559100b")]
        [TestCase(2048, "d70ff9", "5c029cd058da46698662234f46ca7fc9eabe138cb20173cc1559100b")]
        [TestCase(3072, "0100000001", "d37b22fa27c9d330b9be0dd829c736ab87128aaae055a4f26c070693bb26e9df")]
        [TestCase(3072, "d70ff9", "d37b22fa27c9d330b9be0dd829c736ab87128aaae055a4f26c070693bb26e9df")]
        public void ShouldPassWithGoodParameters(int nlen, string e, string seed)
        {
            var hashFunction = new HashFunction
            {
                Mode = ModeValues.SHA1,
                DigestSize = DigestSizes.d160
            };
            var subject = new ProvableProbablePrimesWithConditionsGenerator(hashFunction, EntropyProviderTypes.Random);
            subject.SetBitlens(208, 231, 144, 244);
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase(2048, ModeValues.SHA1, DigestSizes.d160,
            208, 231, 144, 244, "0100000001", "54c64aa7e8227dfa65668e116df2feb258cbe91ff3deea960288baa3",
            "c6a3628a9d242c96f75faea441c4a7508bace05c1e981242f2b7cda6d2f817fe9fa24cfd29d03a0c876c2ddc39a3b8de8da8477b00e53b6fd6d68f8ea19a177ab5bc51dd3e7d994ec5b780ffe95fbd972116dbfdeba595f451e8a979feb79ee323592e5e743c02b5e5411694447bb6991c2f51ffe35cc0eb05900bda97aaf9d7",
            "cf2e6b79f20e664a7301294417ed4ad6d274afd6289f9e61c9ab38d6e3e45f830c14cf9cbfa9574aa53cc1ac8f52bba40067c2ceaad1acb7a7284f9a2e57cb9548829eb26dbbac5b66c36f331a61e532cbead1df2db5abfb13cfe42fe29afab14e39266de29756873bbdd96fb8eb289fcbf1e04d9dfab97af54ee869aaf26497")]
        [TestCase(2048, ModeValues.SHA2, DigestSizes.d224,
            248, 192, 192, 232, "0100000001", "0f187bbf5121fe03b2f19d0381b21ad6b448c5a495f5a59eb27969c6",
            "c4bb9e5814323b078e2785e7a3fb1b2791a537725231e563d16f24d82f7ba582968cca78789211d6731826ff8bf8eab2203d2a0c629dc2d9a46906ba626e261ac943317c2fada46206ea3400db0909287c0fcae955d07a913ed7bcb70507555b966a5f98b73fc476f3026492ed55a5d4b1147c83a12ac4949881389a1c5cb65d",
            "b9c56d750b2e7dfaf5cbbc1f31e2b1559f77085f4a6bc461ad497c5679653b8fafe95acb5484b750c5a1a27709760ed2976a70ef14835a7e5bc7d2e78dc498999643fadccd74ebebf9a0c4fe4c72813699514171702fc0185ac19b8fd5e778c72328a2bf811851e4ed9d83bb58e8d78dbec3bc63c63aadbfbe2b76692a33c0e5")]
        [TestCase(3072, ModeValues.SHA2, DigestSizes.d512,
            440, 190, 560, 189, "0100000001", "cad20639f564334910aed056da3b56f4253c5150632627f8d4f5b8803e941f4d",
            "de530010f038b8dd337a266cd03e08082da825cd60161cfb3d891ec98634257f6e9a3712f48df6cae381f90253d52a828f76c3573e1d596b90316480224768d7519c9f1b9ab59ccd0d2f2e67679cf6c470bd3482483e277a957ec0b49338850efa5d9f4efc90240e89e8a0699f4e2a705bc10fe3e47ccee9dbe14283b71b931f49a562f0cbd6d1fb2ba5f0506ae1a10e6cd5095310dc99c8f7260cad714d301da3b75b36cbf4069198d0cbc79ec0d522d62a02515c1d0b8fe444f889c2be4f23",
            "d7faa626e6092e403ade08a16757818a26a9ec76da4b515ee0e6ac6af6676b74fa69c2820bcd03dd0d17057a013bcca553f00ac08c6bda148685ed6ab9905ccf320c9a5965347826b451412797e92351202655c291bc0a213096507339c2f2780bbef50a0e563e66ea1a1fb994a8a692ac47d77a663085ee82f9ed1e4e66cdf80d71aef30aba7613ea3aea3f2910460bf4174f94ce3ac0d7591a892169858849098c8f1f959344c86e36281129c2e2ef96cd89288c379379bd81fd384fdca6ef")]
        public void ShouldCorrectlyGeneratePrimes(int nlen, ModeValues mode, DigestSizes dig, int bitlen1, int bitlen2,
            int bitlen3, int bitlen4, string e, string seed, string p, string q)
        {
            var subject = new ProvableProbablePrimesWithConditionsGenerator(new HashFunction { Mode = mode, DigestSize = dig }, EntropyProviderTypes.Random);
            subject.SetBitlens(bitlen1, bitlen2, bitlen3, bitlen4);

            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(new BitString(p).ToPositiveBigInteger(), result.P, "p");
            Assert.AreEqual(new BitString(q).ToPositiveBigInteger(), result.Q, "q");
        }
    }
}
