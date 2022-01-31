using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Builders;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA_KeyGen.IntegrationTests
{
    [TestFixture, UnitTest]
    public class DeferredCryptoResolverTests
    {
        [Test]
        [TestCase(2048, PrimeTestModes.TwoPow100ErrorBound, "e021757c777288dacfe67cb2e59dc02c70a8cebf56262336592c18dcf466e0a4ed405318ac406bd79eca29183901a557db556dd06f7c6bea175dcb8460b6b1bc05832b01eedf86463238b7cb6643deef66bc4f57bf8ff7ec7c4b8a8af14f478980aabedd42afa530ca47849f0151b7736aa4cd2ff37f322a9034de791ebe3f51", "ed1571a9e0cd4a42541284a9f98b54a6af67d399d55ef888b9fe9ef76a61e892c0bfbb87544e7b24a60535a65de422830252b45d2033819ca32b1a9c4413fa721f4a24ebb5510ddc9fd6f4c09dfc29cb9594650620ff551a62d53edc2f8ebf10beb86f483d463774e5801f3bb01c4d452acb86ecfade1c7df601cab68b065275")]
        [TestCase(2048, PrimeTestModes.TwoPowSecurityStrengthErrorBound, "fb61c111b038153b645cdd3103fc5eb3e9ab09b64d11de97a08662c569fb22456203fa5fc6b7e41a8e83fe995eeaea9cca670575a662447d39012aa093a051e781df6018c0ea8ab76d49353363074e92f070dfe3c3c8964acad4532da8bea7b0944ffd229f06da23abe7b050418abe4b44513777b988ab30ee696ef053e23ca5", "c0ef0f196921eea05721308d4edca39afd20d0dbd6c6c446571f69d6c873838558c8bd2e3a5bee4b7d32de9819caf9f07d3807a16616081275263789adb5c1d092f9d0001486fde649998d15650b1e442e0076cacf5b276d6d52cbbe1ec713237ff0f59460967515914aed67eb806e92bc9a0affb27de9c5c74fa9aefa357627")]
        [TestCase(3072, PrimeTestModes.TwoPow100ErrorBound, "b9c53dd71792a98fd35eaa569079dfc1f0f6dad9a4a50ca589cccdd80b7810c00c4c0b0a74d3c6ead42c2fa3478c5bfde09ffcad4cb793564fc83977ef1de96a11b16e5eb58590720715c10ac620b862cee5081934c5ddd3e3765fb848781af882558cc4f79663d7fff0263401adc832bc29d396a0c9916ed96005b79bf0dbead4158a3139c855f8d9ae83433410ef5fbdbbe9082ccb3b266c374a08ecca3a2d51bca0495766109ef471c9e07e098a809c9fdbdcada5aaeb11dfa36ca59991b5", "ed98c73529938fb891869c7ecc7de069af00abc5896e4ec1b32528feac69f29bfc93c707aec4921ac8191e7dde69272b97eebcd568641edf7dde60632ed075b93712870e4eccbeceefa06bade9d4fe2dc7c8ce6277371f3471f42d201831e9f95c8a6ac3d63dd47058e13b7d8e420d9790a17bc58470b5c130f84fdc39a7cfac3453f3706cc4118900710bed26deca871bfee3aa6c59263d314b969ef228b7d08ecec99acaba3466d25b99ecfa48388cc53b19ca74deefc6dfd3d1a80804f4c5")]
        [TestCase(3072, PrimeTestModes.TwoPowSecurityStrengthErrorBound, "cb18f35471d85487b94aac37c3fdd84dde3e8a6619085bb80805239e7d55fb8df3b513a8d3804edc994524eeffdcafa54d8739e23f7368bf910390d6572f31b35680b3f90b10b13e1ca48f78d02a03322f6f8b51f2903e1cd6e96a6aacb105416321d7d5fe7c606d58518f5f28d2ca1c56f1193ebfd253f57523e472275081a446feef773185b4cdd3782dfcbd2b906b1595b5da2d4110ac222cd460bb0742ec6a95a39ff407cd425130eba5b1b486f0236163bd9e929e219cbb868b1f16d517", "ed012e3f4ff86a411e23c13e3a7cf8e7807572c1374fb392ec6c98ef72677cc0a374523b8c7d809050c0657697a2081cd8147463a33ca80a46eba8708f6dc42f6f60a1315d6cbf026f3c504d7f697f097fb4ec700ff45f338b133cc83e1b9b3feb4123e41254824364086ff181364a7e6344600f52731a0739619b8a9707ab682e0f20867fafea0c322af5fed80ee1f634fcc9f58e2f8256645c08937e48d19c95c77c14105c9b2f39a800ff647d67f970a67eb5e91a61355103f3de313a9ea9")]
        public async Task ShouldValidateIfSuppliedResultIsPrime(int mod, PrimeTestModes pt, string hexP, string hexQ)
        {
            var serverTestCase = GetTestCase("", "");
            var suppliedTestCase = GetTestCase(hexP, hexQ);
            var testGroup = GetTestGroup(mod, pt);

            var subject = new DeferredTestCaseResolverGDT(new OracleBuilder().Build().GetAwaiter().GetResult());

            var result = await subject.CompleteDeferredCryptoAsync(testGroup, serverTestCase, suppliedTestCase);

            Assert.That(result != null);
            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase(2048, PrimeTestModes.TwoPow100ErrorBound, "e021757c777288dacfe67cb2e59dc02c70a8cebf56262336592c18dcf466e0a4ed405318ac406bd79eca29183901a557db556dd06f7c6bea175dcb8460b6b1bc05832b01eedf86463238b7cb6643deef66bc4f57bf8ff7ec7c4b8a8af14f478980aabedd42afa530ca47849f0151b7736aa4cd2ff37f322a9034de791ebe3f51", "c1e2aa6c64e3e05bde7ca7824833f1ee211d3903d6415a1425da47765d6766eccb348c59acc1ac6ce098b3dcf7e931254f300525872140346d927a438363b0dc0a42babba5e34882fae0cbf8b6d219b904d963073fd636f117fd2ef96c5cd9aa97c741e865b1db18b4c0ed5d64f944c75763d2138005dae5d3df6040c15a08d9")]
        [TestCase(2048, PrimeTestModes.TwoPowSecurityStrengthErrorBound, "e534f4a4eb86ff9ace08a0b446faf3e20c22a0166057507e4f5f07332d5c0878a50798857d5e9946e3f8ef8a1021481bb0c94631f9ad8427df620ec9ca585cab3082222279f41bc40e2ccdc160dbc410c52662699ae16b27b2c9d2bf14e99083920a448ba4e5d3d11e1ab7777613959c07fb213be26f2cb7ea8a759af082f6c5", "e021757c777288dacfe67cb2e59dc02c70a8cebf56262336592c18dcf466e0a4ed405318ac406bd79eca29183901a557db556dd06f7c6bea175dcb8460b6b1bc05832b01eedf86463238b7cb6643deef66bc4f57bf8ff7ec7c4b8a8af14f478980aabedd42afa530ca47849f0151b7736aa4cd2ff37f322a9034de791ebe3f51")]
        [TestCase(3072, PrimeTestModes.TwoPow100ErrorBound, "b9c53dd71792a98fd35eaa569079dfc1f0f6dad9a4a50ca589cccdd80b7810c00c4c0b0a74d3c6ead42c2fa3478c5bfde09ffcad4cb793564fc83977ef1de96a11b16e5eb58590720715c10ac620b862cee5081934c5ddd3e3765fb848781af882558cc4f79663d7fff0263401adc832bc29d396a0c9916ed96005b79bf0dbead4158a3139c855f8d9ae83433410ef5fbdbbe9082ccb3b266c374a08ecca3a2d51bca0495766109ef471c9e07e098a809c9fdbdcada5aaeb11dfa36ca59991b5", "bf55709785a97ada46eeb22edcdd442a2fc891326763278ae99726b096022d939c751bdfacdbf9361b5d9753f00859c494c846db21fdfd01bdf1fa846380cbc4c5a0bb8bbe328841e24d54aebc27b0694c95c5d64465966d5716e049e3e152acc7c4a5b1f08595112a51fe57f93e32f76fac031e8e1e4e6954e59907dd9c3bf7b158ca7cd603c3e24febd9b02b60646e22f43bd4838b7d4529f59f4104703fa360f3bb6dd4a6a61ef4066739b2fa2f12f601cbed87f30cf6b77b36557f303027")]
        [TestCase(3072, PrimeTestModes.TwoPowSecurityStrengthErrorBound, "eff94ea2ed134318a36e4b103fc0db5ddab00dfd4b6c741d44d2630951da75524f0b2d5510ee125ed3b88d6a67841faa8bd36d6149aa7a43bcabd106960bd29b26e6bc963f6513929491ab0ae286642020a1f96fd490370fd2de28f971c1c40b6cfacd7a1c0e27d17bd943870d973774d4a4e2aa8c92b97c313c47365f8e819e9f619ba08932ca8ffd580d402eeec25c76b3ddb704835536d4c984e7e0086a9163d8742c72e8a219f5381cfc23dac1dd9e917b7f666fc9c68616ee063ca0c4ba", "b9c53dd71792a98fd35eaa569079dfc1f0f6dad9a4a50ca589cccdd80b7810c00c4c0b0a74d3c6ead42c2fa3478c5bfde09ffcad4cb793564fc83977ef1de96a11b16e5eb58590720715c10ac620b862cee5081934c5ddd3e3765fb848781af882558cc4f79663d7fff0263401adc832bc29d396a0c9916ed96005b79bf0dbead4158a3139c855f8d9ae83433410ef5fbdbbe9082ccb3b266c374a08ecca3a2d51bca0495766109ef471c9e07e098a809c9fdbdcada5aaeb11dfa36ca59991b5")]
        public async Task ShouldNotValidateIfSuppliedResultIsComposite(int mod, PrimeTestModes pt, string hexP, string hexQ)
        {
            var serverTestCase = GetTestCase("", "");
            var suppliedTestCase = GetTestCase(hexP, hexQ);
            var testGroup = GetTestGroup(mod, pt);

            var subject = new DeferredTestCaseResolverGDT(new OracleBuilder().Build().GetAwaiter().GetResult());

            var result = await subject.CompleteDeferredCryptoAsync(testGroup, serverTestCase, suppliedTestCase);

            Assert.That(result != null);
            Assert.IsFalse(result.Success);
        }

        private TestCase GetTestCase(string hexP, string hexQ)
        {
            return new TestCase
            {
                TestCaseId = 1,
                Key = new KeyPair
                {
                    PubKey = new PublicKey
                    {
                        E = new BitString("010001").ToPositiveBigInteger()
                    },
                    PrivKey = new PrivateKey
                    {
                        P = new BitString(hexP).ToPositiveBigInteger(),
                        Q = new BitString(hexQ).ToPositiveBigInteger()
                    }
                }
            };
        }

        private TestGroup GetTestGroup(int modulo, PrimeTestModes primeTest)
        {
            return new TestGroup
            {
                Modulo = modulo,
                PrimeTest = RsaKeyGenAttributeConverter.GetSectionFromPrimeTest(primeTest),
                PrimeGenMode = RsaKeyGenAttributeConverter.GetSectionFromPrimeGen(PrimeGenModes.RandomProbablePrimes),
                KeyFormat = PrivateKeyModes.Standard,
                PubExp = PublicExponentModes.Random
            };
        }
    }
}
