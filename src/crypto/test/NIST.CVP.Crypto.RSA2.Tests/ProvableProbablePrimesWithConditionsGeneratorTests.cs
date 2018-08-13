using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests
{
    // Takes 5 minutes 0 seconds
    [TestFixture, LongCryptoTest]
    public class ProvableProbablePrimesWithConditionsGeneratorTests
    {
        [Test]
        [TestCase(0, "010001", "ABCD", new[] {200, 200, 200, 200})]
        [TestCase(2048, "03", "ABCD", new[] {200, 200, 200, 200})]
        [TestCase(2048, "010001", "ABCD", new[] {0, 200, 200, 200})]
        [TestCase(2048, "010001", "ABCD", new[] {200, 200, 200})]
        public void ShouldFailWithBadParameters(int nlen, string e, string seed, int[] bitlens)
        {
            var sha = new ShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));
            var subject = new ProvableProbablePrimesWithConditionsGenerator(sha, new EntropyProvider(new Random800_90()));
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            subject.SetBitlens(bitlens);
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
            var sha = new ShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));
            var subject = new ProvableProbablePrimesWithConditionsGenerator(sha, new EntropyProvider(new Random800_90()));
            subject.SetBitlens(208, 231, 144, 244);
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase(2048, ModeValues.SHA1, DigestSizes.d160,
            208, 231, 144, 244, "0100000001", "54c64aa7e8227dfa65668e116df2feb258cbe91ff3deea960288baa3",
            "c6a3628a9d242c96f75faea441c4a7508bace05c1e981242f2b7cda6d2f817fe9fa24cfd29d03a0c876c2ddc39a3b8de8da8477b00e53b6fd6d68f8ea19a177ab5bc51dd3e7d994d2bf49858798d04e37bee50958471dfa850aec16b8b5297f231ddc24695049a747194646cbfd0eddcb0775da8b333a7b22e737e578d7dabcb",
            "cf2e6b79f20e664a7301294417ed4ad6d274afd6289f9e61c9ab38d6e3e45f830c14cf9cbfa9574aa53cc1ac8f52bba40067c2ceaad1acb7a7284f9a2e57cb9548829eb26dbbac5b66c36f331a61e3166ccf375508e82e01b634f803c119db95d0c5ec0669b9eb5ca3d7900cc1c9cbc95132b96035140b7d8753f58362b19a96",
            "c6a3628a9d242c96f75faea441c4a7508bace05c1e981242f2b7cda6d2f817fe9fa24cfd29d03a0c876c2ddc39a3b8de8da8477b00e53b6fd6d68f8ea19a177ab5bc51dd3e7d994ec5b780ffe95fbd972116dbfdeba595f451e8a979feb79ee323592e5e743c02b5e5411694447bb6991c2f51ffe35cc0eb05900bda97aaf9d7",
            "cf2e6b79f20e664a7301294417ed4ad6d274afd6289f9e61c9ab38d6e3e45f830c14cf9cbfa9574aa53cc1ac8f52bba40067c2ceaad1acb7a7284f9a2e57cb9548829eb26dbbac5b66c36f331a61e532cbead1df2db5abfb13cfe42fe29afab14e39266de29756873bbdd96fb8eb289fcbf1e04d9dfab97af54ee869aaf26497",
            TestName = "ShouldCorrectlyGeneratePrimes - ProvableProbablePrimesWithConditionsGenerator - 2048, 208, 231, 144, 244"
        )]
        [TestCase(2048, ModeValues.SHA2, DigestSizes.d224,
            248, 192, 192, 232, "0100000001", "0f187bbf5121fe03b2f19d0381b21ad6b448c5a495f5a59eb27969c6",
            "c4bb9e5814323b078e2785e7a3fb1b2791a537725231e563d16f24d82f7ba582968cca78789211d6731826ff8bf8eab2203d2a0c629dc2d9a46906ba626e261ac943317c2fada45eccc3b366740957c7d4fabbf78785e624cd2b381fa7ee9525f0f3633e43e7a072d62c5ef99697e91ea890a3998a8f56b2e01eca5fb42773b7",
            "b9c56d750b2e7dfaf5cbbc1f31e2b1559f77085f4a6bc461ad497c5679653b8fafe95acb5484b750c5a1a27709760ed2976a70ef14835a7e5bc7d2e78dc498999643fadccd74ebebf99cd4ef19657ac7e6b6a1f3f7c69608e2214e35c14912112fac539b37f8c6503dfae2c3077ca524312402f451094a4d76851bea02ae34e4",
            "c4bb9e5814323b078e2785e7a3fb1b2791a537725231e563d16f24d82f7ba582968cca78789211d6731826ff8bf8eab2203d2a0c629dc2d9a46906ba626e261ac943317c2fada46206ea3400db0909287c0fcae955d07a913ed7bcb70507555b966a5f98b73fc476f3026492ed55a5d4b1147c83a12ac4949881389a1c5cb65d",
            "b9c56d750b2e7dfaf5cbbc1f31e2b1559f77085f4a6bc461ad497c5679653b8fafe95acb5484b750c5a1a27709760ed2976a70ef14835a7e5bc7d2e78dc498999643fadccd74ebebf9a0c4fe4c72813699514171702fc0185ac19b8fd5e778c72328a2bf811851e4ed9d83bb58e8d78dbec3bc63c63aadbfbe2b76692a33c0e5",
            TestName = "ShouldCorrectlyGeneratePrimes - ProvableProbablePrimesWithConditionsGenerator - 2048, 248, 192, 192, 232"
        )]
        [TestCase(3072, ModeValues.SHA2, DigestSizes.d512,
            440, 190, 560, 189, "0100000001", "cad20639f564334910aed056da3b56f4253c5150632627f8d4f5b8803e941f4d",
            "de530010f038b8dd337a266cd03e08082da825cd60161cfb3d891ec98634257f6e9a3712f48df6cae381f90253d52a828f76c3573e1d596b90316480224768d7519c9f1b9ab59ccd0d2f2e67679cf6c470bd3482483e277a957ec0b49338850efa5d9f4efc90240e89e8a0699f4e2a701daa43d74186ca9b4ff033e759eeeac6ac79e6558e51d33834eeba18ff645701bdba4c6db092ecf6a50aa2d3efc8ea743453b567a7b5dcb5d5c7cc63f12eec87259ad56ef1d055eda084c55cf6c1e0b3",
            "d7faa626e6092e403ade08a16757818a26a9ec76da4b515ee0e6ac6af6676b74fa69c2820bcd03dd0d17057a013bcca553f00ac08c6bda148685ed6ab9905ccf320c9a5965347826b451412797e92351202655c291bc0a213096507339c2f2780a2fb48a7a5fd2e9205cc6629eb81b63a657ee654d9f8087a8cb8eb13d0e57bbcf2c48f0cda1fe07577955a4b57303174efaf7a20b21680156ae799718cde1b754542a2ca2fd3486a27ef05b34206169ec5997bf0e04137572566edcd2e58b26",
            "de530010f038b8dd337a266cd03e08082da825cd60161cfb3d891ec98634257f6e9a3712f48df6cae381f90253d52a828f76c3573e1d596b90316480224768d7519c9f1b9ab59ccd0d2f2e67679cf6c470bd3482483e277a957ec0b49338850efa5d9f4efc90240e89e8a0699f4e2a705bc10fe3e47ccee9dbe14283b71b931f49a562f0cbd6d1fb2ba5f0506ae1a10e6cd5095310dc99c8f7260cad714d301da3b75b36cbf4069198d0cbc79ec0d522d62a02515c1d0b8fe444f889c2be4f23",
            "d7faa626e6092e403ade08a16757818a26a9ec76da4b515ee0e6ac6af6676b74fa69c2820bcd03dd0d17057a013bcca553f00ac08c6bda148685ed6ab9905ccf320c9a5965347826b451412797e92351202655c291bc0a213096507339c2f2780bbef50a0e563e66ea1a1fb994a8a692ac47d77a663085ee82f9ed1e4e66cdf80d71aef30aba7613ea3aea3f2910460bf4174f94ce3ac0d7591a892169858849098c8f1f959344c86e36281129c2e2ef96cd89288c379379bd81fd384fdca6ef",
            TestName = "ShouldCorrectlyGeneratePrimes - ProvableProbablePrimesWithConditionsGenerator - 3072, 440, 190, 560, 189"
        )]
        [TestCase(3072, ModeValues.SHA2, DigestSizes.d256,
            392, 357, 344, 273, "0100000001", "00c43e35054d734394b4448eeaeb8724173aa0955357af8545ce24411f62cf7e",
            "fc213d30093bdb30ae037f1ad6ee400001789a883ee431d9cbd5bee2f9c36fd533683342526ffcf03d94552233e816f26d2b8ef96ba1b21ccacc7207ebb0fba031fb3eab934c43984c97681adecaf87848b474ed40e4678f4ffcb97caa1ba4b68537e9e7a048306bb8fc25ce23280ee625146351fe1b0ed78db2522f9334334f570b3a5241988c9cb33a63c8b9026668329c77f7a9d9782ab240b94542e74cb4569a79e8b9bcebc7d10b1c19d288ecab2b3873c25d9743988dc1747d4d6eb3da",
            "cc68a5244b4eb734f1c4d6d3264cdc085d83ad466720a6ac2eddfdab1f8be565b71b17a32b2f9a22ea041605279051c6370ae3996320be4db194b75df97a71d89d82191c26de3ba7e0e8d8796600e77665199d34ff41cc9f460ce56c659a730958bfe3d6e235368a17c974abfeb1ca60132429d8eb479dca96102842c4c3f5f5564ceb1323fe97a515aec8da58fcd7ba058564d7a25ca8197089806fea1d403ebd8cc532c2f9c64b271b879f6366e813bd22ff44c77150911d529f87ab4ee30c",
            "fc213d30093bdb30ae037f1ad6ee400001789a883ee431d9cbd5bee2f9c36fd533683342526ffcf03d94552233e816f26d2b8ef96ba1b21ccacc7207ebb0fba031fb3eab934c43984c97681adecaf87848b474ed40e4678f4ffcb97caa1ba4b685a1a3e45f95b71c440bbfac8df9ece82ee3bbb94660658abae0c77c88c04803650f2380ee17a8ad73a52a2e8d29f61ab9022bf6c63f61d8e49491179c0a6c6d8ef0a3811a1f955408ef854fa908758e853385fbb5e0e64283a2ab98f7250027",
            "cc68a5244b4eb734f1c4d6d3264cdc085d83ad466720a6ac2eddfdab1f8be565b71b17a32b2f9a22ea041605279051c6370ae3996320be4db194b75df97a71d89d82191c26de3ba7e0e8d8796600e77665199d34ff41cc9f460ce56c659a730958bfe3d6e235368a17c974abfeb1ca601357b4e12f49c121d36306c6a23ec95736590d085319ca5ba2802f19469ba26ca171f1eaf00ae8f8b04ac48eb955c91df8d561e8c3652f94e53da89750c3d1517d564aed78b0e4c6d0858920eff875a5",
            TestName = "ShouldCorrectlyGeneratePrimes - ProvableProbablePrimesWithConditionsGenerator - 3072, 392, 357, 344, 273"
        )]
        [TestCase(3072, ModeValues.SHA2, DigestSizes.d256,
            480, 189, 200, 190, "0100000001", "e0908d38462ad10362fe250ad20b4dbbeee802a7c596e8221e6b0a169af65daf",
            "d2f505ec914ea43a07f83b215301deccf024983eec2876c67126f8db74b2954eb67901248e8585703f56bd622683e5bd0d8286b450dbdbf3ba0d9dc22fc0dfb6bf4e276d2435c5903d125aaa2d7f6877367d7e4465eb97552f62a1deb9c64979ea2e9e94e4dbc6e41927fffdb19bf322eeaee2b076bcd9bc33d3ec2f1471fa4b180527fb6d551e7ed315160a6fbf3ccdbe1cab07d90acf7956f3acd686d756ffd9e115e55ce2845d8005280cf9b4fb78cd28612c3167076f2fabd9275daff32a",
            "f3fc409be7b7f0af4f6d5909ebfe5fcb5e1ff0ad4dec33b8733ca878cc414ec7f7efe202e998d57a8081f598c2ff5c5c373ad50d86bfb0af3df9f76890e5f929025e12c7f7c57661d97a37c716474419586b4d4361363c2f9f43c8f3f3e6882986f9b12a705cc47f244fc84d6396a3090e3cddd1ae643b67cdd58bc500fd504e8fbc5caf67433a682f04a5b54ec1fde81e0885099b8c3598d20d0dc906b2e24829fef9d2a439a98875bbb0a9cdc223100adece430d291f060edfce41e998247f",
            "d2f505ec914ea43a07f83b215301deccf024983eec2876c67126f8db74b2954eb67901248e8585703f56bd622683e5bd0d8286b450dbdbf3ba0d9dc22fc0dfb6bf4e276d2435c5903d125aaa2d7f6877367d7e4465eb97552f62a1deb9c64979ea2e9e94e4dbc6e4192800488014a6654209284e5182fa425a4b0116b216cd969de266c937046d2029b4be588de8c1e0aad974642bc6ef816475a7be71e9a8b63c87fd802bacabe925ee8a33d987335f4276b44a742128091cac3970250c2e15",
            "f3fc409be7b7f0af4f6d5909ebfe5fcb5e1ff0ad4dec33b8733ca878cc414ec7f7efe202e998d57a8081f598c2ff5c5c373ad50d86bfb0af3df9f76890e5f929025e12c7f7c57661d97a37c716474419586b4d4361363c2f9f43c8f3f3e6882986f9b12a705cc47f244fc84d6396a3090e3cddd1ae643b67cdd58bc500fd504e8fbc5caf67433a682f04a5b54ec3b934baa2116d08db133c8aa1a84fdd37a43f9aeeb4803193396741daca9151488db9bbfa77aa730917490d32193d2ff0127b",
            TestName = "ShouldCorrectlyGeneratePrimes - ProvableProbablePrimesWithConditionsGenerator - 3072, 480, 189, 200, 190"
        )]
        public void ShouldCorrectlyGeneratePrimes(int nlen, ModeValues mode, DigestSizes dig, int bitlen1, int bitlen2,
            int bitlen3, int bitlen4, string e, string seed, string xp, string xq, string p, string q)
        {
            var sha = new ShaFactory().GetShaInstance(new HashFunction(mode, dig));
            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(new BitString(xp).ToPositiveBigInteger());
            entropyProvider.AddEntropy(new BitString(xq).ToPositiveBigInteger());

            var subject = new ProvableProbablePrimesWithConditionsGenerator(sha, entropyProvider);
            subject.SetBitlens(bitlen1, bitlen2, bitlen3, bitlen4);

            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(new BitString(p).ToPositiveBigInteger(), result.Primes.P, "p");
            Assert.AreEqual(new BitString(q).ToPositiveBigInteger(), result.Primes.Q, "q");
        }
    }
}
